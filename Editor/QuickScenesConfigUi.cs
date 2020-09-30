using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Abrusle.Editor.SceneSwitcher
{
    internal sealed class QuickScenesConfigUi
    {
        public event Action LayoutChanged;
        public event Action<SceneAsset[]> SaveClick;
        public event Action CancelClick;
        public event Action RefreshClick;

        public IReadOnlyCollection<SceneAsset> DisplayingScenes => _sceneAssets;
        private List<SceneAsset> _sceneAssets;

        public QuickScenesConfigUi(IEnumerable<SceneAsset> sceneAssets)
        {
            UpdateData(sceneAssets);
        }

        public void UpdateData(IEnumerable<SceneAsset> sceneAssets)
        {
            _sceneAssets = sceneAssets.ToList();
        }

        public void Draw()
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
                    RefreshClick?.Invoke();
            }
            
            GUILayout.Space(10);
            
            if (_sceneAssets.Count == 0)
                EditorGUILayout.LabelField("Press “+” to add Scene Assets...");
            else
            {
                for (int i = 0; i < _sceneAssets.Count; i++)
                {
                    if (i != 0) GUILayout.Space(2.5f);
                    DrawSceneAssetLine(i);
                }
            }

            GUILayout.Space(5);
            
            if (GUILayout.Button(new GUIContent("+", "Add a scene asset"), EditorStyles.miniButton))
            {
                _sceneAssets.Add(null);
                LayoutChanged?.Invoke();
            }
        }

        private void DrawSceneAssetLine(int i)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                var moveButtonStyle = new GUIStyle(EditorStyles.miniButton) {fontSize = 8};
                using (new EditorGUI.DisabledScope(_sceneAssets.Count < 2 || i == 0))
                    if (GUILayout.Button(new GUIContent("\u25B2", "Move Up"), moveButtonStyle, GUILayout.Width(20)))
                    {
                        var thisAsset = _sceneAssets[i];
                        _sceneAssets[i] = _sceneAssets[i - 1];
                        _sceneAssets[i - 1] = thisAsset;
                    }
                
                using (new EditorGUI.DisabledScope(_sceneAssets.Count < 2 || i == _sceneAssets.Count - 1))
                    if (GUILayout.Button(new GUIContent("\u25BC", "Move Down"), moveButtonStyle, GUILayout.Width(20)))
                    {
                        var thisAsset = _sceneAssets[i];
                        _sceneAssets[i] = _sceneAssets[i + 1];
                        _sceneAssets[i + 1] = thisAsset;
                    }
                
                var sceneAsset = _sceneAssets[i];
                _sceneAssets[i] = EditorGUILayout.ObjectField(
                    GUIContent.none, 
                    sceneAsset,
                    typeof(SceneAsset),
                    false) as SceneAsset;

                if (GUILayout.Button(new GUIContent("\u00D7", "Remove asset from list"), EditorStyles.miniButton, GUILayout.Width(20)))
                {
                    _sceneAssets.RemoveAt(i);
                    LayoutChanged?.Invoke();
                }
            }
        }

        private void DrawFooter()
        {
            GUILayout.Space(LayoutSettings.SectionSpacing);
            
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button(new GUIContent("Cancel", "Discard changes to the list."), GUILayout.ExpandWidth(false), GUILayout.MinWidth(50), GUILayout.Height(25)))
                    CancelClick?.Invoke();
                
                GUILayout.FlexibleSpace();
                
                if (GUILayout.Button(new GUIContent("Save", "Save current list as quick scenes."), GUILayout.ExpandWidth(false), GUILayout.MinWidth(50), GUILayout.Height(25)))
                {
                    SaveClick?.Invoke(_sceneAssets.ToArray());
                }
            }
        }

        private struct LayoutSettings
        {
            public static readonly RectOffset Margin = new RectOffset(10, 10, 10, 10);
            public const float SectionSpacing = 15;
        }
    }
}