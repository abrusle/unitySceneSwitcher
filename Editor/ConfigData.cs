using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Abrusle.Editor.SceneSwitcher
{
    [FilePath("UserSettings/QuickScenes.asset", FilePathAttribute.Location.ProjectFolder)]
    internal class ConfigData : ScriptableSingleton<ConfigData>
    {
        public List<SceneAsset> SceneAssets
        {
            get => sceneAssets;
            set
            {
                if (sceneAssets == value) return;
                sceneAssets = value;
                Save(true);
            }
        }

        [SerializeField]
        private List<SceneAsset> sceneAssets = new List<SceneAsset>();

        [UnityEditor.CustomEditor(typeof(ConfigData))]
        private class CustomEditor : UnityEditor.Editor
        {
            private QuickScenesConfigUi _configUi;

            private ConfigData Target => (ConfigData) target;

            private void OnEnable()
            {
                _configUi = new QuickScenesConfigUi(Target.SceneAssets);
                _configUi.RefreshClick += OnConfigRefreshClick;
                _configUi.SaveClick += OnConfigSaveClick;
            }

            private void OnConfigSaveClick(SceneAsset[] scenes)
            {
                Target.SceneAssets = scenes.ToList();
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
                _configUi.UpdateData(Target.SceneAssets);
            }
        }
    }
}