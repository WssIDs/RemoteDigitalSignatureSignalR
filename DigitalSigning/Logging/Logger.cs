namespace DigitalSigning.Logging;

/// <summary>
/// 
/// </summary>
public static class Logger
{
    private readonly static string path = "logs";
    private readonly static string filename = Path.Combine(path, $"log_{DateTime.Now:dd_MM_yyyy}.log");

    public static void Init()
    {
        CreateDir();

        var di = new DirectoryInfo(path);

        var files = di.GetFiles("*.log").OrderByDescending(x => x.CreationTimeUtc).ToList();

        if(files.Count > 10)
        {
            for (int i = 0; i < 10; i++)
            {
                files[i].Delete();
            }
        }

    }

    private static void CreateDir()
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public static void Log(string logMessage)
    {
        try
        {
            CreateDir();

            using StreamWriter w = File.AppendText(filename);
            InternalLog(logMessage, w);
        }
        catch
        {
        }
    }

    public static void InternalLog(string message, TextWriter txtWriter)
    {
        try
        {
            txtWriter.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()} - {message}");
        }
        catch
        {
        }
    }
}
