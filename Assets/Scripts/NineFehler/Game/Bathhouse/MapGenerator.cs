using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private GameObject _winWindow;
        [SerializeField] private GameObject _loadingAnim;

        private List<Level> _levels;
        private int _currentLevelIndex;
        private int _totalLevelsCount = 10;
        private int _currentLevelsCount = 0;

        private void Start()
        {
            _levels = new List<Level>();
            PrepareLevels();
        }

        private void PrepareLevels()
        {
            Level startLevel = Instantiate(_startLevelPrefab, _levelsContainer);
            startLevel.OnDoorOpened += Level_OnDoorOpened;

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
            _currentLevelIndex = Random.Range(0, _levelPrefabs.Length);
            _levels[_currentLevelIndex].gameObject.SetActive(true);
            _player.SetPlacement(
                _levels[_currentLevelIndex].PlayerSpawnPoint.position,
                _levels[_currentLevelIndex].PlayerSpawnPoint.rotation);
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

        private void Level_OnDoorOpened()
        {
            if (_currentLevelsCount >= _totalLevelsCount)
                PlayerWin();
            else
            {
                _currentLevelsCount++;
                OpenNextLevel();
            }
        }
    }
}