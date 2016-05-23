using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CDKspUtil.Logic;
using System.Threading;

namespace SimpleJetPilot.Testing
{
    class Program
    {
        private static SpeedSimulator _tester;
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            var keyListener = new Thread(KeyListener);

            keyListener.Start();

            _tester = new SpeedSimulator();

            _tester.Start();
        }
        static void KeyListener()
        {
            while (true)
            {
                if (!Console.KeyAvailable) { Thread.Sleep(1); continue; }

                var key = Console.ReadKey(true).Key;

                _tester.OnKeyPress(key);
            }
        }
    }
}
