using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Abrusle.Editor.SceneSwitcher
{
    internal static class SceneFetcher
    {
        private const string EditorPrefKey = "SceneSwitcherScenes";
        
        public static string[] ScenePaths
        {
            get
            {
                if (!EditorPrefs.HasKey(EditorPrefKey))
                    ScenePaths = new string[0];
                var json = EditorPrefs.GetString(EditorPrefKey);
                SaveData data;
                try
                {
                    data = JsonUtility.FromJson<SaveData>(json);
                }
                catch (System.ArgumentException)
                {
                    data = new SaveData();
                    ScenePaths = new string[0];
                }
                Debug.Log("Get: " + json);
                return data.scenePaths ?? new string[0];
            }
            private set
            {
                var json = EditorJsonUtility.ToJson(new SaveData{scenePaths = value});
                EditorPrefs.SetString(EditorPrefKey, json);
                Debug.Log("Set: " + json);
            }
        }
        
        public static void SaveScenes(IEnumerable<SceneAsset> sceneAssets)
        {
            ScenePaths = sceneAssets
                .Distinct()
                .Where(s => s != null)
                .Select(AssetDatabase.GetAssetPath)
                .ToArray();
        }

        [MenuItem("Tools/Scene Switcher/Clear EditorPrefs Data")]
        private static void ClearSavedScenes()
        {
            if (EditorUtility.DisplayDialog("Delete Scene Switcher preferences.",
                "Are you sure you want to delete all Scene Switcher editor preferences? " +
                "This action cannot be undone.", "Yes", "No"))
            {
                EditorPrefs.DeleteKey(EditorPrefKey);
            }
        }

        [MenuItem("CONTEXT/SceneAsset/Add to Scene Switcher")]
        [MenuItem("Assets/Add to Scene Switcher", false, 111)]
        private static void AddSceneFromEditorUi(MenuCommand cmd)
        {
            if (!(Selection.activeObject is SceneAsset sceneAsset)) return;
            ScenePaths = ScenePaths.Append(AssetDatabase.GetAssetPath(sceneAsset)).ToArray();
            SceneSwitchWindow.Refresh();
        }

        [System.Serializable]
        private struct SaveData
        {
            public string[] scenePaths;
        }
    }
}