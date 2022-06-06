using UnityEditor;
using UnityEngine;

namespace Control.Tests
{
    public class TestEditorWindow : EditorWindow
    {
        public static TestEditorWindow Create()
        {
            TestEditorWindow window = EditorWindow.GetWindowWithRect<TestEditorWindow>(new Rect(0, 0, 1920, 1080), true);
            window.Show();
            return window;
        }
    }
}