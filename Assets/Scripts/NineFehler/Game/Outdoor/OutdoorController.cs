using System;
using System.Collections;
using NineFehler.Game.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NineFehler.Game.Outdoor
{
    public class OutdoorController : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private AudioSource _screamer;
        [SerializeField] private AudioSource _successSound;
        [Space]
        [SerializeField] private TriggerListener _forwardLeftListener;
        [SerializeField] private TriggerListener _forwardRightListener;
        [SerializeField] private TriggerListener _backListener;
        [SerializeField] private TriggerListener[] _deadZoneListeners;
        [Space]
        [SerializeField] private Transform _forwardLeftAnchor;
        [SerializeField] private Transform _forwardRightAnchor;
        [SerializeField] private Transform _backAnchor;
        [SerializeField] private Transform _spawnPoint;
        [Space]
        [SerializeField] private GameObject _winWindow;
        [SerializeField] private GameObject _loadingAnim;

        private bool[] _isLeftCorrect = new[] { false, false, false, true, false };
        private int _currentProgressIndex;

        private void Start()
        {
            _forwardLeftListener.OnEnter += ForwardLeftListener_OnEnter;
            _forwardRightListener.OnEnter += ForwardRightListener_OnEnter;
            _backListener.OnEnter += BackListener_OnEnter;
            foreach (TriggerListener deadZone in _deadZoneListeners)
                deadZone.OnEnter += DeadZone_OnEnter;

            SetPlayer(null, Quaternion.identity);
            _screamer.gameObject.SetActive(false);
            _winWindow.SetActive(false);
            _player.UnlockInput();
            _loadingAnim.SetActive(false);
        }

        private void SetPlayer(Transform anchor, Quaternion rot)
        {
            Vector3 placementVector = Vector3.zero;
            if (anchor != null)
            {
                placementVector = anchor.position - _player.Position;
                placementVector.x = -placementVector.x;
                placementVector.z = -placementVector.z;
            }

            _player.SetPlacement(_spawnPoint.position + placementVector, rot);
        }

        private IEnumerator DeadCoroutine()
        {
            _player.LockInput();
            _screamer.Play();
            _screamer.gameObject.SetActive(true);
            yield return new WaitForSeconds(_screamer.clip.length);

            _screamer.gameObject.SetActive(false);

            SetPlayer(null, Quaternion.identity);
            _player.UnlockInput();
        }
        private void PlayerWin()
        {
            _player.LockInput();
            _winWindow.SetActive(true);
            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MenuScene");
            asyncLoad.allowSceneActivation = false;
            yield return new WaitForSeconds(3f);
            _loadingAnim.SetActive(true);
            yield return new WaitForSeconds(2f);
            asyncLoad.allowSceneActivation = true;
        }

        private void ForwardLeftListener_OnEnter(Collider c)
        {
            if (c.transform.tag.Equals("Player"))
            {
                if (_isLeftCorrect[_currentProgressIndex])
                {
                    _successSound.Play();
                    _currentProgressIndex++;
                }
                else
                    _currentProgressIndex = 0;

                if (_currentProgressIndex >= _isLeftCorrect.Length)
                    PlayerWin();
                else
                    SetPlayer(_forwardLeftAnchor, _player.Rotation);
            }
        }
        private void ForwardRightListener_OnEnter(Collider c)
        {
            if (c.transform.tag.Equals("Player"))
            {
                if (!_isLeftCorrect[_currentProgressIndex])
                {
                    _successSound.Play();
                    _currentProgressIndex++;
                }
                else
                    _currentProgressIndex = 0;

                if (_currentProgressIndex >= _isLeftCorrect.Length)
                    PlayerWin();
                else
                    SetPlayer(_forwardRightAnchor, _player.Rotation);
            }
        }
        private void BackListener_OnEnter(Collider c)
        {
            if (c.transform.tag.Equals("Player"))
            {
                _currentProgressIndex = 0;
                Vector3 playerRot = _player.Rotation.eulerAngles;
                playerRot.y = -playerRot.y;
                SetPlayer(_backAnchor, Quaternion.Euler(playerRot));
            }
        }
        private void DeadZone_OnEnter(Collider c)
        {
            if (c.transform.tag.Equals("Player"))
            {
                _currentProgressIndex = 0;
                StartCoroutine(DeadCoroutine());
            }
        }
    }
}