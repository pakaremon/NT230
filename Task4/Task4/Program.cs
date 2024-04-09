using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;

class Program
{

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    // Constants for the MessageBox type parameter
    public const uint MB_OK = 0x00000000;
    public const uint MB_ICONINFORMATION = 0x00000040;


    [DllImport("advapi32.dll", SetLastError = true)]
    static extern int RegOpenKeyEx(
        IntPtr hKey,
        string lpSubKey,
        int ulOptions,
        int samDesired,
        out IntPtr phkResult);

    [DllImport("advapi32.dll", SetLastError = true)]
    static extern int RegSetValueEx(
        IntPtr hKey,
        string lpValueName,
        int Reserved,
        RegistryValueKind dwType,
        IntPtr lpData,
        int cbData);

    [DllImport("advapi32.dll", SetLastError = true)]
    static extern int RegCloseKey(IntPtr hKey);

    const int HKEY_CURRENT_USER = unchecked((int)0x80000001);
    const int KEY_WRITE = 0x00020006;
    const int REG_SZ = 1;

    static void Main(string[] args)
    {
        
        MessageBox(IntPtr.Zero, "20521143", "Greetings", MB_OK | MB_ICONINFORMATION);

        // Add the program to the registry key
        AddToRunKey();

    }

    // Function to add the program to the registry key
    static void AddToRunKey()
    {
        // Get the executable path
        string executablePath = System.Reflection.Assembly.GetEntryAssembly().Location;
        string newFilePath = Path.ChangeExtension(executablePath, ".exe");

        // Open the registry key
        IntPtr hKey = IntPtr.Zero;
        int res = RegOpenKeyEx(
            (IntPtr)HKEY_CURRENT_USER,
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
            0,
            KEY_WRITE,
            out hKey);

        if (res == 0) // Check if operation is successful
        {
            // Write the value to the registry
            res = RegSetValueEx(
                hKey,
                "hack",
                0,
                RegistryValueKind.String,
                Marshal.StringToHGlobalAnsi(newFilePath),
                newFilePath.Length);

            if (res == 0) // Check if operation is successful
            {
                Console.WriteLine("Registry key created successfully.");
            }
            else
            {
                Console.WriteLine("Error: Unable to set registry value.");
            }

            // Close the registry key
            RegCloseKey(hKey);
        }
        else
        {
            Console.WriteLine("Error: Unable to open registry key.");
        }
    }
}
