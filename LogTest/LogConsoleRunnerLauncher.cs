using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LogTest
{
  public class LogConsoleRunnerLauncher
  {
    [Fact]
    public async Task LaunchMultipleRunners()
    {
      string logFilePath = Path.GetTempFileName();
      using var ensureLogFileDisposed = new Nito.Disposables.AnonymousDisposable(() => File.Delete(logFilePath));

      string logConsoleRunnerAppExePath = Path.GetFullPath(
        Path.Combine(
          Path.GetDirectoryName(this.GetType().Assembly.Location),
          @"..\..\..\..\LogConsoleRunner\bin\Debug\LogConsoleRunner.exe"));      
      var startInfo = new ProcessStartInfo(logConsoleRunnerAppExePath)
      {
        Arguments = logFilePath,
        UseShellExecute = false
      };
      const int LaunchProcessCount = 10;
      Process[] processes = Enumerable
        .Range(0, LaunchProcessCount)
        .Select(i => Process.Start(startInfo))
        .ToArray();
      while (!processes.All(p => p.HasExited))
      {
        await Task.Delay(LogConsoleRunner.Program.DelayBetweenLogWrites);
      }

      string[] lines = File.ReadAllLines(logFilePath);
      Assert.Equal(LaunchProcessCount * LogConsoleRunner.Program.LogWritesCount, lines.Length);
    }
  }
}
