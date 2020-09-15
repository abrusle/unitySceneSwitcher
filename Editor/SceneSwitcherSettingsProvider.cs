using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Abrusle.Editor.SceneSwitcher
{
    public class SceneSwitcherSettingsProvider : SettingsProvider
    {
        private readonly QuickScenesConfigUi _configUi;
        
        /// <inheritdoc />
        private SceneSwitcherSettingsProvider(string path, SettingsScope scopes = SettingsScope.User, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
            _configUi = new QuickScenesConfigUi(ConfigData.Instance.sceneAssets);
            _configUi.RefreshClick += () => _configUi.UpdateData(ConfigData.Instance.sceneAssets);
            _configUi.SaveClick += scenes =>
            {
                ConfigData.Instance.sceneAssets = scenes.ToList();
                QuickScenesWindow.Refresh();
            };
        }

        /// <inheritdoc />
        public override void OnGUI(string searchContext)
        {
            base.OnGUI(searchContext);
            _configUi.Draw();
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettings()
        {
            return new SceneSwitcherSettingsProvider("Package Preferences/Scene Switcher")
            {
                keywords = GetSearchKeywordsFromSerializedObject(new SerializedObject(ConfigData.Instance))
            };
        }
    }
}