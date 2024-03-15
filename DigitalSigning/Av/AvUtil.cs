using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace DigitalSigning.Av;

internal static class AvUtil
{
    private static IntPtr MarshalToPointer(object data)
    {
        var buf = Marshal.AllocHGlobal(Marshal.SizeOf(data));
        Marshal.StructureToPtr(data, buf, false);
        return buf;
    }

    private static object MarshalToStruct(IntPtr buf, Type t)
    {
        return Marshal.PtrToStructure(buf, t);
    }

    private static unsafe T ToStruct<T>(this byte[] buf) where T : unmanaged
    {
        fixed (byte* ptr = buf)
        {
            return *(T*)ptr;
        }
    }

    private static unsafe byte[] FromStruct<T>(T str) where T : unmanaged
    {
        var ret = new byte[sizeof(T)];
        fixed (byte* ptr = ret)
        {
            *(T*)ptr = str;
        }
        return ret;
    }

    private static unsafe byte[] ConvertStruct<T>(ref T str) where T : struct
    {
        var size = Marshal.SizeOf(str);
        var arr = new byte[size];

        fixed (byte* arrPtr = arr)
        {
            Marshal.StructureToPtr(str, (IntPtr)arrPtr, true);
        }

        return arr;
    }

    public static byte[] StreamToByteArray(this Stream input)
    {
        using var ms = new MemoryStream();
        input.CopyTo(ms);
        return ms.ToArray();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    /// <param name="append"></param>
    /// <returns></returns>
    internal static bool Write2File(string s, bool append = true)
    {
        var result = false;
        //var path = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        var processModule = Process.GetCurrentProcess().MainModule;
        if (processModule == null) return false;
        var path = System.IO.Path.GetDirectoryName(processModule.FileName) + "\\log.txt";
        try
        {
            using (var sw = new StreamWriter(path, append, System.Text.Encoding.Default))
            {
                sw.WriteLine(s);
            }
            result = true;
        }
        catch (Exception e)
        {
            Debug.WriteLine("Exception Message: " + e.Message);
        }

        return result;
    }
    /// <summary>
    /// Переключение автостарта программы в реестре
    /// </summary>
    /// <param name="isChecked"></param>
    internal static void RegisterInStartup(bool isChecked)
    {
        var registryKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        if (isChecked)
        {
            var p = Assembly.GetEntryAssembly()?.Location;
            if (p != null) registryKey?.SetValue("ApplicationName", p);
            //registryKey.SetValue("ApplicationName", Application.ExecutablePath);
        }
        else
        {
            registryKey?.DeleteValue("ApplicationName");
        }
    }
    /// <summary>
    /// Получить рабочий каталог приложения
    /// </summary>
    public static string AssemblyDirectory
    {
        get
        {
            var codeBase = Assembly.GetExecutingAssembly().Location;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
    /// <summary>
    /// getFileVersion
    /// </summary>
    /// <returns></returns>
    public static string GetFileVersion()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var fvNfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        var version = fvNfo.FileVersion;
        return version;
    }
    /// <summary>
    /// ByteArrayToString
    /// </summary>
    /// <param name="ba"></param>
    /// <returns></returns>
    public static string ByteArrayToString(byte[] ba)
    {
        StringBuilder hex = new(ba.Length * 2);
        foreach (var b in ba)
            hex.Append($"{b:x2}");
        return hex.ToString();
    }
    /// <summary>
    /// Получить timestamp создания сборки
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="tzi"></param>
    /// <returns>DateTime</returns>
    public static DateTime GetLinkerDateTime(this Assembly assembly, TimeZoneInfo tzi = null)
    {
        // Constants related to the Windows PE file format.
        const int PE_HEADER_OFFSET = 60;
        const int LINKER_TIMESTAMP_OFFSET = 8;

        // Discover the base memory address where our assembly is loaded
        var entryModule = assembly.ManifestModule;
        var hMod = Marshal.GetHINSTANCE(entryModule);
        if (hMod == IntPtr.Zero - 1) throw new Exception("Failed to get HINSTANCE.");

        // Read the linker timestamp
        var offset = Marshal.ReadInt32(hMod, PE_HEADER_OFFSET);
        var secondsSince1970 = Marshal.ReadInt32(hMod, offset + LINKER_TIMESTAMP_OFFSET);

        // Convert the timestamp to a DateTime
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var linkTimeUtc = epoch.AddSeconds(secondsSince1970);
        var dt = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tzi ?? TimeZoneInfo.Local);
        return dt;
    }
}