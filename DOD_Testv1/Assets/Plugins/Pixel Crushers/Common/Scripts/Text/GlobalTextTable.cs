﻿// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;

namespace PixelCrushers
{

    /// <summary>
    /// Maintains a reference to a global TextTable that other scripts can use.
    /// </summary>
    [AddComponentMenu("")] // Use wrapper instead.
    public class GlobalTextTable : MonoBehaviour
    {

        [Tooltip("The global TextTable.")]
        [SerializeField]
        private TextTable m_textTable = null;

        protected static GlobalTextTable s_instance = null;

        protected virtual void Awake()
        {
            s_instance = this;
        }

        protected virtual void OnDestroy()
        {
            s_instance = null;
        }

        /// <summary>
        /// Current instance of GlobalTextTable.
        /// </summary>
        public static GlobalTextTable instance { get { return s_instance; } }

        /// <summary>
        /// Current global text table.
        /// </summary>
        public static TextTable textTable
        {
            get
            {
                return (instance != null) ? instance.m_textTable : null;
            }
            set
            {
                if (instance != null)
                {
                    instance.m_textTable = value;
                    if (UILocalizationManager.instance != null) UILocalizationManager.instance.UpdateUIs(currentLanguage);
                }
            }
        }

        /// <summary>
        /// The current language to use.
        /// </summary>
        public static string currentLanguage
        {
            get { return UILocalizationManager.instance.currentLanguage; }
            set { UILocalizationManager.instance.currentLanguage = value; }
        }

        /// <summary>
        /// Looks up a field value in the global text table.
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <returns>The field value in the global text table for the current language.</returns>
        public static string Lookup(StringField fieldName)
        {
            if (fieldName == null) return string.Empty;
            return Lookup(fieldName.value);
        }

        /// <summary>
        /// Looks up a field value in the global text table.
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <returns>The field value in the global text table for the current language.</returns>
        public static string Lookup(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName)) return string.Empty;
            if (textTable == null) return fieldName;
            return textTable.GetFieldTextForLanguage(fieldName, currentLanguage);
        }

    }
}
