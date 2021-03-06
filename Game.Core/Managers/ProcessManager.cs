﻿using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core.Extensions;
using Game.Core.Managers.Interfaces;
using Game.Core.Processes;
using NLog;

namespace Game.Core.Managers
{
  /// <summary>
  ///   The general process manager implementation.
  /// </summary>
  public class ProcessManager
    : IProcessManager
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private const int InitialListSize = 10;

    private readonly List<ProcessBase> m_processes = 
      new List<ProcessBase>(InitialListSize);
    private readonly List<int> m_toRemove = new List<int>(InitialListSize);

    public ProcessManager()
    {
      CanPause = true;
    }

    #region IManager

    public bool CanPause { get; private set; }
    
    public bool Paused { get; set; }

    public bool Initialize()
    {
      Log.Trace("ProcessManager Initializing");

      return true;
    }

    public bool PostInitialize()
    {
      Log.Trace("ProcessManager Post-Initializing");

      return true;
    }

    public void Update(float deltaTime)
    {
      if (Paused)
      {
        return;
      }

      if (m_toRemove.Any())
      {
        var notRemoved = m_processes.RemoveAllItems(m_toRemove,
          (process, id) => process.Id == id);

        if (notRemoved.Any())
        {
          Log.Error("Process ids were slated for removal but not removed: {0}",
            string.Join(",", notRemoved));
        }
        
        m_toRemove.Clear();
      }

      var runningProcesses = m_processes.Where(p => p.IsRunning);
      foreach (var process in runningProcesses)
      {
        process.Update(deltaTime);
      }
    }

    public void Shutdown()
    {
      Log.Trace("ProcessManager Shutting Down");

      Log.Debug("Aborting {0} process chains", m_processes.Count);
      foreach (var process in m_processes)
      {
        process.AbortAll();
      }
      
      m_processes.Clear();
    }

    #endregion
    #region IProcessManager

    public IReadOnlyCollection<ProcessBase> Processes
    {
      get { return m_processes; }
    }

    public ProcessBase GetProcess(int id)
    {
      return m_processes.SingleOrDefault(p => p.Id == id);
    }

    public IEnumerable<T> GetProcesses<T>() 
      where T : ProcessBase
    {
      return m_processes.OfType<T>();
    }

    public void AddProcess(ProcessBase process)
    {
      if (process == null) throw new ArgumentNullException("process");
      if (m_processes.Any(p => p.Id == process.Id))
        throw new InvalidOperationException(string.Format(
          "Process id {0} is already in use", process.Id));
      if (!process.IsInitialized)
        throw new InvalidOperationException("process is not initialized");

      process.Succeeded += HandleProcessSucceeded;
      process.Failed += HandleProcessFailed;
      process.Aborted += HandleProcessAborted;

      m_processes.Add(process);
      Log.Trace("Added {0}", process.Name);
    }
    
    #endregion

    private void RemoveProcess(ProcessBase process)
    {
      process.Succeeded -= HandleProcessSucceeded;
      process.Failed -= HandleProcessFailed;
      process.Aborted -= HandleProcessAborted;

      m_toRemove.Add(process.Id);
      Log.Trace("Flagged process {0} for removal", process.Name);

      if (process.Child == null)
      {
        return;
      }

      var activateChild = process.HasSucceeded ||
                          process.ActivateChildOnFailure ||
                          process.ActivateChildOnAbort;
      var child = process.RemoveChild();
      if (!activateChild)
      {
        Log.Debug("Children of {0} are not activating, aborting them",
          process.Name);
        child.AbortAll();
        return;
      }

      if (!child.IsInitialized && !child.Initialize())
      {
        Log.Error("{0}, child of {1}, was not initialized and failed to " +
                  "initialize, aborting its chain", child.Name, process.Name);
        child.AbortAll();
        return;
      }

      child.Succeeded += HandleProcessSucceeded;
      child.Failed += HandleProcessFailed;
      child.Aborted += HandleProcessAborted;
      m_processes.Add(child);
      child.Resume();
    }

    #region Event Handlers

    private void HandleProcessSucceeded(object sender, EventArgs eventArgs)
    {
      var process = (ProcessBase) sender;
      RemoveProcess(process);
    }

    private void HandleProcessFailed(object sender, EventArgs eventArgs)
    {
      var process = (ProcessBase)sender;
      RemoveProcess(process);
    }

    private void HandleProcessAborted(object sender, EventArgs eventArgs)
    {
      var process = (ProcessBase)sender;
      RemoveProcess(process);
    }

    #endregion
  }
}
