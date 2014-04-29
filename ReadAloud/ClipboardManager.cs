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
using System.Windows.Forms;

namespace Read_Aloud
{
    class ClipboardManager : Control
    {
        #region INTERNALS
        #region SINGLETON_CODE
        private static ClipboardManager instance = new ClipboardManager();
        public static ClipboardManager Instance
        {
            get { return instance; }
        }
        #endregion

        #region CLIPBOARD_CHAIN_MANAGEMENT
        private bool intercept = false;
        private IntPtr nextClipboardViewer;

        private ClipboardManager()
        {
            nextClipboardViewer = Win32Wrapper.SetClipboardViewer(this.Handle);
        }

        ~ClipboardManager()
        {
            Win32Wrapper.ChangeClipboardChain(this.Handle, nextClipboardViewer);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_DRAWCLIPBOARD = 0x308;
            const int WM_CHANGECBCHAIN = 0x030D;

            switch (m.Msg)
            {
                case WM_DRAWCLIPBOARD:
                    if (intercept)
                        newText();
                    Win32Wrapper.SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;

                case WM_CHANGECBCHAIN:
                    if (m.WParam == nextClipboardViewer)
                        nextClipboardViewer = m.LParam;
                    else
                        Win32Wrapper.SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private void newText()
        {
            intercept = false;
            string text = Clipboard.GetText();
            restoreCurrentContent();
            MainForm.Instance.Enqueue(TextManager.Instance.Process(text));
        }
        #endregion

        #region SAVE_RESTORE_CLIPBOARD
        private enum ContentType
        {
            AudioStream,
            FileDropList,
            Image,
            Text,
            DataObject
        }

        private ContentType currentType;
        private object currentContent;

        private void saveCurrentContent()
        {
            if (Clipboard.ContainsAudio())
            {
                currentContent = Clipboard.GetAudioStream();
                currentType = ContentType.AudioStream;
            }
            else if (Clipboard.ContainsFileDropList())
            {
                currentContent = Clipboard.GetFileDropList();
                currentType = ContentType.FileDropList;
            }
            else if (Clipboard.ContainsImage())
            {
                currentContent = Clipboard.GetImage();
                currentType = ContentType.Image;
            }
            else if (Clipboard.ContainsText())
            {
                currentContent = Clipboard.GetText();
                currentType = ContentType.Text;
            }
            else
            {
                currentContent = Clipboard.GetDataObject();
                currentType = ContentType.DataObject;
            }
        }

        private void restoreCurrentContent()
        {
            switch (currentType)
            {
                case ContentType.AudioStream:
                    Clipboard.SetAudio((System.IO.Stream)currentContent);
                    break;

                case ContentType.FileDropList:
                    Clipboard.SetFileDropList(
                        (System.Collections.Specialized.StringCollection)currentContent);
                    break;

                case ContentType.Image:
                    Clipboard.SetImage((System.Drawing.Image)currentContent);
                    break;

                case ContentType.Text:
                    Clipboard.SetText((string)currentContent);
                    break;

                case ContentType.DataObject:
                    Clipboard.SetDataObject(currentContent);
                    break;

                default:
                    throw new InvalidOperationException("Invalid value for enum");
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// Gets the highlighted text from the active window, via the
        /// clipboard without damaging current clipboard contents.
        /// </summary>
        /// <remarks>
        /// Saves current clipboard and then sends Ctrl+C to active window.
        /// Picks text from clipboard, and restores original clipboard content.
        /// </remarks>
        public void GetHighlightedText()
        {
            intercept = true;
            saveCurrentContent();
            IntPtr handle = Win32Wrapper.GetForegroundWindow();
            Win32Wrapper.keybd_event(Win32Wrapper.VK_CONTROL, 0, 0, IntPtr.Zero);
            Win32Wrapper.keybd_event(Win32Wrapper.VK_C, 0, 0, IntPtr.Zero);
            Win32Wrapper.keybd_event(Win32Wrapper.VK_C, 0, Win32Wrapper.KEYEVENTF_KEYUP, IntPtr.Zero);
            Win32Wrapper.keybd_event(Win32Wrapper.VK_CONTROL, 0, Win32Wrapper.KEYEVENTF_KEYUP, IntPtr.Zero);
        }
    }
}
