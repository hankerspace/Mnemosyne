using System;
using Mnemosyne;

namespace MnemosyneTest
{
    class Program
    {
        static void Main(string[] args)
        {
            KeyHook.OnKeyDown += key => { Console.WriteLine(key + " down"); };
            KeyHook.OnKeyUp += key => { Console.WriteLine(key + " up"); };
            KeyHook.Initialize();
            Console.WriteLine("Press enter to stop...");
            Console.Read();
        }
    }
}
