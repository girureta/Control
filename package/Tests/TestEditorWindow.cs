using UnityEditor;

namespace Control.Tests
{
    public class TestEditorWindow : EditorWindow
    {
        public static TestEditorWindow Create()
        {
            TestEditorWindow window = EditorWindow.GetWindow<TestEditorWindow>(true);
            window.Show();
            return window;
        }
    }
}