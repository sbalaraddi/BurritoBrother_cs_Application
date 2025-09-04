using System;
using System.IO;
using System.Text;

public static class Logging
{
    private static readonly string defaultLogFile = "BurritoBrothersLogFile.txt";

    /// <summary>
    /// Appends a log entry to the Burrito Brothers log file.
    /// </summary>
    public static void LogFile(string logString)
    {
        try
        {
            // Ensure directory exists (writes in current working dir by default)
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), defaultLogFile);

            // Append text with newline
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
}
