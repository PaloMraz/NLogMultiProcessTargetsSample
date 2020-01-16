using LogLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogConsoleRunner
{
  public static class Program
  {
    public const int LogWritesCount = 10;
    public static readonly TimeSpan DelayBetweenLogWrites = TimeSpan.FromMilliseconds(25);

    static async Task Main(string[] args)
    {
      string logFilePath = args.FirstOrDefault();
      if (string.IsNullOrWhiteSpace(logFilePath))
      {
        throw new InvalidOperationException("Must specify logging file path as an argument.");
      }

      logFilePath = Path.GetFullPath(logFilePath);
      Process currentProcess = Process.GetCurrentProcess();
      var logger = new CompositeLogger(logFilePath);
      for(int i = 0; i < LogWritesCount; i++)
      {
        logger.Log($"Message from {currentProcess.ProcessName}#{currentProcess.Id} at {DateTimeOffset.Now:O}");
        await Task.Delay(DelayBetweenLogWrites);
      }
    }
  }
}
