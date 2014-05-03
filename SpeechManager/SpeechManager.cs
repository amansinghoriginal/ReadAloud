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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Threading;
using System.Drawing;

namespace SpeechManager
{
    public class SpeechManager : IDisposable
    {
        public enum State
        {
            Playing,
            Paused,
            Stopped
        };
        public State CurrentState;

        public SpeechManager(string speechVoice, int speechRate, int speechVolume)
        {
            this.SpeechVoice = speechVoice;
            this.SpeechRate = speechRate;
            this.SpeechVolume = speechVolume;
            UpdateSynthesizer();

            InitializeComponents();

            lock (dispatcherRunLock)
                dispatcherRun = true;
            new Thread(() => Dispatcher()).Start();
        }

        public SpeechManager(string speechVoice, int speechRate, int speechVolume,
            bool showControls, bool showDisplay)
            : this(speechVoice, speechRate, speechVolume)
        {
            if (showControls)
                ShowMediaControls();
            if (showDisplay)
                ShowTextDisplay();
        }
        
        public void Enqueue(string text)
        {
            MessageEnqueued(this, new MessageEnqueuedArgs(text));
            lock (messageQueue)
            {
                messageQueue.Enqueue(text);
                if (messageQueue.Count == 1)
                    producedEvent.Set();
            }
        }

        public void Play()
        {
            lock(playLock)
            {
                if (!play)
                {
                    CurrentState = State.Playing;
                    play = true;
                    mediaControls.TogglePlayPause(false);
                    if (synthesizer.State == SynthesizerState.Paused)
                        synthesizer.Resume();
                    playEvent.Set();
                }
            }
        }

        public void Pause()
        {
            lock (playLock)
            {
                if (play)
                {
                    CurrentState = State.Paused;
                    play = false;
                    mediaControls.TogglePlayPause(true);
                }
            }
        }

        public void Stop()
        {
            CurrentState = State.Stopped;
            //Clear Message Queue
            lock (messageQueue)
                messageQueue.Clear();

            synthesizer.SpeakAsyncCancelAll();

            //Clear TextDisplay
            textDisplay.SetText("");
        }

        public string SpeechVoice { get; private set; }
        public int SpeechRate { get; private set; }
        public int SpeechVolume { get; private set; }

