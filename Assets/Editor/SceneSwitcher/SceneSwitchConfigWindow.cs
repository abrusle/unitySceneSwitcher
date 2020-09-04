using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AlexisBrusle.Editor.SceneSwitcher
{
    internal class SceneSwitchConfigWindow : EditorWindow
    {
        public static event Action ConfigurationSaved;
        private static SceneSwitchConfigWindow Window => GetWindow<SceneSwitchConfigWindow>();

        private List<SceneAsset> _sceneAssets = new List<SceneAsset>();

        private void OnEnable()
        {
            _sceneAssets = SceneFetcher.ScenePaths
                .Select(AssetDatabase.LoadAssetAtPath<SceneAsset>)
                .ToList();
            SetWindowSize();
        }

        [MenuItem("Tools/Scene Switcher/Configure Scenes")]
        public static void ShowWindow()
        {
            Window.titleContent = new GUIContent("Scene Switcher Configuration");
            Window.position = new Rect(Screen.width * .5f, Screen.height * .5f, 300, 200);
            Window.ShowPopup();
        }

        private void OnGUI()
        {
            DrawSceneAssetList();
            DrawFooter();
        }

        private void DrawSceneAssetList()
        {
            GUILayout.Space(10);
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Quick Switch Scenes");
                if (GUILayout.Button("Refresh", EditorStyles.miniButton, GUILayout.MaxWidth(60)))
                    OnEnable();
            }
            
            GUILayout.Space(10);
            
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(10);
                using (new EditorGUILayout.VerticalScope())
                {
                    if (_sceneAssets.Count == 0)
                        EditorGUILayout.LabelField("Press “+” to add Scene Assets...");
                    else
                    {
                        for (int i = 0; i < _sceneAssets.Count; i++)
                            DrawSceneAssetField(i);
                    }
                }
                GUILayout.Space(10);
            }
            
            GUILayout.Space(5);
            if (GUILayout.Button("+", EditorStyles.miniButton))
            {
                _sceneAssets.Add(null);
                SetWindowSize();
            }

        }

        private void DrawSceneAssetField(int i)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var sceneAsset = _sceneAssets[i];
                _sceneAssets[i] = EditorGUILayout.ObjectField(
                    GUIContent.none, 
                    sceneAsset,
                    typeof(SceneAsset),
                    false) as SceneAsset;

                if (GUILayout.Button("x", EditorStyles.miniButton, GUILayout.Width(15)))
                {
                    _sceneAssets.RemoveAt(i);
                    SetWindowSize();
                }
            }
        }

        private void DrawFooter()
        {
            GUILayout.Space(15);
            
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(15);
                
                if (GUILayout.Button("Cancel", GUILayout.ExpandWidth(false), GUILayout.MinWidth(50), GUILayout.Height(25)))
                    Window.Close();
                
                GUILayout.FlexibleSpace();
                
                if (GUILayout.Button("Save", GUILayout.ExpandWidth(false), GUILayout.MinWidth(50), GUILayout.Height(25)))
                {
                    SceneFetcher.SaveScenes(_sceneAssets);
                    ConfigurationSaved?.Invoke();
                    Window.Close();
                }

                GUILayout.Space(15);
            }

            GUILayout.Space(15);
        }

        private void SetWindowSize()
        {
            var w = Window;
            float height = GetHeight();
            w.position = new Rect(w.position)
            {
                height = height
            };
            w.minSize = new Vector2(w.minSize.x, height);
            w.maxSize = new Vector2(w.maxSize.x, height);
        }

        private float GetHeight()
        {
            return 80 + Mathf.Max(1, _sceneAssets.Count) * 20 + 35;
        }
    }
}