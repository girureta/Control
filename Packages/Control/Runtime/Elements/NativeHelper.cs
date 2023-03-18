using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Control
{
    public interface INativeHelper
    {
        void Click(Vector2 pos);
    }
    public static class NativeHelper
    {
        public static INativeHelper GetNativeHelper()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
                return new WindowsNativeHelper();

            return null;
        }
    }

    public class WindowsNativeHelper : INativeHelper
    {
        [Flags]
        public enum MouseEventFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
        }

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        readonly IntPtr winHandle;

        public WindowsNativeHelper()
        {
            //We get the of the Unity window.
            //If we call GetActiveWindow at another point it might return another window,
            //since it returns whatever is active
            winHandle = GetActiveWindow();
        }

        public void Click(Vector2 pos)
        {
            //Bring app to focus
            SwitchToThisWindow(winHandle, true);

            POINT currentCursorPos;
            GetCursorPos(out currentCursorPos);

            //Add the click position to the screen position
            //The win32 api coordinates rever to the whole screen
            var winPos = Screen.mainWindowPosition;
            pos = winPos + pos;

            //Hackity hack
            //Screen.mainWindowPosition  refers to the "title bar" so we offset this by hand?
            pos.y += 40;

            //Move cursor to the position
            SetCursorPos((int)pos.x, (int)pos.y);

            //Perform the actual click
            mouse_event((int)MouseEventFlags.LeftDown, (int)pos.x, (int)pos.y, 0, 0);
            mouse_event((int)MouseEventFlags.LeftUp, (int)pos.x, (int)pos.y, 0, 0);

            //Return the cursor to the original position
            //Not mandatory but less intrusive to the user
            SetCursorPos(currentCursorPos.X, currentCursorPos.Y);
        }
    }
}