using System.Collections;
using System.Collections.Generic;
using NineFehler.Game.Monsters;
using NineFehler.Game.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace NineFehler.Game.Bathhouse
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private Level _startLevelPrefab;
        [SerializeField] private Level[] _levelPrefabs;
        [SerializeField] private Transform _levelsContainer;
        [SerializeField] private PlayerController _player;
        [Space]
        [SerializeField] private Follower _follower;
        [Space]
        [SerializeField] private AudioSource _screamer;
        [SerializeField] private GameObject _winWindow;
        [SerializeField] private GameObject _loadingAnim;

        private List<Level> _levels;
        private int _currentLevelIndex;
        private int _currentLevelsCount = 0;
        private Level _startLevel;
        private Coroutine _followerCoroutine;

        private void Start()
        {
            _levels = new List<Level>();

            _screamer.gameObject.SetActive(false);

            _follower.gameObject.SetActive(false);
            _follower.OnPlayerCatch += Follower_OnPlayerCatch;

            PrepareLevels();

            _player.SetPlacement(
                _startLevel.PlayerSpawnPoint.position,
                _startLevel.PlayerSpawnPoint.rotation);
        }

        private void PrepareLevels()
        {
            _startLevel = Instantiate(_startLevelPrefab, _levelsContainer);
            _startLevel.OnDoorOpened += Level_OnDoorOpened;
            _startLevel.gameObject.SetActive(true);

            foreach (Level levelPrefab in _levelPrefabs)
            {
                Level level = Instantiate(levelPrefab, _levelsContainer);
                level.OnDoorOpened += Level_OnDoorOpened;
                _levels.Add(level);
                level.gameObject.SetActive(false);
            }
        }

        public void OpenNextLevel()
        {
            _levels[_currentLevelIndex].gameObject.SetActive(false);
            // _currentLevelIndex = Random.Range(0, _levelPrefabs.Length);
            _currentLevelIndex = _currentLevelsCount;
            _levels[_currentLevelIndex].gameObject.SetActive(true);
            _player.SetPlacement(
                _levels[_currentLevelIndex].PlayerSpawnPoint.position,
                _levels[_currentLevelIndex].PlayerSpawnPoint.rotation);
            if (_followerCoroutine != null)
                StopCoroutine(_followerCoroutine);
            _followerCoroutine = StartCoroutine(FollowerCoroutine());
        }

        private void PlayerWin()
        {
            _player.LockInput();
            _winWindow.SetActive(true);
            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("OutdoorScene");
            asyncLoad.allowSceneActivation = false;
            yield return new WaitForSeconds(3f);

            _loadingAnim.SetActive(true);
            yield return new WaitForSeconds(2f);

            asyncLoad.allowSceneActivation = true;
        }

        private IEnumerator FollowerCoroutine()
        {
            yield return new WaitForSeconds(4);

            _follower.gameObject.SetActive(true);
            _follower.Set(_levels[_currentLevelIndex].PlayerSpawnPoint.position);
        }

        private IEnumerator FollowerScreamerCoroutine()
        {
            _player.LockInput();
            _screamer.gameObject.SetActive(true);
            _screamer.Play();
            yield return new WaitForSeconds(_screamer.clip.length);

            _screamer.gameObject.SetActive(false);
            _follower.gameObject.SetActive(false);
            _player.UnlockInput();
        }

        private void Level_OnDoorOpened()
        {
            _follower.gameObject.SetActive(false);

            if (_currentLevelsCount == 0)
                _startLevel.gameObject.SetActive(false);

            if (_currentLevelsCount >= _levels.Count-1)
            {
                PlayerWin();
            }
            else
            {
                _currentLevelsCount++;
                OpenNextLevel();
            }
        }

        private void Follower_OnPlayerCatch()
        {
            StartCoroutine(FollowerScreamerCoroutine());
        }
    }
}