using UnityEngine;
using System.Collections;

/// <summary>
/// Tracks information for a loading task.
/// </summary>
public class GameLoaderTask
{
    private float complete;

    /// <summary>
    /// Name tht describes the task.
    /// </summary>
    public string Name
    {
        get;
        private set;
    }

    /// <summary>
    /// The percentage completeness of the task.
    /// </summary>
    public float PercentageComplete
    {
        get
        {
            return complete;
        }
        set
        {
            complete = Mathf.Clamp(value, 0f, 1f);
        }
    }

    /// <summary>
    /// Creates a task for the game loader to track.
    /// </summary>
    /// <param name="name">Name that describes the task.</param>
    public GameLoaderTask (string name)
    {
        Name = name;
        complete = 0f;
    }
}
