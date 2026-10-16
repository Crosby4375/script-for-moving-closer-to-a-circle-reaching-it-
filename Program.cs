using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace PZWalker
{
    class Program
    {
       
        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public uint type;
            public InputUnion U;
            public static int Size => Marshal.SizeOf(typeof(INPUT));
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)] public MOUSEINPUT mi;
            [FieldOffset(0)] public KEYBDINPUT ki;
            [FieldOffset(0)] public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT { public int dx; public int dy; public uint mouseData; public uint dwFlags; public uint time; public IntPtr dwExtraInfo; }
        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT { public ushort wVk; public ushort wScan; public uint dwFlags; public uint time; public IntPtr dwExtraInfo; }
        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT { public uint uMsg; public ushort wParamL; public ushort wParamH; }

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);

        
        const uint INPUT_KEYBOARD = 1;
        const uint KEYEVENTF_KEYDOWN = 0x0000;
        const uint KEYEVENTF_KEYUP = 0x0002;

        const ushort VK_W = 0x57;
        const ushort VK_A = 0x41;
        const ushort VK_S = 0x53;
        const ushort VK_D = 0x44;

       
        const ushort VK_ESCAPE = 0x1B;

        static bool isRunning = true;

        static void Main(string[] args)
        {
            Console.Title = "PZ Walker Helper";
            Console.WriteLine("=== Project Zomboid Walker ===");
            Console.WriteLine("1. Запустите Project Zomboid.");
            Console.WriteLine("2. Сверните игру в окно (Windowed Mode рекомендуется) или оставьте на втором мониторе.");
            Console.WriteLine("3. Сделайте окно игры АКТИВНЫМ (кликните по нему).");
            Console.WriteLine("4. Вернитесь сюда и нажмите ENTER для старта.");
            Console.WriteLine("5. Для ЭКСТРЕННОЙ ОСТАНОВКИ нажмите ESC в игре или здесь.");
            Console.WriteLine("==============================");

            Console.ReadLine();

            Console.WriteLine("Запуск через 3 секунды... Успейте выбрать окно игры!");
            Thread.Sleep(3000);

            try
            {
                while (isRunning)
                {
                    
                    if (GetAsyncKeyState(VK_ESCAPE) < 0)
                    {
                        Console.WriteLine("Обнаружено нажатие ESC! Остановка...");
                        StopAllKeys();
                        break;
                    }

                    WalkCircle();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка: {e.Message}");
                StopAllKeys();
            }

            Console.WriteLine("Скрипт остановлен. Нажмите любую клавишу для выхода.");
            Console.ReadKey();
        }

        static void WalkCircle()
        {
            Random rnd = new Random();

            
            if (!CheckStop()) return;
            PressKey(VK_W);
            Thread.Sleep(50); 
            PressKey(VK_D);
            Thread.Sleep(1500 + rnd.Next(0, 500)); 
            ReleaseKey(VK_D);
            ReleaseKey(VK_W);
            Thread.Sleep(100);

           
            if (!CheckStop()) return;
            PressKey(VK_S);
            Thread.Sleep(50);
            PressKey(VK_D);
            Thread.Sleep(1500 + rnd.Next(0, 500));
            ReleaseKey(VK_D);
            ReleaseKey(VK_S);
            Thread.Sleep(100);

           
            if (!CheckStop()) return;
            PressKey(VK_S);
            Thread.Sleep(50);
            PressKey(VK_A);
            Thread.Sleep(1500 + rnd.Next(0, 500));
            ReleaseKey(VK_A);
            ReleaseKey(VK_S);
            Thread.Sleep(100);

            
            if (!CheckStop()) return;
            PressKey(VK_W);
            Thread.Sleep(50);
            PressKey(VK_A);
            Thread.Sleep(1500 + rnd.Next(0, 500));
            ReleaseKey(VK_A);
            ReleaseKey(VK_W);
            Thread.Sleep(100);
        }

        static bool CheckStop()
        {
            if (GetAsyncKeyState(VK_ESCAPE) < 0 || GetAsyncKeyState(0x20) < 0) 
            {
                isRunning = false;
                StopAllKeys();
                return false;
            }
            return true;
        }

        static void PressKey(ushort key)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = INPUT_KEYBOARD;
            inputs[0].U.ki.wVk = key;
            inputs[0].U.ki.dwFlags = KEYEVENTF_KEYDOWN;
            SendInput(1, inputs, INPUT.Size);
        }

        static void ReleaseKey(ushort key)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = INPUT_KEYBOARD;
            inputs[0].U.ki.wVk = key;
            inputs[0].U.ki.dwFlags = KEYEVENTF_KEYUP;
            SendInput(1, inputs, INPUT.Size);
        }

        static void StopAllKeys()
        {
          
            ReleaseKey(VK_W);
            ReleaseKey(VK_A);
            ReleaseKey(VK_S);
            ReleaseKey(VK_D);
        }
    }
}