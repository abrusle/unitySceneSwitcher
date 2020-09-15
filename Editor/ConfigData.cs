using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Abrusle.Editor.SceneSwitcher
{
    // [CreateAssetMenu(fileName = "new Scene Switch Configuration", menuName = "Scene Switcher/Configuration file")]
    internal class ConfigData : ScriptableObject
    {
        #region Singleton

        private const string Guid = "8bbb0eb201d484f2787a3b382fa06a65";

        private static ConfigData _instance;

        internal static ConfigData Instance
        {
            get
            {
                if (_instance == null)
                    _instance = AssetDatabase.LoadAssetAtPath<ConfigData>(AssetDatabase.GUIDToAssetPath(Guid));
                return _instance;
            }
        }

        #endregion
        
        public List<SceneAsset> sceneAssets;

        [UnityEditor.CustomEditor(typeof(ConfigData))]
        private class CustomEditor : UnityEditor.Editor
        {
            private QuickScenesConfigUi _configUi;

            private ConfigData Target => (ConfigData) target;

            private void OnEnable()
            {
                _configUi = new QuickScenesConfigUi(Target.sceneAssets);
                _configUi.RefreshClick += OnConfigRefreshClick;
                _configUi.SaveClick += OnConfigSaveClick;
            }

            private void OnConfigSaveClick(SceneAsset[] scenes)
            {
                Target.sceneAssets = scenes.ToList();
                QuickScenesWindow.Refresh();
            }

            private void OnDisable()
            {
                _configUi.RefreshClick -= OnConfigRefreshClick;
            }

            public override void OnInspectorGUI()
            {
                _configUi.Draw();
            }

            private void OnConfigRefreshClick()
            {
                _configUi.UpdateData(Target.sceneAssets);
            }
        }
    }
}