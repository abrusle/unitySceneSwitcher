using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Abrusle.Editor.SceneSwitcher
{
    public class SceneSwitchWindow : EditorWindow
    {
        private static string[] _cachedScenePaths = new string[0];
        private static string _activeScenePath;
        private Texture2D normalBackground;

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
            normalBackground = Texture2D.redTexture;
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
            ListenForUserInput();

            using (new EditorGUI.DisabledScope(Application.isPlaying))
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(LayoutSettings.Margins.left);
                using (new EditorGUILayout.VerticalScope())
                {
                    GUILayout.Space(LayoutSettings.Margins.top);
                
                    if (_cachedScenePaths.Length == 0)
                        DrawEmptySceneSelector();
                    else
                        DrawSceneSelector();
                
                    GUILayout.Space(LayoutSettings.Margins.bottom);
                }
                GUILayout.Space(LayoutSettings.Margins.right);
            }

            float height = LayoutSettings.Margins.vertical + (_cachedScenePaths.Length == 0
                ? LayoutSettings.EmptyWindowHeight
                : LayoutSettings.Height);
            
            minSize = new Vector2(minSize.x, height);
            
            maxSize = new Vector2(maxSize.x, height);
            
            
        }

        private void ListenForUserInput()
        {
            var e = Event.current;
            if (e.button == 1 && e.isMouse)
            {
                e.Use();
                DrawGenericMenu();
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
            EditorGUILayout.HelpBox("No scene configured. Right-click here and go to “Add & Remove Scenes” begin.",
                MessageType.Info);
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
            if (GUILayout.Button(sceneName, GetButtonStyle(scenePath, i), GUILayout.Height(LayoutSettings.Height)))
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
            string baseStyle =
                _cachedScenePaths.Length == 1
                    ? "Button"
                    : buttonIndex == _cachedScenePaths.Length - 1
                        ? "ButtonRight"
                        : buttonIndex == 0
                            ? "ButtonLeft"
                            : "ButtonMid";
            var style = new GUIStyle(baseStyle);
            if (_activeScenePath == scenePath)
            {
#if UNITY_2019_3_OR_NEWER
                GUI.backgroundColor = new Color(0.52f, 0.52f, 0.52f);
                style.normal.textColor = style.hover.textColor = Color.white;
            }
            else
            {
                GUI.backgroundColor = Color.white;
            }
#else
                style.normal.background = style.active.background;
            }
#endif
            return style;
        }

        private struct LayoutSettings
        {
            public const float Height = 20;
            public static readonly RectOffset Margins = new RectOffset(5, 5, 5, 5);
            public const float EmptyWindowHeight = 25;
        }
    }
}