using System;
using UnityEngine;

namespace NineFehler.Game.Map
{
    public class Level : MonoBehaviour
    {
        public Action OnDoorOpened;
        
        [SerializeField] private LevelDoor _enterDoor;
        [SerializeField] private LevelDoor[] _exitDoors;

        private void Start()
        {
            _enterDoor.OnOpen += Door_OnOpen;
            foreach (LevelDoor door in _exitDoors)
                door.OnOpen += Door_OnOpen;
        }
        
        private void Door_OnOpen(bool isLocked)
        {
            if (!isLocked)
                OnDoorOpened?.Invoke();
        }
    }
}