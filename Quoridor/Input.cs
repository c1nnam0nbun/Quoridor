using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Quoridor
{
    class Input
    {
        public static Point MousePosition { get; private set; }

        private static Dictionary<MouseButtons, bool> MouseButtonState { get; } = new Dictionary<MouseButtons, bool>();
        private static Dictionary<MouseButtons, bool> PrevMouseButtonState { get; } = new Dictionary<MouseButtons, bool>();
        private static Dictionary<Keys, bool> KeyState { get; } = new Dictionary<Keys, bool>();

        public static void Flush()
        {
            foreach (MouseButtons key in MouseButtonState.Keys.ToList())
            {
                PrevMouseButtonState[key] = MouseButtonState[key];
                MouseButtonState[key] = false;
            }
        }
        
        public static bool IsMouseButtonDown(MouseButtons button)
        {
            if (!MouseButtonState.ContainsKey(button)) MouseButtonState.Add(button, false);
            if (!PrevMouseButtonState.ContainsKey(button)) PrevMouseButtonState.Add(button, false);
            return MouseButtonState[button];
        }

        public static bool IsMouseButtonUp(MouseButtons button)
        {
            if (!MouseButtonState.ContainsKey(button)) MouseButtonState.Add(button, false);
            if (!PrevMouseButtonState.ContainsKey(button)) PrevMouseButtonState.Add(button, false);
            bool res = !MouseButtonState[button] && PrevMouseButtonState[button];
            return res;
        }

        public static bool IsKeyDown(Keys key)
        {
            if (!KeyState.ContainsKey(key)) KeyState.Add(key, false);
            return KeyState[key];
        }
        
        public static void OnMouseMoved(object sender, MouseEventArgs e)
        {
            MousePosition = e.Location;
        }

        public static void OnMouseDown(object sender, MouseEventArgs e)
        {
            PrevMouseButtonState[e.Button] = MouseButtonState[e.Button];
            MouseButtonState[e.Button] = true;
        }
        
        public static void OnMouseUp(object sender, MouseEventArgs e)
        {
            PrevMouseButtonState[e.Button] = MouseButtonState[e.Button];
            MouseButtonState[e.Button] = false;
        }
        
        public static void OnKeyDown(object sender, KeyEventArgs e)
        {
            KeyState[e.KeyCode] = true;
        }
        
        public static void OnKeyUp(object sender, KeyEventArgs e)
        {
            KeyState[e.KeyCode] = false;
        }
    }
}