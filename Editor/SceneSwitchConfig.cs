using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Abrusle.Editor.SceneSwitcher
{
    [CreateAssetMenu(fileName = "new Scene Switch Configuration", menuName = "Scene Switcher/Configuration file")]
    internal class SceneSwitchConfig : ScriptableObject
    {
        private const string Guid = "8bbb0eb201d484f2787a3b382fa06a65";

        private static SceneSwitchConfig _instance;

        internal static SceneSwitchConfig Instance
        {
            get
            {
                if (_instance == null)
                    _instance = AssetDatabase.LoadAssetAtPath<SceneSwitchConfig>(AssetDatabase.GUIDToAssetPath(Guid));
                return _instance;
            }
        }
        
        public List<SceneAsset> sceneAssets;
    }
}