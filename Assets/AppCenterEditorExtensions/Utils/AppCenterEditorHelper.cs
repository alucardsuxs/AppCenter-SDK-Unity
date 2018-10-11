using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

namespace AppCenterEditor
{
    [InitializeOnLoad]
    public static partial class AppCenterEditorHelper
    {
        public static string EDEX_NAME = "AppCenter_EditorExtensions";
        public static string EDEX_ROOT = Application.dataPath + "/AppCenterEditorExtensions/Editor";
        public static string APPCENTER_SETTINGS_TYPENAME = "AppCenterSettings";
        public static string DEFAULT_SDK_LOCATION = "Assets/AppCenter";

        private static GUISkin _uiStyle;
        public static GUISkin uiStyle
        {
            get
            {
                if (_uiStyle != null)
                    return _uiStyle;
                _uiStyle = GetUiStyle();
                return _uiStyle;
            }
        }

        private static GUISkin GetUiStyle()
        {
            var searchRoot = string.IsNullOrEmpty(EDEX_ROOT) ? Application.dataPath : EDEX_ROOT;
            var pfGuiPaths = Directory.GetFiles(searchRoot, "AppCenterStyles.guiskin", SearchOption.AllDirectories);
            foreach (var eachPath in pfGuiPaths)
            {
                var loadPath = eachPath.Substring(eachPath.LastIndexOf("Assets/"));
                return (GUISkin)AssetDatabase.LoadAssetAtPath(loadPath, typeof(GUISkin));
            }
            return null;
        }
    }
}