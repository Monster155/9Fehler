using MimoProhodili.Utils;
using NineFehler.Game.Bathhouse;
using NineFehler.Utils;
using UnityEngine;

namespace NineFehler.Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputService _input;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _playerChar;
        [SerializeField] private Transform _playerCamera;

        private bool _isPlacementSet;
        public Vector3 Position => _playerChar.position;
        public Quaternion Rotation => _playerChar.rotation;

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
            _rigidbody.velocity = Vector3.zero;
            _playerChar.SetPositionAndRotation(pos, rot);
        }

        private void Move()
        {
            Vector3 movementVector = _playerChar.forward * _input.Forward + _playerChar.right * _input.Right;
            _rigidbody.velocity = movementVector.normalized * Constants.PlayerSpeed * Time.deltaTime;
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
                    case "PlayerInteractable":
                        hit.transform.GetComponent<IPlayerInteractable>().Interact();
                        break;
                    default:
                        Debug.Log("Player Interaction Raycast hit object name: " + hit.transform.name);
                        break;
                }
            }
        }
    }
}