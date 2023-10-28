using System;
using System.Collections;
using NineFehler.Game.Player;
using UnityEngine;

namespace NineFehler.Game.Outdoor
{
    public class OutdoorController : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;
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

        private void Start()
        {
            _forwardLeftListener.OnEnter += ForwardLeftListener_OnEnter;
            _forwardRightListener.OnEnter += ForwardRightListener_OnEnter;
            _backListener.OnEnter += BackListener_OnEnter;
            foreach (TriggerListener deadZone in _deadZoneListeners)
                deadZone.OnEnter += DeadZone_OnEnter;

            SetPlayer(null, Quaternion.identity);
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
            yield return new WaitForSeconds(1f);

            SetPlayer(null, Quaternion.identity);
        }

        private void ForwardLeftListener_OnEnter(Collider c)
        {
            if (c.transform.tag.Equals("Player"))
                SetPlayer(_forwardLeftAnchor, _player.Rotation);
        }
        private void ForwardRightListener_OnEnter(Collider c)
        {
            if (c.transform.tag.Equals("Player"))
                SetPlayer(_forwardRightAnchor, _player.Rotation);
        }
        private void BackListener_OnEnter(Collider c)
        {
            if (c.transform.tag.Equals("Player"))
            {
                Vector3 playerRot = _player.Rotation.eulerAngles;
                playerRot.y = -playerRot.y;
                SetPlayer(_backAnchor, Quaternion.Euler(playerRot));
            }
        }
        private void DeadZone_OnEnter(Collider c)
        {
            if (c.transform.tag.Equals("Player"))
                StartCoroutine(DeadCoroutine());
        }
    }
}