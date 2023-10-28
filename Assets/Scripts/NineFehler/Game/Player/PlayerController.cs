using MimoProhodili.Utils;
using NineFehler.Game.Map;
using NineFehler.Utils;
using UnityEngine;

namespace NineFehler.Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputService _input;
        [SerializeField] private CharacterController _controller;
        [SerializeField] private Transform _playerChar;
        [SerializeField] private Transform _playerCamera;

        private bool _isPlacementSet;

        private void Update()
        {
            if (_isPlacementSet)
            {
                _isPlacementSet = false;
                return;
            }

            Move();
            Rotate();
            Interact();
        }

        public void SetPlacement(Vector3 pos, Quaternion rot)
        {
            _isPlacementSet = true;
            _controller.enabled = false;
            _controller.transform.position = pos;
            _controller.transform.rotation = rot;
            _controller.enabled = true;
        }

        private void Move()
        {
            Vector3 movementVector = _playerChar.forward * _input.Forward + _playerChar.right * _input.Right;
            _controller.SimpleMove(movementVector.normalized * Constants.PlayerSpeed * Time.deltaTime);
        }

        private void Rotate()
        {
            _playerChar.Rotate(new Vector3(0, _input.RotationX, 0));
            _playerCamera.Rotate(new Vector3(_input.RotationY, 0, 0));
        }

        private void Interact()
        {
            if (!_input.IsInteract)
                return;

            if (Physics.Raycast(_playerCamera.position, _playerCamera.forward, out RaycastHit hit, 10f))
            {
                switch (hit.transform.tag)
                {
                    case "Door":
                        hit.transform.GetComponent<LevelDoor>().Open();
                        break;
                    default:
                        Debug.Log("Player Interaction Raycast hit object name: " + hit.transform.name);
                        break;
                }
            }
        }
    }
}