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
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;

namespace Read_Aloud
{
    partial class MainForm : Form
    {
        public static MainForm Instance { get { return instance; } }

        #region INTERNALS
        private static MainForm instance = new MainForm();

        private MainForm()
        {
            InitializeComponent();
            InitializeTrayIcon();

            speechManager = new SpeechManager.SpeechManager(true, true);
            speechManager.EnqueueButtonClick += speechManager_EnqueueButtonClick;
            speechManager.HomeButtonClick += speechManager_HomeButtonClick;

            hookManager.KeyDown += hookManager_KeyDown;
        }
        
        HookManager.HookManager hookManager = new HookManager.HookManager();
        private SpeechManager.SpeechManager speechManager;

        private void InitializeTrayIcon()
        {
            ContextMenuStrip ctxmenu = new ContextMenuStrip();

            ToolStripMenuItem home = new ToolStripMenuItem("&Home", null, home_Click);
            ToolStripMenuItem enqueue = new ToolStripMenuItem("&Enqueue", null, enqueue_click);
            ToolStripMenuItem playpause = new ToolStripMenuItem("&Play/Pause", null, playpause_Click);
            ToolStripMenuItem stop = new ToolStripMenuItem("&Stop", null, stop_Click);
            ToolStripMenuItem exit = new ToolStripMenuItem("E&xit", null, exit_Click);

            ctxmenu.Items.AddRange(new[] { home, enqueue, playpause, stop, exit });
            notifyIcon1.ContextMenuStrip = ctxmenu;
        }

        private void home_Click(object sender, EventArgs e)
        {
            
        }
        private void enqueue_click(object sender, EventArgs e)
        {
            Enqueue();
        }

        private void stop_Click(object sender, EventArgs e)
        {
            Stop();
        }

        private void playpause_Click(object sender, EventArgs e)
        {
            PauseResume();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            speechManager.Stop();
            speechManager.Dispose();
            Application.Exit();
        }

        private void speechManager_EnqueueButtonClick(object sender, EventArgs e)
        {
            ClipboardManager.Instance.GetHighlightedText();
        }

        private void speechManager_HomeButtonClick(object sender, EventArgs e)
        {
            //TODO
        }

        private void hookManager_KeyDown(object sender, HookManager.KeyArgs e)
        {
            if (e.KeyData == Keys.Space && e.Ctrl)
            {
                Enqueue();
                e.Handled = true;
            }
            else if(e.KeyData == Keys.P && e.Ctrl && e.Alt)
            {
                PauseResume();
                e.Handled = true;
            }
            else if(e.KeyData == Keys.X && e.Ctrl && e.Alt)
            {
                Stop();
                e.Handled = true;
            }
        }

        private void PauseResume()
        {
            if (speechManager.CurrentState == SpeechManager.SpeechManager.State.Playing)
                speechManager.Pause();
            else
                speechManager.Play();
        }

        private void Stop()
        {
            speechManager.Stop();
        }

        private void Enqueue()
        {
            ClipboardManager.Instance.GetHighlightedText();
        }
        #endregion

        public void Enqueue(string message)
        {
            speechManager.Enqueue(message);
        }
    }
}