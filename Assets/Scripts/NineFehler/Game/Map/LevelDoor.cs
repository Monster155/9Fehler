using System;
using UnityEngine;

namespace NineFehler.Game.Map
{
    public class LevelDoor : MonoBehaviour
    {
        public Action<bool> OnOpen;

        [SerializeField] private bool _isLocked;

        public void Open()
        {
            OnOpen?.Invoke(_isLocked);
        }
    }
}