using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Abrusle.Editor.SceneSwitcher
{
    internal class QuickScenesConfigPopUp : EditorWindow
    {
        private QuickScenesConfigUi _configUi;

        public static void ShowWindow()
        {
            var w = GetWindow<QuickScenesConfigPopUp>();
            w.titleContent = new GUIContent("Scene Switcher Configuration");
            float height = GetHeight(ConfigData.Instance.sceneAssets.Count);
            w.position = new Rect(100, 100, 300, height);
            w.minSize = new Vector2(300, height);
            w.Init();
            w.ShowPopup();
        }

        private void Init()
        {
            _configUi = new QuickScenesConfigUi(ConfigData.Instance.sceneAssets);
            SetWindowSize(_configUi.DisplayingScenes.Count);

            _configUi.LayoutChanged += () => SetWindowSize(_configUi.DisplayingScenes.Count);
            _configUi.RefreshClick += () => _configUi.UpdateData(ConfigData.Instance.sceneAssets);
            _configUi.CancelClick += Close;
            _configUi.SaveClick += scenes =>
            {
                ConfigData.Instance.sceneAssets = scenes.ToList();
                QuickScenesWindow.Refresh();
                Close();
            };
        }

        private void OnGUI()
        {
            if (_configUi == null) Init();
            _configUi.Draw();
        }

        private void SetWindowSize(int sceneCount)
        {
            float height = GetHeight(sceneCount);
            position = new Rect(position)
            {
                height = height
            };
            minSize = new Vector2(minSize.x, height);
            maxSize = new Vector2(maxSize.x, height);
        }

        private static float GetHeight(int sceneCount)
        {
            return 80 + Mathf.Max(1, sceneCount) * 20 + 35;
        }
    }
}