using System;
using UnityEngine;

namespace NineFehler.Game.Outdoor
{
    public class TriggerListener : MonoBehaviour
    {
        public Action<Collider> OnEnter;
        public Action<Collider> OnExit;

        private void OnTriggerEnter(Collider other) => OnEnter?.Invoke(other);
        private void OnTriggerExit(Collider other) => OnExit?.Invoke(other);
    }
}
