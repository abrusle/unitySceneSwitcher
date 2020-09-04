using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AlexisBrusle.Editor.SceneSwitcher
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
                return JsonUtility.FromJson<string[]>(json);
            }
            private set
            {
                string json = JsonUtility.ToJson(value);
                EditorPrefs.SetString(EditorPrefKey, json);
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

        [MenuItem("CONTEXT/SceneAsset/Add to Scene Switcher")]
        [MenuItem("Assets/Add to Scene Switcher", false, 111)]
        private static void AddSceneFromEditorUi(MenuCommand cmd)
        {
            if (!(Selection.activeObject is SceneAsset sceneAsset)) return;
            ScenePaths = ScenePaths.Append(AssetDatabase.GetAssetPath(sceneAsset)).ToArray();
            SceneSwitchWindow.Refresh();
        }
    }
}