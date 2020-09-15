using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Abrusle.Editor.SceneSwitcher
{
    public static class SceneSwitcher
    {
        public static void AddScene(SceneAsset sceneAsset)
        {
            if (sceneAsset == null) return;
            var config = ConfigData.Instance;
            if (!config.sceneAssets.Contains(sceneAsset))
                config.sceneAssets.Add(sceneAsset);
        }

        public static void AddScenes(IEnumerable<SceneAsset> sceneAssets)
        {
            var config = ConfigData.Instance;
            config.sceneAssets = config.sceneAssets
                .Concat(sceneAssets)
                .Distinct()
                .Where(s => s != null)
                .ToList();
        }

        public static void RemoveScene(SceneAsset sceneAsset)
        {
            ConfigData.Instance.sceneAssets.Remove(sceneAsset);
        }
        
        [MenuItem("CONTEXT/SceneAsset/Add to Quick Scenes")]
        [MenuItem("Assets/Add to Quick Scenes", false, 111)]
        private static void AddSceneFromEditorUi(MenuCommand cmd)
        {
            if (Selection.assetGUIDs.Length == 1)
            {
                AddScene(Selection.activeObject as SceneAsset);
            }
            else if (Selection.assetGUIDs.Length > 0)
            {
                AddScenes(Selection.assetGUIDs.Select(LoadSceneAssetFromGuid));
            }
        }

        private static SceneAsset LoadSceneAssetFromGuid(string guid)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
        }

    }
}