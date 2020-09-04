using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;

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
                return JsonConvert.DeserializeObject<string[]>(json);
            }
            private set
            {
                string json = JsonConvert.SerializeObject(value);
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
    }
}