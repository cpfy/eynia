using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia.Input; // 使用 Avalonia 的 Key 枚举

public class KeyboardHook : IDisposable
{
    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;

    private IntPtr _hookID = IntPtr.Zero;
    private LowLevelKeyboardProc _proc;

    public event KeyboardHookEventHandler KeyIntercepted;

    public KeyboardHook()
    {
        if (OperatingSystem.IsWindows()) // 仅在 Windows 平台上启用
        {
            _proc = HookCallback;
            _hookID = SetHook(_proc);
        }
    }

    private IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (Process curProcess = Process.GetCurrentProcess())
        using (ProcessModule curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            Key key = ConvertToAvaloniaKey(vkCode);

            KeyIntercepted?.Invoke(new KeyboardHookEventArgs(key, false));

            return (IntPtr)1; // 阻止按键传递
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