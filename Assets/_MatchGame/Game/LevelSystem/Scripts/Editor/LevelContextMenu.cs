using Abstractions.LevelSystem;
using UnityEditor;

namespace Game.LevelSystem.Editor
{
    public static class LevelContextMenu
    {
        private const string TestLevelKey = "MatchGame.TestLevelName";

        [MenuItem("Assets/Use as Test Level", priority = -9999)]
        private static void UseAsTestLevel()
        {
            var level = Selection.activeObject as ILevel;
            EditorPrefs.SetString(TestLevelKey, level.name);
            EditorApplication.isPlaying = true;
        }

        [MenuItem("Assets/Use as Test Level", validate = true)]
        private static bool UseAsTestLevelValidate()
        {
            return Selection.activeObject is ILevel;
        }
    }
}