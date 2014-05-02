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
using System.Speech.Synthesis;

namespace SpeechManager
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        public event EventHandler<EventArgs> QuitClick = delegate { };
        public event EventHandler<EventArgs> AboutClick = delegate { };

        public string TestSpeechString = "The quick brown fox jumps over the lazy dog.";

        private SpeechSynthesizer testSynthesizer = new SpeechSynthesizer();
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutClick(sender, e);
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuitClick(sender, e);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void volumeTrackbar_Scroll(object sender, EventArgs e)
        {
            volumeLabel.Text = "Volume : " + volumeTrackbar.Value;
        }

        private void rateTrackbar_Scroll(object sender, EventArgs e)
        {
            rateLabel.Text = "Rate : " + rateTrackbar.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            testSynthesizer.SpeakAsyncCancelAll();
            testSynthesizer.SelectVoice(
                ((VoiceInfo)installedVoicesComboBox.SelectedItem).Name);
            testSynthesizer.Rate = rateTrackbar.Value;
            testSynthesizer.Volume = volumeTrackbar.Value;
            testSynthesizer.SpeakAsync(TestSpeechString);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            testSynthesizer.SpeakAsyncCancelAll();
        }
    }
}
