using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlexisBrusle.Editor.SceneSwitcher
{
    public class SceneSwitchWindow : EditorWindow
    {
        private static string[] _cachedScenePaths;
        private static string _activeScenePath;

        private static SceneSwitchWindow Window => GetWindow<SceneSwitchWindow>();
        
        public static void Refresh()
        {
            Init();
            Window?.Repaint();
        }

        private static void Init()
        {
            _cachedScenePaths = SceneFetcher.ScenePaths;
            _activeScenePath = EditorSceneManager.GetActiveScene().path;
        }

        private void OnEnable()
        {
            Init();
            EditorSceneManager.sceneOpened += OnSceneOpened;
            SceneSwitchConfigWindow.ConfigurationSaved += Refresh;
        }

        private void OnDisable()
        {
            EditorSceneManager.sceneOpened -= OnSceneOpened;
            SceneSwitchConfigWindow.ConfigurationSaved -= Refresh;
        }

        private void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            _activeScenePath = scene.path;
        }

        [MenuItem("Window/Scene Switcher")]
        private static void ShowWindow()
        {
            var window = Window;
            window.titleContent = new GUIContent("Scene Switcher");
            window.autoRepaintOnSceneChange = true;
            window.minSize = new Vector2(window.minSize.x, 45);
            window.Show();
        }

        private void OnGUI()
        {
            var e = Event.current;
            if (e.button == 1 && e.isMouse)
            {
                e.Use();
                DrawGenericMenu();
            }
            
            using (new EditorGUI.DisabledScope(Application.isPlaying))
            {
                if (_cachedScenePaths.Length == 0) 
                    DrawEmptySceneSelector();
                else 
                    DrawSceneSelector();
            }
        }

        private void DrawGenericMenu()
        {
            var menu = new GenericMenu();
            menu.AddDisabledItem(new GUIContent("General Options"));
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Refresh"), false, OnEnable);
            menu.AddItem(new GUIContent("Add & Remove Scenes"), false, SceneSwitchConfigWindow.ShowWindow);
            menu.ShowAsContext();
        }

        private void DrawEmptySceneSelector()
        {
            EditorGUILayout.HelpBox("No scene configured. Right-click here and go to “Add & Remove Scenes” begin.", MessageType.Info);
        }

        private void DrawSceneSelector()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                for (var i = 0; i < _cachedScenePaths.Length; i++)
                {
                    string scenePath = _cachedScenePaths[i];
                    DrawSceneButton(scenePath, i);
                }
            }
        }

        private void DrawSceneButton(string scenePath, int i)
        {
            string sceneName = SceneNameFromPath(scenePath);
            if (GUILayout.Button(sceneName, GetButtonStyle(scenePath, i), GUILayout.Height(20)))
            {
                OnSceneButtonClick(scenePath);
            }
        }

        private string SceneNameFromPath(string path)
        {
            var parts = path.Split('/', '.');
            return parts[parts.Length - 2];
        }

        private void OnSceneButtonClick(string scenePath)
        {
            if (_activeScenePath == scenePath) return;
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(scenePath);
            _activeScenePath = scenePath;
        }

        private GUIStyle GetButtonStyle(string scenePath, int buttonIndex)
        {
            string baseStyle = buttonIndex == _cachedScenePaths.Length - 1
                ? "ButtonRight"
                : buttonIndex == 0
                    ? "ButtonLeft"
                    : "ButtonMid";
            var style = new GUIStyle(baseStyle);
            if (_activeScenePath == scenePath)
                style.normal.background = style.active.background;
            return style;
        }
    }
}