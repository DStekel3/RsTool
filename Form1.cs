using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Threading;
using Timer = System.Windows.Forms.Timer;

namespace howto_move_click_mouse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        // See http://www.pinvoke.net/default.aspx/user32/mouse_event.html
        // Mouse event flags.
        [Flags]
        public enum MouseEventFlags : uint
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010,
            WHEEL = 0x00000800,
            XDOWN = 0x00000080,
            XUP = 0x00000100
        }

        // Use the values of this enum for the 'dwData' parameter
        // to specify an X button when using MouseEventFlags.XDOWN or
        // MouseEventFlags.XUP for the dwFlags parameter.
        public enum MouseEventDataXButtons : uint
        {
            XBUTTON1 = 0x00000001,
            XBUTTON2 = 0x00000002
        }

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        // The mouse's target location.
        private Point m_Target = new Point(150, 100);

        // Move the mouse and click it.
        private void btnMoveClick_Click(object sender, EventArgs e)
        {
            // Convert the target to absolute screen coordinates.
            Point pt = this.PointToScreen(m_Target);

            // mouse_event moves in a coordinate system where
            // (0, 0) is in the upper left corner and
            // (65535,65535) is in the lower right corner.
            // Convert the coordinates.
            Rectangle screen_bounds = Screen.GetBounds(pt);
            uint x = (uint)(pt.X * 65535 / screen_bounds.Width);
            uint y = (uint)(pt.Y * 65535 / screen_bounds.Height);

            // Move the mouse.
            mouse_event(
                (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MOVE),
                x, y, 0, 0);

            // Click there.
            mouse_event(
                (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MOVE |
                    MouseEventFlags.LEFTDOWN | MouseEventFlags.LEFTUP),
                x, y, 0, 0);
        }

        private int sleeper = 100;

        private void Form1_Click(object sender, EventArgs e)
        {
            //var mineSession = new Stopwatch();
            //while (mineSession.Elapsed.Minutes < 60)
            //{
            //    var t = new Stopwatch();
            //    t.Start();
            //    while(t.Elapsed.Minutes < 4)
            //    {
            //        AutoClickMidScreen();
            //        Thread.Sleep(3000);
            //    }
            //    EmptyBag();
            //}


            // EmptyBag();

        }

        private void EmptyBag(int rowBegin, int colBegin)
        {
            Point startPos = new Point(-450 + 40 * (colBegin - 1), 730 + 40 * (rowBegin - 1));
            // Convert the target to absolute screen coordinates.
            Point pt = this.PointToScreen(startPos);

            for(int col = 0; col <= (3 - (colBegin - 1)); col++)
            {
                pt.X += col * 40;
                for(int row = 0; row < (7 - (rowBegin - 1)); row++)
                {
                    RightMouseClick(pt, true);

                    if(row < (6-(rowBegin - 1)))
                    {
                        pt.Y += 40;
                    }

                    LeftMouseClick(pt, true);
                }
                pt = this.PointToScreen(startPos);
            }
        }

        private void AutoClickMidScreen()
        {
            Point pt = this.PointToScreen(new Point((-1920 / 2) - 20, 1080 / 2));
            Rectangle screen_bounds = Screen.GetBounds(pt);
            uint x = (uint)(pt.X * 65535 / screen_bounds.Width);
            uint y = (uint)(pt.Y * 65535 / screen_bounds.Height);

            // Move the mouse.
            mouse_event(
                (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MOVE),
                x, y, 0, 0);

            mouse_event((uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MOVE |
                               MouseEventFlags.LEFTDOWN | MouseEventFlags.LEFTUP),
                x, y, 0, 0);
        }

        private void RightMouseClick(Point pt, bool click)
        {
            Rectangle screen_bounds = Screen.GetBounds(pt);
            uint x = (uint)(pt.X * 65535 / screen_bounds.Width);
            uint y = (uint)(pt.Y * 65535 / screen_bounds.Height);

            // Move the mouse.
            mouse_event(
                (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MOVE),
                x, y, 0, 0);

            Thread.Sleep(sleeper);

            if(click)
            {
                mouse_event((uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MOVE |
                                    MouseEventFlags.RIGHTDOWN | MouseEventFlags.RIGHTUP),
                    x, y, 0, 0);
            }
        }

        private void LeftMouseClick(Point pt, bool click)
        {
            Rectangle screen_bounds = Screen.GetBounds(pt);
            uint x = (uint)(pt.X * 65535 / screen_bounds.Width);
            uint y = (uint)(pt.Y * 65535 / screen_bounds.Height);

            // Move the mouse.
            mouse_event(
                (uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MOVE),
                x, y, 0, 0);

            Thread.Sleep(sleeper);
            if(click)
            {
                mouse_event((uint)(MouseEventFlags.ABSOLUTE | MouseEventFlags.MOVE |
                                    MouseEventFlags.LEFTDOWN | MouseEventFlags.LEFTUP),
                    x, y, 0, 0);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            EmptyInventory();
        }

        void EmptyInventory()
        {
            int.TryParse(this.RowStart.Text, out int rowBegin);
            int.TryParse(this.ColStart.Text, out int colBegin);
            int.TryParse(this.SpeedBox.Text, out int speed);
            sleeper = speed;
            EmptyBag(rowBegin, colBegin);

            this.WindowState = WindowState = FormWindowState.Minimized;
        }

        private void Form1_KeyPress(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if(e.KeyCode == Keys.Space)
            {
                EmptyInventory();
            }
        }
    }
}
