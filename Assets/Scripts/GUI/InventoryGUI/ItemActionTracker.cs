using UnityEngine;
using System.Collections;
using UnityEngine.Events;

/// <summary>
/// Item action tracker.
/// </summary>
public class ItemActionTracker
{
	public string ActionName;
	public delegate Event ActionDelegate();
	public ActionDelegate ActionSubscription;
	public bool ExecuteAction;
}
