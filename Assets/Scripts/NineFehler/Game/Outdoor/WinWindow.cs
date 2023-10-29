using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace NineFehler.Game.Outdoor
{
    public class WinWindow : MonoBehaviour
    {
        [SerializeField] private Transform _fogLeft;
        [SerializeField] private Transform _fogRight;
        [SerializeField] private Transform _houseImage;

        private void OnEnable()
        {
            StartCoroutine(FogAnimCoroutine());
            StartCoroutine(HouseAnimCoroutine());
        }

        private IEnumerator FogAnimCoroutine()
        {
            _houseImage.localScale = Vector3.one;

            Vector3 startPosLeft = _fogLeft.position;
            Vector3 startPosRight = _fogRight.position;
            Vector3 delta = new Vector3(1500, 0, 0);

            float maxTimer = 2f;
            float timer = maxTimer;
            while (timer > 0)
            {
                timer -= Time.deltaTime;

                _fogLeft.position = Vector3.Lerp(startPosLeft, startPosLeft - delta, 1 - timer / maxTimer);
                _fogRight.position = Vector3.Lerp(startPosRight, startPosRight + delta, 1 - timer / maxTimer);

                yield return null;
            }
        }

        private IEnumerator HouseAnimCoroutine()
        {
            yield return new WaitForSeconds(0.6f);

            Vector3 finalScale = Vector3.one * 1.4f;
            float maxTimer = 2f;
            float timer = maxTimer;
            while (timer > 0)
            {
                timer -= Time.deltaTime;

                _houseImage.localScale = Vector3.Lerp(Vector3.one, finalScale, 1 - timer / maxTimer);

                yield return null;
            }
        }
    }
}