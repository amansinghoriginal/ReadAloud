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

namespace SpeechManager
{
    partial class MediaControls
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MediaControls));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.EnqueueButton = new System.Windows.Forms.ToolStripButton();
            this.PlayButton = new System.Windows.Forms.ToolStripButton();
            this.PauseButton = new System.Windows.Forms.ToolStripButton();
            this.StopButton = new System.Windows.Forms.ToolStripButton();
            this.HomeButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(64, 64);
            this.toolStrip.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EnqueueButton,
            this.PlayButton,
            this.PauseButton,
            this.StopButton,
            this.HomeButton});
            this.toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new System.Windows.Forms.Padding(0);
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(70, 284);
            this.toolStrip.Stretch = true;
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip";
            this.toolStrip.Click += new System.EventHandler(this.toolStrip_Click);
            this.toolStrip.MouseEnter += new System.EventHandler(this.toolStrip_MouseEnter);
            this.toolStrip.MouseLeave += new System.EventHandler(this.toolStrip_MouseLeave);
            // 
            // EnqueueButton
            // 
            this.EnqueueButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.EnqueueButton.Image = ((System.Drawing.Image)(resources.GetObject("EnqueueButton.Image")));
            this.EnqueueButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.EnqueueButton.Name = "EnqueueButton";
            this.EnqueueButton.Size = new System.Drawing.Size(69, 68);
            this.EnqueueButton.Text = "Enqueue Text";
            this.EnqueueButton.MouseEnter += new System.EventHandler(this.toolStrip_MouseEnter);
            this.EnqueueButton.MouseLeave += new System.EventHandler(this.toolStrip_MouseLeave);
            // 
            // PlayButton
            // 
            this.PlayButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PlayButton.Image = ((System.Drawing.Image)(resources.GetObject("PlayButton.Image")));
            this.PlayButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(69, 68);
            this.PlayButton.Text = "Play";
            this.PlayButton.Visible = false;
            this.PlayButton.MouseEnter += new System.EventHandler(this.toolStrip_MouseEnter);
            this.PlayButton.MouseLeave += new System.EventHandler(this.toolStrip_MouseLeave);
            // 
            // PauseButton
            // 
            this.PauseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PauseButton.Image = ((System.Drawing.Image)(resources.GetObject("PauseButton.Image")));
            this.PauseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(69, 68);
            this.PauseButton.Text = "Pause";
            this.PauseButton.MouseEnter += new System.EventHandler(this.toolStrip_MouseEnter);
            this.PauseButton.MouseLeave += new System.EventHandler(this.toolStrip_MouseLeave);
            // 
            // StopButton
            // 
            this.StopButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StopButton.Image = ((System.Drawing.Image)(resources.GetObject("StopButton.Image")));
            this.StopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(69, 68);
            this.StopButton.Text = "Stop";
            this.StopButton.ToolTipText = "Stop";
            this.StopButton.MouseEnter += new System.EventHandler(this.toolStrip_MouseEnter);
            this.StopButton.MouseLeave += new System.EventHandler(this.toolStrip_MouseLeave);
            // 
            // HomeButton
            // 
            this.HomeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.HomeButton.Image = ((System.Drawing.Image)(resources.GetObject("HomeButton.Image")));
            this.HomeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.HomeButton.Name = "HomeButton";
            this.HomeButton.Size = new System.Drawing.Size(68, 68);
            this.HomeButton.Text = "Home";
            this.HomeButton.MouseEnter += new System.EventHandler(this.toolStrip_MouseEnter);
            this.HomeButton.MouseLeave += new System.EventHandler(this.toolStrip_MouseLeave);
            // 
            // MediaControls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(70, 284);
            this.ControlBox = false;
            this.Controls.Add(this.toolStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(70, 284);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(70, 284);
            this.Name = "MediaControls";
            this.Opacity = 0.1D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "MediaControls";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MediaControls_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ToolStripButton PlayButton;
        public System.Windows.Forms.ToolStripButton PauseButton;
        private System.Windows.Forms.ToolStrip toolStrip;
        public System.Windows.Forms.ToolStripButton StopButton;
        public System.Windows.Forms.ToolStripButton EnqueueButton;
        public System.Windows.Forms.ToolStripButton HomeButton;


    }
}