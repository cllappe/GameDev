﻿// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PixelCrushers.DialogueSystem
{

    /// <summary>
    /// This script provides a facility to update the localized text of 
    /// all localized Text elements. You will typically call it from the
    /// event handler of a language selection button or pop-up menu. It
    /// also localizes Texts at start.
    /// </summary>
    [AddComponentMenu("")] // Use wrapper.
    public class UpdateLocalizedUITexts : MonoBehaviour
    {

        /// <summary>
        /// The PlayerPrefs key to store the player's selected language code.
        /// </summary>
        public string languagePlayerPrefsKey = "Language";

        IEnumerator Start()
        {
            yield return null; // Wait for Text components to start.
            var languageCode = string.Empty;
            if (!string.IsNullOrEmpty(languagePlayerPrefsKey) && PlayerPrefs.HasKey(languagePlayerPrefsKey))
            {
                languageCode = PlayerPrefs.GetString(languagePlayerPrefsKey);
            }
            if (string.IsNullOrEmpty(languageCode))
            {
                languageCode = DialogueManager.displaySettings.localizationSettings.language;
            }
            UpdateTexts(languageCode);
        }

        /// <summary>
        /// Updates the current language and all localized Texts.
        /// </summary>
        /// <param name="languageCode">Language code.</param>
        public void UpdateTexts(string languageCode)
        {
            if (DialogueDebug.logInfo) Debug.Log(DialogueDebug.Prefix + ": Setting language to '" + languageCode + "'.", this);
            DialogueManager.displaySettings.localizationSettings.useSystemLanguage = false;
            DialogueManager.displaySettings.localizationSettings.language = languageCode;
            PixelCrushers.DialogueSystem.Localization.language = languageCode;
            if (!string.IsNullOrEmpty(languagePlayerPrefsKey))
            {
                PlayerPrefs.SetString(languagePlayerPrefsKey, languageCode);
            }
            foreach (var localizeUIText in FindObjectsOfType<LocalizeUIText>())
            {
                localizeUIText.LocalizeText();
            }
        }

#if UNITY_EDITOR
        [MenuItem("Tools/Pixel Crushers/Dialogue System/Tools/Clear Saved Localization Settings", false, 1)]
        public static void ClearSavedLocalizationSettings()
        {
            PlayerPrefs.DeleteKey("Language");
        }
#endif

    }

}
