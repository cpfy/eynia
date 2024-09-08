using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
// using System.Windows.Input;
using Avalonia.Input; // 使用 Avalonia 的 Key 枚举

public class KeyboardHook : IDisposable
{
    public enum Parameters
    {
        None,
        AllowAltTab,
        AllowWindowsKey,
        AllowAltTabAndWindows,
        PassAllKeysToNextApp
    }
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;

    private IntPtr _hookID = IntPtr.Zero;
    private LowLevelKeyboardProc? _proc;

    // support for blocking more keys
    private bool PassAllKeysToNextApp = false;
    private bool AllowAltTab = false;
    private bool AllowWindowsKey = false;

    // public static bool IsKeyDown(Key key) => Keys.Contains(key);

    public event KeyboardHookEventHandler? KeyIntercepted;

    // 三种不同的构造函数
    public KeyboardHook()
    {
        if (OperatingSystem.IsWindows()) // 仅在 Windows 平台上启用
        {
            _proc = HookCallback;
            _hookID = SetHook(_proc);
        }
    }

    public KeyboardHook(string param) : this()
    {
        if (!string.IsNullOrEmpty(param) && Enum.IsDefined(typeof(Parameters), param))
        {
            SetParameters((Parameters)Enum.Parse(typeof(Parameters), param));
        }
    }

    public KeyboardHook(Parameters param) : this()
    {
        SetParameters(param);
    }

    private void SetParameters(Parameters param)
    {
        switch (param)
        {
            case Parameters.None:
                break;
            case Parameters.AllowAltTab:
                AllowAltTab = true;
                break;
            case Parameters.AllowWindowsKey:
                AllowWindowsKey = true;
                break;
            case Parameters.AllowAltTabAndWindows:
                AllowAltTab = true;
                AllowWindowsKey = true;
                break;
            case Parameters.PassAllKeysToNextApp:
                PassAllKeysToNextApp = true;
                break;
        }
    }

    private IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        Process curProcess = Process.GetCurrentProcess();
        if(curProcess.MainModule == null)
        {
            return IntPtr.Zero;
        }
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule?.ModuleName ?? string.Empty), 0);
        }
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            Key key = ConvertToAvaloniaKey(vkCode);

            // KeyIntercepted?.Invoke(new KeyboardHookEventArgs(key, false));
            // return (IntPtr)1; // 阻止按键传递

            bool allowKey = PassAllKeysToNextApp;

            if (AllowWindowsKey && (key == Key.LWin || key == Key.RWin))
            {
                allowKey = true;
            }

            // TODO
            // if (AllowAltTab && key == Key.Tab && (Keyboard.IsKeyDown(Key.LeftAlt) || KeyboardDevice.IsKeyDown(Key.RightAlt)))
            if(AllowAltTab){}
            // {
            //     allowKey = true;
            // }

            KeyIntercepted?.Invoke(new KeyboardHookEventArgs(key, allowKey));

            if (!allowKey)
            {
                return (IntPtr)1; // 阻止按键传递
            }
        }

        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }

    public void Dispose()
    {
        if (_hookID != IntPtr.Zero)
        {
            UnhookWindowsHookEx(_hookID);
            _hookID = IntPtr.Zero;
        }
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    // 将虚拟键码转换为 Avalonia 的 Key 枚举
    private Key ConvertToAvaloniaKey(int vkCode)
    {
        return vkCode switch
        {
            0x5B => Key.LWin, // 左 Windows 键
            0x5C => Key.RWin, // 右 Windows 键
            0x09 => Key.Tab,  // Tab 键
            0xA4 => Key.LeftAlt, // 左 Alt 键
            0xA5 => Key.RightAlt, // 右 Alt 键
            0x10 => Key.LeftShift, // 左 Shift 键
            0x11 => Key.LeftCtrl, // 左 Ctrl 键
            0x14 => Key.CapsLock, // CapsLock 键
            // 你可以在这里添加更多的键码转换
            _ => Key.None,
        };
    }
}
public class KeyboardHookEventArgs : EventArgs
{
    public Key KeyCode { get; private set; }
    public bool PassThrough { get; set; }

    public KeyboardHookEventArgs(Key keyCode, bool passThrough)
    {
        KeyCode = keyCode;
        PassThrough = passThrough;
    }
}

public delegate void KeyboardHookEventHandler(KeyboardHookEventArgs e);