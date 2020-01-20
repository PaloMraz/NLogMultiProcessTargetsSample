using System;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace LogLib
{
  public class CompositeLogger
  {
    private readonly ILogger _logger;

    public CompositeLogger(string logFilePath)
    {
      var fileTarget = new FileTarget("file")
      {
        FileName = logFilePath,
        AutoFlush = true,
        KeepFileOpen = true,
        ConcurrentWrites = true
      };
      var asyncTargetWrapper = new AsyncTargetWrapper("async", fileTarget)
      {
        OverflowAction = AsyncTargetWrapperOverflowAction.Discard
      };


      // This does not help either:
      // Target targetToRegister = fileTarget;
      Target targetToRegister = asyncTargetWrapper;

      var config = new LoggingConfiguration();
      config.AddTarget(targetToRegister);
      config.AddRuleForAllLevels(targetToRegister);
      LogManager.Configuration = config;

      this._logger = LogManager.GetLogger("Default");
    }

    public void Log(string message) => this._logger.Trace(message);
  }
}
