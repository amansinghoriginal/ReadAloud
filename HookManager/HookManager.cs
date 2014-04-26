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

namespace HookManager
{
    public class HookManager
    {

        public delegate void KeyEventHandler(object sender, KeyArgs e);

        /// <summary>
        /// Keyboard delegate
        /// </summary>
        private WinAPI.LowLevelKeyboardProc keyboardDelegate = null;

        /// <summary>
        /// Handle to the low level keyboard hook
        /// </summary>
        private IntPtr hhook = IntPtr.Zero;

        public event KeyEventHandler KeyDown = delegate { };

        /// <summary>
        /// Sets up the global low level keyboard hook
        /// </summary>
        public HookManager()
        {
            keyboardDelegate = keyboardHookProcedure;

            hhook = WinAPI.SetWindowsHookEx(
                WinAPI.WH_KEYBOARD_LL,
                keyboardDelegate,
                IntPtr.Zero,
                0);

            if (hhook == IntPtr.Zero)
            {
                int errorCode =
                    System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                throw new System.ComponentModel.Win32Exception(errorCode);
            }
        }

        /// <summary>
        /// Removes the global low level keyboard hook
        /// </summary>
        ~HookManager()
        {
            if (hhook != IntPtr.Zero)
            {
                keyboardDelegate = null;
                if (WinAPI.UnhookWindowsHookEx(hhook))
                {
                    int errorCode =
                        System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                    throw new System.ComponentModel.Win32Exception(errorCode);
                }
            }
        }

        private int keyboardHookProcedure(int nCode, IntPtr wParam,
            ref WinAPI.KeyboardLowLevelHookStruct lParam)
        {
            if (nCode >= 0)
            {
                if (wParam == (IntPtr)WinAPI.WM_KEYDOWN)
                {
                    Keys keyData = (Keys)lParam.VirtualKeyCode;
                    KeyArgs eventArgs = new KeyArgs(keyData,
                        (WinAPI.GetKeyState(WinAPI.VK_SHIFT) < 0),
                        (WinAPI.GetKeyState(WinAPI.VK_CONTROL) < 0),
                        (WinAPI.GetKeyState(WinAPI.VK_ALT) < 0));
                    KeyDown(this, eventArgs);

                    if (eventArgs.Handled)
                    {
                        return -1;
                    }
                }
            }
            return WinAPI.CallNextHookEx(hhook, nCode, wParam, ref lParam);
        }

    }
}
