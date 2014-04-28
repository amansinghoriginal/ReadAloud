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
using System.Runtime.InteropServices;

namespace Read_Aloud
{
    static class Win32Wrapper
    {
        public const int WM_COPY = 0x31;
        public const int HWND_BROADCAST = 0xFFFF;
        public const int KEYEVENTF_KEYUP = 2;
        public const byte VK_CONTROL = 0x11;
        public const byte VK_C = 0x43;

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall,
            SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int Msg,
            IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall,
            SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall,
            SetLastError = true, CharSet = CharSet.Auto)]
        public static extern void keybd_event(Byte bVk, Byte bScan,
            UInt32 dwFlags, IntPtr dwExtraInfo);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall,
            SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall,
            SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr Remove, IntPtr NewNext);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall,
            SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall,
            SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall,
            SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SetFocus(IntPtr hWnd);
    }
}
