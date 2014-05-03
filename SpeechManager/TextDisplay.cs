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

namespace SpeechManager
{
    partial class TextDisplay : Form
    {
        public enum DisplayLocation
        {
            Top,
            Bottom,
            Center
        };

        public TextDisplay()
        {
            InitializeComponent();
            this.textBox.HideSelection = false;
            this.textBox.ReadOnly = true;
        }

        public void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.textBox.Text = text;
            }
        }
        public void SetOpacity(double opacity)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox.InvokeRequired)
            {
                SetOpacityCallback d = new SetOpacityCallback(SetOpacity);
                this.Invoke(d, new object[] { opacity });
            }
            else
            {
                this.Opacity = opacity;
            }
        }

        public void SelectText(int characterPosition, int characterCount)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (textBox.InvokeRequired)
            {
                this.Invoke(new SelectTextCallback(SelectText),
                    new object[] { characterPosition, characterCount });
            }
            else
            {
                textBox.Select(characterPosition, characterCount);
            }
        }

        public Font DisplayFont
        {
            get {
                return this.textBox.Font;
            }
            set {
                this.textBox.Font = value;
            }
        }

        public Color BackgroundColor
        {
            get {
                return this.textBox.BackColor;
            }
            set {
                this.textBox.BackColor = value;
            }
        }

        public Color ForegroundColor
        {
            get {
                return this.textBox.ForeColor;
            }
            set {
                this.textBox.ForeColor = value;
            }
        }

        private DisplayLocation textLocation = DisplayLocation.Top;
        public DisplayLocation TextLocation
        {
            get {
                return this.textLocation;
            }
            set {
                if(this.textLocation != value)
                {
                    this.textLocation = value;
                    UpdateDisplayLocation();
                }
            }
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

        private void TextDisplay_Load(object sender, EventArgs e)
        {
            UpdateDisplayLocation();
        }

        private void UpdateDisplayLocation()
        {
            switch (this.TextLocation)
            {
                case DisplayLocation.Top:
                    SetToTop();
                    break;
                case DisplayLocation.Bottom:
                    SetToBottom();
                    break;
                case DisplayLocation.Center:
                    SetToCenter();
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        private void SetToTop()
        {
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            this.Height = (int) (screenHeight * 0.3);
            this.Width = (int) (screenWidth * 0.8);
            int delta = screenWidth - this.Width;
            this.Location = new Point(delta / 2, 0);
        }

        private void SetToBottom()
        {
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            this.Height = (int)(screenHeight * 0.3);
            this.Width = (int)(screenWidth * 0.8);
            int delta = screenWidth - this.Width;
            this.Location = new Point(delta / 2, screenHeight - this.Height);
        }

        private void SetToCenter()
        {
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            this.Height = (int)(screenHeight * 0.7);
            this.Width = (int)(screenWidth * 0.8);
            int deltaX = screenWidth - this.Width;
            int deltaY = screenHeight - this.Height;
            this.Location = new Point(deltaX/2, deltaY/2);
        }

        delegate void SetTextCallback(string text);
        delegate void SelectTextCallback(int characterPosition, int characterCount);
        delegate void SetOpacityCallback(double opacity);
        #endregion
    }
}
