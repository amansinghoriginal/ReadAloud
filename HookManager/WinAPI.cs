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

namespace HookManager
{
    static class WinAPI
    {
        public delegate int LowLevelKeyboardProc(
            int code, IntPtr wParam, ref KeyboardLowLevelHookStruct lParam);

        /// <summary>
        /// KeyboardLowLevelHookStruct contains info about a
        /// low-level keyboard event.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct KeyboardLowLevelHookStruct
        {
            /// <summary>
            /// The virtual key code 
            /// </summary>
            public uint VirtualKeyCode;

            /// <summary>
            /// Hardware scan code for the key. 
            /// </summary>
            public uint ScanCode;

            /// <summary>
            /// Specifies the context code, the event-injected flag,
            /// the transition-state and the extended-key flag.
            /// </summary>
            public uint Flags;

            /// <summary>
            /// Time of the message.
            /// </summary>
            public uint Time;

            /// <summary>
            /// Additional info sent with the message. 
            /// </summary>
            public uint ExtraInfo;
        }

        public const int WH_KEYBOARD_LL = 13;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int WM_SYSKEYDOWN = 0x104;
        public const int WM_SYSKEYUP = 0x105;
        public const int WM_CHAR = 0x61;

        public const int VK_SHIFT = 0x10;
        public const int VK_CONTROL = 0x11;
        public const int VK_ALT = 0x12;


        /// <summary>
        /// Installs an application-defined hook procedure into a hook chain
        ///     monitors the system for certain types of events.
        /// </summary>
        /// <param name="hookID">The type of hook procedure to be installed.</param>
        /// <param name="hookProc">A pointer to the hook procedure.</param>
        /// <param name="hMod">A handle to the DLL containing the hook procedure
        ///     pointed to by the lpfn parameter.</param>
        /// <param name="dwThreadId">The identifier of the thread with which the
        ///     hook procedure is to be associated.</param>
        /// <returns>If the function succeeds, the return value is the handle to
        ///     the hook procedure. If the function fails, the return value is NULL.</returns>
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall,
            SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowsHookEx(int hookID,
            LowLevelKeyboardProc hookProc, IntPtr hMod, uint dwThreadId);

        /// <summary>
        /// Removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
        /// </summary>
        /// <param name="hInstance">A handle to the hook to be removed. This parameter
        /// is a hook handle obtained by a previous call to SetWindowsHookEx. </param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall,
            SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        /// <summary>
        /// Passes the hook information to the next hook procedure in the current
        /// hook chain. A hook procedure can call this function either before or
        /// after processing the hook information. 
        /// </summary>
        /// <param name="idHook">This parameter is ignored. </param>
        /// <param name="nCode">The hook code passed to the current hook procedure.</param>
        /// <param name="wParam">The identifier of the keyboard message. This parameter can
        /// be one of the following messages:
        /// WM_KEYDOWN, WM_KEYUP, WM_SYSKEYDOWN, or WM_SYSKEYUP. </param>
        /// <param name="lParam">A pointer to a KBDLLHOOKSTRUCT structure. </param>
        /// <returns></returns>
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall,
            SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int CallNextHookEx(IntPtr idHook, int nCode,
            IntPtr wParam, ref KeyboardLowLevelHookStruct lParam);

        /// <summary>
        /// The GetKeyState function retrieves the status of the specified virtual key.
        /// The status specifies whether the key is up, down, or toggled 
        /// (on, off—alternating each time the key is pressed). 
        /// </summary>
        /// <param name="vKey">[in] Specifies a virtual key.</param>
        /// <returns>
        /// The return value specifies the status of the specified virtual key, as follows: 
        ///If the high-order bit is 1, the key is down; otherwise, it is up.
        ///If the low-order bit is 1, the key is toggled. A key, such as the CAPS LOCK key,
        ///is toggled if it is turned on. The key is off and untoggled if the low-order bit
        ///is 0. A toggle key's indicator light (if any) on the keyboard will be on when the
        ///key is toggled, and off when the key is untoggled.
        /// </returns>
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall,
            SetLastError = true, CharSet = CharSet.Auto)]
        public static extern short GetKeyState(int vKey);
    }
}
