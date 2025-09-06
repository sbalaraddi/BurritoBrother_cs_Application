using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public static class Logging
{
    private static readonly string defaultLogFile = "BurritoBrothersLogFile.txt";
    private static readonly string filePath = Path.Combine(Directory.GetCurrentDirectory(), defaultLogFile);

    private static readonly string defaultLogMatrices = "BurritoBrothersLogMatrices.txt";
    private static readonly string matricesFilePath = Path.Combine(Directory.GetCurrentDirectory(), defaultLogMatrices);

    /// <summary>
    /// Appends a log entry to the Burrito Brothers log file.
    /// </summary>
    public static void LogFile(string logString)
    {
        try
        {
            using (var writer = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                writer.WriteLine(logString);
            }
        }
        catch (IOException ex)
        {
            Console.Error.WriteLine("Logging error: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error during logging: " + ex.Message);
        }
    }


    public static void LogMatrices(string logString)
    {
        try
        {
            using (var writer = new StreamWriter(matricesFilePath, true, Encoding.UTF8))
            {
                writer.WriteLine(logString);
            }
        }
        catch (IOException ex)
        {
            Console.Error.WriteLine("Logging error: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error during logging: " + ex.Message);
        }
    }

    /// <summary>
    /// Appends a log entry asynchronously to the Burrito Brothers log file.
    /// </summary>
    public static async Task LogFileAsync(string logString)
    {
        try
        {
            using (var writer = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                await writer.WriteLineAsync(logString);
            }
        }
        catch (IOException ex)
        {
            Console.Error.WriteLine("Logging error: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error during logging: " + ex.Message);
        }
    }
}
