using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Abrusle.Editor.SceneSwitcher
{
    internal class SceneSwitchConfigWindow : EditorWindow
    {
        public static event Action ConfigurationSaved;

        private List<SceneAsset> _sceneAssets = new List<SceneAsset>();

        private void OnEnable()
        {
            _sceneAssets = SceneSwitchConfig.Instance.sceneAssets;
            SetWindowSize();
        }

        [MenuItem("Tools/Scene Switcher/Configure Scenes")]
        public static void ShowWindow()
        {
            var w = GetWindow<SceneSwitchConfigWindow>();
            w.titleContent = new GUIContent("Scene Switcher Configuration");
            float height = w.GetHeight();
            w.position = new Rect(100, 100, 300, height);
            w.minSize = new Vector2(300, height);
            w.ShowPopup();
        }

        private void OnGUI()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(LayoutSettings.Margin.left);
                using (new EditorGUILayout.VerticalScope())
                {
                    GUILayout.Space(LayoutSettings.Margin.top);
                    
                    DrawSceneAssetList();
                    DrawFooter();
                    
                    GUILayout.Space(LayoutSettings.Margin.bottom);
                }
                GUILayout.Space(LayoutSettings.Margin.right);
            }
        }

        private void DrawSceneAssetList()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Quick Switch Scenes");
                if (GUILayout.Button("Refresh", EditorStyles.miniButton, GUILayout.MaxWidth(60)))
                    OnEnable();
            }
            
            GUILayout.Space(10);
            
            if (_sceneAssets.Count == 0)
                EditorGUILayout.LabelField("Press “+” to add Scene Assets...");
            else
            {
                for (int i = 0; i < _sceneAssets.Count; i++)
                    DrawSceneAssetField(i);
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

                if (GUILayout.Button("x", EditorStyles.miniButton, GUILayout.Width(20)))
                {
                    _sceneAssets.RemoveAt(i);
                    SetWindowSize();
                }
            }
        }

        private void DrawFooter()
        {
            GUILayout.Space(LayoutSettings.SectionSpacing);
            
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Cancel", GUILayout.ExpandWidth(false), GUILayout.MinWidth(50), GUILayout.Height(25)))
                    Close();
                
                GUILayout.FlexibleSpace();
                
                if (GUILayout.Button("Save", GUILayout.ExpandWidth(false), GUILayout.MinWidth(50), GUILayout.Height(25)))
                {
                    SceneSwitchConfig.Instance.sceneAssets = _sceneAssets;
                    ConfigurationSaved?.Invoke();
                    Close();
                }
            }
        }

        private void SetWindowSize()
        {
            float height = GetHeight();
            position = new Rect(position)
            {
                height = height
            };
            minSize = new Vector2(minSize.x, height);
            maxSize = new Vector2(maxSize.x, height);
        }

        private float GetHeight()
        {
            return 80 + Mathf.Max(1, _sceneAssets.Count) * 20 + 35;
        }

        private struct LayoutSettings
        {
            public static readonly RectOffset Margin = new RectOffset(10, 10, 10, 10);
            public const float SectionSpacing = 15;
        }
    }
}