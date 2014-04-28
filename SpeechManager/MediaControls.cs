// Copyright (c) 2014 Amandeep Singh

//  The MIT License (MIT)
//  Permission is hereby granted, free of charge, to any person obtaining a
//  copy of this software and associated documentation files (the "Software"),
//  to deal in the Software without restriction, including without limitation
//  the rights to use, copy, modify, merge, publish, distribute, sublicense,
//  and/or sell copies of the Software, and to permit persons to whom the
//  Software is furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included
//  in all copies or substantial portions of the Software.

//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//  OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//  DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SpeechManager
{
    partial class MediaControls : Form
    {
        public MediaControls()
        {
            InitializeComponent();
        }

        public void TogglePlayPause(bool play)
        {
            PauseButton.Visible = !play;
            PlayButton.Visible = play;
        }

        #region INTERNALS
        protected override CreateParams CreateParams
        {
            get
            {
                // Turn on WS_EX_TOOLWINDOW style bit
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        private void MediaControls_Load(object sender, EventArgs e)
        {
            this.Location = new Point(
                Screen.PrimaryScreen.WorkingArea.Width - this.Width,
                Screen.PrimaryScreen.WorkingArea.Height - this.Height);
        }


        private IntPtr handle = IntPtr.Zero;
        private void toolStrip_MouseEnter(object sender, EventArgs e)
        {
            handle = GetForegroundWindow();
            this.Opacity = 0.8;
            this.Focus();
        }

        private void toolStrip_MouseLeave(object sender, EventArgs e)
        {
            if (MousePosition.X < this.Left || MousePosition.Y < this.Top)
            {
                this.Opacity = 0.1;
                SetForegroundWindow(handle);
            }
        }

        private void toolStrip_Click(object sender, EventArgs e)
        {
            SetForegroundWindow(handle);
        }

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall,
            SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall,
            SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        #endregion
    }
}