        public void Home()
        {
            HomeStart(this, EventArgs.Empty);
            Stop();
            //Load settings into Home Dialog
            home.installedVoicesComboBox.Items.Clear();
            VoiceInfo currentVoice = synthesizer.Voice;
            foreach (var voice in synthesizer.GetInstalledVoices())
                home.installedVoicesComboBox.Items.Add(voice.VoiceInfo);
            home.installedVoicesComboBox.DisplayMember = "Description";
            home.installedVoicesComboBox.SelectedItem = currentVoice;
            lock (DisplayControlsLock)
            {
                home.showMediaControls.Checked = MediaControlsVisible;

                home.showTextDisplay.Checked = TextDisplayVisible;
                home.DisplayFont = textDisplay.DisplayFont;
                home.ForegroundColor = textDisplay.ForegroundColor;
                home.BackgroundColor = textDisplay.BackgroundColor;
                switch (textDisplay.TextLocation)
                {
                    case TextDisplay.DisplayLocation.Top:
                        home.LocationTopRadio.Checked = true;
                        break;
                    case TextDisplay.DisplayLocation.Bottom:
                        home.LocationBottomRadio.Checked = true;
                        break;
                    case TextDisplay.DisplayLocation.Center:
                        home.LocationBottomRadio.Checked = true;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }

            home.volumeTrackbar.Value = synthesizer.Volume;
            home.volumeLabel.Text = "Volume : " + synthesizer.Volume;

            home.rateTrackbar.Value = synthesizer.Rate;
            home.rateLabel.Text = "Rate : " + synthesizer.Rate;

            if (home.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SpeechVoice = ((VoiceInfo)home.installedVoicesComboBox.SelectedItem).Name;
                SpeechVolume = synthesizer.Volume = home.volumeTrackbar.Value;
                SpeechRate = synthesizer.Rate = home.rateTrackbar.Value;

                UpdateSynthesizer();

                lock(DisplayControlsLock)
                {

                    textDisplay.DisplayFont = home.DisplayFont;
                    textDisplay.ForegroundColor = home.ForegroundColor;
                    textDisplay.BackgroundColor = home.BackgroundColor;
                    if (home.LocationTopRadio.Checked)
                    {
                        textDisplay.TextLocation = TextDisplay.DisplayLocation.Top;
                    }
                    else if (home.LocationBottomRadio.Checked)
                    {
                        textDisplay.TextLocation = TextDisplay.DisplayLocation.Bottom;
                    }
                    else if(home.LocationCenterRadio.Checked)
                    {
                        textDisplay.TextLocation = TextDisplay.DisplayLocation.Center;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }

                if (home.showTextDisplay.Checked)
                    ShowTextDisplay();
                else
                    HideTextDisplay();
                if (home.showMediaControls.Checked)
                {
                    restoreMediaControls = true;
                    ShowMediaControls();
                }
                else
                {
                    restoreMediaControls = false;
                    HideMediaControls();
                }
            }
            HomeEnd(this, EventArgs.Empty);
        }

        private object DisplayControlsLock = new object();

        public bool TextDisplayVisible { get; private set; }
        public void ShowTextDisplay()
        {
            lock (DisplayControlsLock)
            {
                if (!TextDisplayVisible)
                {
                    TextDisplayVisible = true;
                    textDisplay.Show();
                }
            }
        }

        public void HideTextDisplay()
        {
            lock(DisplayControlsLock)
            {
                if (TextDisplayVisible)
                {
                    TextDisplayVisible = false;
                    textDisplay.Hide();
                }
            }
        }

        public void ConfigureTextDisplay(string location,
            Font font, Color backColor, Color foreColor)
        {
            lock (DisplayControlsLock)
            {
                TextDisplay.DisplayLocation dl = TextDisplay.DisplayLocation.Top;
                if (Enum.TryParse<TextDisplay.DisplayLocation>(location, true, out dl))
                    textDisplay.TextLocation = dl;
                if (font != null)
                    textDisplay.DisplayFont = font;
                if (backColor != null)
                    textDisplay.BackgroundColor = backColor;
                if (foreColor != null)
                    textDisplay.ForegroundColor = foreColor;
            }
        }

        public bool MediaControlsVisible { get; private set; }
        public void ShowMediaControls()
        {
            lock (DisplayControlsLock)
            {
                if (!MediaControlsVisible)
                {
                    mediaControls.EnqueueButton.Click += EnqueueButton_Click;
                    mediaControls.PlayButton.Click += PlayButton_Click;
                    mediaControls.PauseButton.Click += PauseButton_Click;
                    mediaControls.StopButton.Click += StopButton_Click;
                    mediaControls.HomeButton.Click += HomeButton_Click;
                    mediaControls.Show();
                    MediaControlsVisible = true;
                }
            }
        }

        public void HideMediaControls()
        {
            lock (DisplayControlsLock)
            {
                if (MediaControlsVisible)
                {
                    mediaControls.EnqueueButton.Click -= EnqueueButton_Click;
                    mediaControls.PlayButton.Click -= PlayButton_Click;
                    mediaControls.PauseButton.Click -= PauseButton_Click;
                    mediaControls.StopButton.Click -= StopButton_Click;
                    mediaControls.HomeButton.Click -= HomeButton_Click;
                    mediaControls.Hide();
                    MediaControlsVisible = false;
                }
            }
        }

        public void SelectVoice(VoiceInfo info, int rate, int volume)
        {
            Stop();
            synthesizer.SelectVoice(info.Name);
            synthesizer.Rate = rate;
            synthesizer.Volume = volume;
        }

        public event EventHandler<EventArgs> EnqueueButtonClick = delegate { };

        public event EventHandler<MessageEnqueuedArgs> MessageEnqueued = delegate { };

        public event EventHandler<EventArgs> HomeQuitClick = delegate { };
        public event EventHandler<EventArgs> HomeAboutClick = delegate { };
        public event EventHandler<EventArgs> HomeStart = delegate { };
        public event EventHandler<EventArgs> HomeEnd = delegate { };
        
        public void Dispose()
        {
            producedEvent.Set();
            lock (dispatcherRunLock)
                dispatcherRun = false;
        }

        #region INTERNALS

        private void UpdateSynthesizer()
        {
            synthesizer.SelectVoice(SpeechVoice);
            synthesizer.Rate = SpeechRate;
            synthesizer.Volume = SpeechVolume;
        }

        #region HOME_DIALOG
        private Home home = new Home();

        private void InitializeComponents()
        {
            home.AboutClick += home_AboutClick;
            home.QuitClick += home_QuitClick;
            HomeStart += SpeechManager_HomeStart;
            HomeEnd += SpeechManager_HomeEnd;
        }

        private bool restoreMediaControls = false;
        private void SpeechManager_HomeEnd(object sender, EventArgs e)
        {
            if (restoreMediaControls)
            {
                mediaControls.Visible = true;
                restoreMediaControls = false;
            }
            mediaControls.HomeButton.Enabled = true;
            mediaControls.EnqueueButton.Enabled = true;
            mediaControls.PlayButton.Enabled = true;
            mediaControls.PauseButton.Enabled = true;
            mediaControls.StopButton.Enabled = true;
        }

        private void SpeechManager_HomeStart(object sender, EventArgs e)
        {
            if(mediaControls.Visible)
            {
                mediaControls.Visible = false;
                restoreMediaControls = true;
            }
            else
            {
                restoreMediaControls = false;
            }
            mediaControls.HomeButton.Enabled = false;
            mediaControls.EnqueueButton.Enabled = false;
            mediaControls.PlayButton.Enabled = false;
            mediaControls.PauseButton.Enabled = false;
            mediaControls.StopButton.Enabled = false;
        }

        void home_QuitClick(object sender, EventArgs e)
        {
            HomeQuitClick(sender, e);
        }

        void home_AboutClick(object sender, EventArgs e)
        {
            HomeAboutClick(sender, e);
        }
        #endregion

        private SpeechSynthesizer synthesizer = new SpeechSynthesizer();

        private bool play = true;
        private object playLock = new object();
        private AutoResetEvent playEvent = new AutoResetEvent(false);

        private Queue<string> messageQueue = new Queue<string>();
        private AutoResetEvent producedEvent = new AutoResetEvent(false);

        private object dispatcherRunLock = new object();
        private bool dispatcherRun = false;

        private void Dispatcher()
        {
            synthesizer.SpeakCompleted += synthesizer_SpeakCompleted;
            synthesizer.SpeakProgress += synthesizer_SpeakProgress;

            bool run = false;
            string message;
            do
            {
                message = string.Empty;
                lock (messageQueue)
                {
                    if (messageQueue.Count > 0)
                        message = messageQueue.Dequeue();
                }
                if (message == string.Empty)
                {
                    textDisplay.SetOpacity(0);
                    producedEvent.WaitOne();
                    textDisplay.SetOpacity(0.8);
                }
                else
                {
                    Speak(message);
                }

                lock (dispatcherRunLock)
                    run = dispatcherRun;
            } while (run);

            synthesizer.SpeakCompleted -= synthesizer_SpeakCompleted;
            synthesizer.SpeakProgress -= synthesizer_SpeakProgress;
        }

        private AutoResetEvent speakCompletedEvent = new AutoResetEvent(false);
        private void Speak(string message)
        {
            bool continueSpeak = false;
            lock (playLock)
                continueSpeak = play;
            if (continueSpeak)
            {
                textDisplay.SetText(message);
                synthesizer.SpeakAsync(message);
                speakCompletedEvent.WaitOne();
            }
            else
            {
                playEvent.WaitOne();
                Speak(message);
            }
        }

        private void synthesizer_SpeakProgress(object sender, SpeakProgressEventArgs e)
        {
            lock (playLock)
            {
                if (play)
                {
                    textDisplay.SelectText(e.CharacterPosition, e.CharacterCount);
                }
                else
                {
                    synthesizer.Pause();
                }
            }
        }

        private void synthesizer_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            speakCompletedEvent.Set();
        }

        #region GUI_CONTROLS_SUPPORT
        private TextDisplay textDisplay = new TextDisplay();
        private MediaControls mediaControls = new MediaControls();

        void EnqueueButton_Click(object sender, EventArgs e)
        {
            EnqueueButtonClick(sender, e);
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            Play();
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {
            Pause();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            Stop();
        }
        private void HomeButton_Click(object sender, EventArgs e)
        {
            Home();
        }
        #endregion

        #endregion
    }
}
