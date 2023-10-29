using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NineFehler.Game.Outdoor
{
    public class RandomScream : MonoBehaviour
    {
        [SerializeField] private AudioSource[] _audioSources;

        private void Start()
        {
            StartCoroutine(ScreamsCoroutine());
        }

        private IEnumerator ScreamsCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(15f, 20f));
                
                _audioSources[Random.Range(0, _audioSources.Length)].Play();
            }
        }
    }
}
