using System;
using UnityEngine;

namespace NineFehler.Game.Bathhouse
{
    public class LevelDoor : MonoBehaviour, IPlayerInteractable
    {
        public Action<bool> OnOpen;

        [SerializeField] private bool _isLocked;

        public void Interact()
        {
            OnOpen?.Invoke(_isLocked);
        }
    }
}