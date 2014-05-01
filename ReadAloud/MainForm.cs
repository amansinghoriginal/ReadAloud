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

            InitializeSpeechManager();

            InitializeKeyboard();
        }
        
        private HookManager.HookManager hookManager = new HookManager.HookManager();
        private SpeechManager.SpeechManager speechManager;
        private ContextMenuStrip ctxmenu = new ContextMenuStrip();
        private ToolStripMenuItem homeTSMenuItem;
        private ToolStripMenuItem enqueueTSMenuItem;
        private ToolStripMenuItem playpauseTSMenuItem;
        private ToolStripMenuItem stopTSMenuItem;
        private ToolStripMenuItem exitTSMenuItem;

        private void InitializeTrayIcon()
        {
            homeTSMenuItem = new ToolStripMenuItem("&Home", null, home_Click);
            enqueueTSMenuItem = new ToolStripMenuItem("&Enqueue", null, enqueue_Click);
            playpauseTSMenuItem = new ToolStripMenuItem("&Play/Pause", null, playpause_Click);
            stopTSMenuItem = new ToolStripMenuItem("&Stop", null, stop_Click);
            exitTSMenuItem = new ToolStripMenuItem("E&xit", null, exit_Click);
            ctxmenu.Items.AddRange(new[] { homeTSMenuItem, enqueueTSMenuItem,
                playpauseTSMenuItem, stopTSMenuItem, exitTSMenuItem });
            notifyIcon.ContextMenuStrip = ctxmenu;
            notifyIcon.DoubleClick += notifyIcon_DoubleClick;
        }

        private void InitializeSpeechManager()
        {
            bool mediaControls = Properties.Settings.Default.MediaControls;
            bool textDisplay = Properties.Settings.Default.TextDisplay;
            string speechVoice = Properties.Settings.Default.SpeechVoice;
            int speechRate = Properties.Settings.Default.SpeechRate;
            int speechVolume = Properties.Settings.Default.SpeechVolume;
            speechManager = new SpeechManager.SpeechManager(speechVoice, speechRate,
                speechVolume,mediaControls, textDisplay);
            speechManager.EnqueueButtonClick += speechManager_EnqueueButtonClick;
            speechManager.HomeAboutClick += speechManager_HomeAboutClick;
            speechManager.HomeQuitClick += speechManager_HomeQuitClick;
            speechManager.HomeStart += speechManager_HomeStart;
            speechManager.HomeEnd += speechManager_HomeEnd;
        }

        private void InitializeKeyboard()
        {
            hookManager.KeyDown += hookManager_KeyDown;
            lock (keysOnLock)
            {
                if (Properties.Settings.Default.KeyboardShortcuts)
                    keysOn = true;
                else keysOn = false;
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            speechManager.Home();
        }

        private void speechManager_HomeEnd(object sender, EventArgs e)
        {
            lock (keysOnLock)
                keysOn = true;
            homeTSMenuItem.Enabled = true;
 	        enqueueTSMenuItem.Enabled = true;
            playpauseTSMenuItem.Enabled = true;
            stopTSMenuItem.Enabled = true;
        }

        private void speechManager_HomeStart(object sender, EventArgs e)
        {
            homeTSMenuItem.Enabled = false;
            enqueueTSMenuItem.Enabled = false;
            playpauseTSMenuItem.Enabled = false;
            stopTSMenuItem.Enabled = false;
            lock (keysOnLock)
                keysOn = false;
        }

        private void home_Click(object sender, EventArgs e)
        {
            speechManager.Home();
        }

        private void enqueue_Click(object sender, EventArgs e)
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
            Exit();
        }

        private void speechManager_EnqueueButtonClick(object sender, EventArgs e)
        {
            ClipboardManager.Instance.GetHighlightedText();
        }

        private AboutDialog aboutDialog = null;
        private void speechManager_HomeAboutClick(object sender, EventArgs e)
        {
            if (aboutDialog == null)
                aboutDialog = new AboutDialog();
            aboutDialog.Show();
        }

        private void speechManager_HomeQuitClick(object sender, EventArgs e)
        {
            Exit();
        }

        private bool keysOn = true;
        private object keysOnLock = new object();
        private void hookManager_KeyDown(object sender, HookManager.KeyArgs e)
        {
            bool keys = false;
            lock (keysOnLock)
                keys = keysOn;
            if (keys)
            {
                if (e.KeyData == Keys.Space && e.Ctrl)
                {
                    Enqueue();
                    e.Handled = true;
                }
                else if (e.KeyData == Keys.P && e.Ctrl && e.Alt)
                {
                    PauseResume();
                    e.Handled = true;
                }
                else if (e.KeyData == Keys.X && e.Ctrl && e.Alt)
                {
                    Stop();
                    e.Handled = true;
                }
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

        private void Exit()
        {
            Stop();
            SaveProperties();
            notifyIcon.Visible = false;
            speechManager.Stop();
            speechManager.Dispose();
            Application.Exit();
        }

        private void SaveProperties()
        {
            Properties.Settings.Default["TextDisplay"] = speechManager.TextDisplayVisible;
            Properties.Settings.Default["MediaControls"] = speechManager.MediaControlsVisible;
            Properties.Settings.Default["SpeechVoice"] = speechManager.SpeechVoice;
            Properties.Settings.Default["SpeechVolume"] = speechManager.SpeechVolume;
            Properties.Settings.Default["SpeechRate"] = speechManager.SpeechRate;
            Properties.Settings.Default.Save();
        }
        #endregion

        public void Enqueue(string message)
        {
            speechManager.Enqueue(message);
        }
    }
}