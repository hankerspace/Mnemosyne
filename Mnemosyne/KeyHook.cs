using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Mnemosyne
{
    public static class KeyHook
    {
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        public delegate void KeyEventDelegate(Keys key);

        private static Thread _pollingThread;
        private static volatile Dictionary<Keys, bool> _keysStates = new Dictionary<Keys, bool>();

        public static void Initialize()
        {

            if (_pollingThread != null && _pollingThread.IsAlive)
            {
                return;
            }
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                _keysStates[key] = false;
            }


            _pollingThread = new Thread(PollKeys) { IsBackground = true, Name = "KeyThread" };
            _pollingThread.Start();
        }

        private static void PollKeys()
        {
            while (true)
            {
                Thread.Sleep(10);
                foreach (Keys key in Enum.GetValues(typeof(Keys)))
                {
                    if (((GetAsyncKeyState(key) & (1 << 15)) != 0))
                    {
                        if (_keysStates[key]) continue;
                        OnKeyDown?.Invoke(key);
                        _keysStates[key] = true;
                    }
                    else
                    {
                        if (!_keysStates[key]) continue;
                        OnKeyUp?.Invoke(key);
                        _keysStates[key] = false;
                    }
                }


            }
        }

        public static event KeyEventDelegate OnKeyDown;
        public static event KeyEventDelegate OnKeyUp;
    }
}
