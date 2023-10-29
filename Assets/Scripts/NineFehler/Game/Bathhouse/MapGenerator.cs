using System.Collections.Generic;
using NineFehler.Game.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NineFehler.Game.Bathhouse
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private Level _startLevelPrefab;
        [SerializeField] private Level[] _levelPrefabs;
        [SerializeField] private Transform _levelsContainer;
        [SerializeField] private PlayerController _player;

        private List<Level> _levels;
        private int _currentLevelIndex;

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

        private void Level_OnDoorOpened()
        {
            OpenNextLevel();
        }
    }
}
