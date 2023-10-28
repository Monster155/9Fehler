using System.Collections.Generic;
using UnityEngine;

namespace MimoProhodili.Utils
{
    public class LocalizationService
    {
        public static LanguageType CurrentLanguage
        {
            get
            {
                return (LanguageType)PlayerPrefs.GetInt("CurrentLanguage", (int)LanguageType.English);
            }
            private set
            {
                PlayerPrefs.SetInt("CurrentLanguage", (int)value);
            }
        }

        public static Dictionary<string, Dictionary<LanguageType, string>> Texts = TextArray();

        public static void ChangeLanguage(LanguageType language) => CurrentLanguage = language;

        public static string Get(string key, params object[] args) => string.Format(Get(key), args);

        public static string Get(string key)
        {
            var languageTexts = Texts[key];
            if (languageTexts == null)
            {
                Debug.LogError($"Can't find localization for {key} key");
                return key;
            }

            if (languageTexts.ContainsKey(LanguageType.All))
            {
                return languageTexts[LanguageType.All];
            }

            var word = languageTexts[CurrentLanguage];
            if (word == null)
            {
                Debug.LogError($"Can't find localization for {CurrentLanguage} language");
                return key;
            }

            return word;
        }

        private static Dictionary<string, Dictionary<LanguageType, string>> TextArray()
        {
            return new Dictionary<string, Dictionary<LanguageType, string>>()
            {
                {
                    "loading", new Dictionary<LanguageType, string>()
                    {
                        { LanguageType.Russian, "Загрузка" },
                        { LanguageType.English, "Loading" },
                    }
                },
                {
                    "loading_percent", new Dictionary<LanguageType, string>()
                    {
                        { LanguageType.All, "{0} %" },
                    }
                }
            };
        }
    }

    public enum LanguageType
    {
        All, English, Russian,
    }
}