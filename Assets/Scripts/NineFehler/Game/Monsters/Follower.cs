using System;
using NineFehler.Game.Player;
using NineFehler.Utils;
using UnityEngine;

namespace NineFehler.Game.Monsters
{
    public class Follower : MonoBehaviour
    {
        public Action OnPlayerCatch;
        
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private AudioSource _audioSource;

        public void Set(Vector3 pos)
        {
            transform.position = pos;
            _audioSource.Play();
        }

        private void Update()
        {
            Vector3 forwardVector = (_playerTransform.position - transform.position).normalized;
            transform.position += forwardVector * Constants.FollowerSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(forwardVector, Vector3.up);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag.Equals("Player"))
            {
                OnPlayerCatch?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}