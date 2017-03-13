﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Tracks all loading tasks and determines overall loading percentage.
/// </summary>
public class GameLoader
{
    private List<GameLoaderTask> tasks;

    /// <summary>
    /// Instatiate a new GameLoader
    /// </summary>
    public GameLoader()
    {
        tasks = new List<GameLoaderTask>();
    }

    /// <summary>
    /// Creates a loading task and adds it the the list of tracked tasks.
    /// </summary>
    /// <param name="name">Name describing task.</param>
    /// <returns>An instance of the task.</returns>
    public GameLoaderTask CreateGameLoaderTask(string name)
    {
        GameLoaderTask task = new GameLoaderTask(name);
        tasks.Add(task);
        return task;
    }

    /// <summary>
    /// Get overall loading percentage complete.
    /// </summary>
    public float PercentageComplete
    {
        get
        {
            // If there are 0 tasks, we're done.
            if (tasks.Count == 0)
            {
                return 1f;
            }

            float totalComplete = 0f;
            for (int i = 0; i < tasks.Count; ++i)
            {
                totalComplete += tasks[i].PercentageComplete;
            }

            return totalComplete / tasks.Count;
        }
    }

    /// <summary>
    /// Get the name of the currently processing task.
    /// </summary>
    public string CurrentTask
    {
        get
        {
            for (int i = 0; i < tasks.Count; ++i)
            {
                if (tasks[i].PercentageComplete < 1.0f)
                {
                    return tasks[i].Name + "...";
                }
            }

            return "";
        }
    }
}
