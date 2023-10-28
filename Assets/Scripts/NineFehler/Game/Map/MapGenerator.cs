using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NineFehler.Game.Map
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private Level[] _levelPrefabs;
        [SerializeField] private Transform _levelsContainer;

        private List<Level> _levels;
        private int _currentLevelIndex;

        private void Start()
        {
            PrepareLevels();
            OpenNextLevel();
        }

        private void PrepareLevels()
        {
            foreach (Level levelPrefab in _levelPrefabs)
            {
                Level level = Instantiate(levelPrefab, _levelsContainer);
                level.OnDoorOpened += Level_OnDoorOpened;
                _levels.Add(level);
            }
        }

        public void OpenNextLevel()
        {
            _levels[_currentLevelIndex].gameObject.SetActive(false);
            _currentLevelIndex = Random.Range(0, _levelPrefabs.Length);
            _levels[_currentLevelIndex].gameObject.SetActive(true);
        }

        private void Level_OnDoorOpened()
        {
            OpenNextLevel();
        }
    }
}
