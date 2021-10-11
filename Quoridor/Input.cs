using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Quoridor
{
    class Input
    {
        public static Point MousePosition { get; private set; }

        private static Dictionary<MouseButtons, bool> mouseButtonState = new Dictionary<MouseButtons,bool>();
        private static Dictionary<Keys, bool> keyState = new Dictionary<Keys,bool>();
        
        public static bool IsMouseButtonDown(MouseButtons button)
        {
            if (!mouseButtonState.ContainsKey(button)) mouseButtonState.Add(button, false);
            return mouseButtonState[button];
        }

        public static bool IsKeyDown(Keys key)
        {
            if (!keyState.ContainsKey(key)) keyState.Add(key, false);
            return keyState[key];
        }
        
        public static void OnMouseMoved(object sender, MouseEventArgs e)
        {
            MousePosition = e.Location;
        }

        public static void OnMouseDown(object sender, MouseEventArgs e)
        {
            mouseButtonState[e.Button] = true;
        }
        
        public static void OnMouseUp(object sender, MouseEventArgs e)
        {
            mouseButtonState[e.Button] = false;
        }
        
        public static void OnKeyDown(object sender, KeyEventArgs e)
        {
            keyState[e.KeyCode] = true;
        }
        
        public static void OnKeyUp(object sender, KeyEventArgs e)
        {
            keyState[e.KeyCode] = false;
        }
    }
}