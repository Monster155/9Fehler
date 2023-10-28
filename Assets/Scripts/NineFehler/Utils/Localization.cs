using System;
using TMPro;
using UnityEngine;

namespace MimoProhodili.Utils
{
    [RequireComponent(typeof(TMP_Text))]
    public class Localization : MonoBehaviour
    {
        [SerializeField] private string _key;

        private void Start()
        {
            GetComponent<TMP_Text>().text = LocalizationService.Get(_key);
        }
    }
}