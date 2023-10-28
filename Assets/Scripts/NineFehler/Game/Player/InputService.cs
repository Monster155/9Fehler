using System;
using UnityEngine;

namespace NineFehler.Game.Player
{
    public class InputService : MonoBehaviour
    {
        public float Forward { get; private set; }
        public float Right { get; private set; }
        public float RotationX { get; private set; }
        public float RotationY { get; private set; }
        public bool IsInteract { get; private set; }

        private void Start()
        {
            Cursor.visible = false;
        }

        private void FixedUpdate()
        {
            Forward = Input.GetAxis("Vertical");
            Right = Input.GetAxis("Horizontal");

            RotationX = Input.GetAxis("Mouse X");
            RotationY = -Input.GetAxis("Mouse Y");

            IsInteract = Input.GetKey(KeyCode.E);
        }
    }
}