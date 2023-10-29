using System;
using UnityEngine;

namespace NineFehler.Game.Outdoor
{
    public class TreesSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _treePrefab;
        [SerializeField] private Transform[] _treesContainers;

        private void Start()
        {
            foreach (Transform container in _treesContainers)
                Instantiate(_treePrefab, container);
        }
    }
}