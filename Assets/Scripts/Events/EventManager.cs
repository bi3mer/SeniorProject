using UnityEngine;
using System;
using System.Collections;

public class EventManager
{
	/// <summary>
	/// Storm started event -- can be subscribed to by calling EventManager.Instance.StormStartedSubscription += functionToCall
	/// </summary>
	public event StormStartedDelegate StormStartedSubscription;
	public delegate void StormStartedDelegate();

	/// <summary>
	/// Storm stopped event
	/// </summary>
	public event StormStoppedDelegate StormStoppedSubscription;
	public delegate void StormStoppedDelegate();

	/// <summary>
	/// Storm stopped event
	/// </summary>
	public event PlayerBoardRaftDelegate PlayerBoardRaftSubscription;
	public delegate void PlayerBoardRaftDelegate();


	/// <summary>
	/// Storm start function called by WeatherSystem. Notifies StormStartedSubscription subscribers.
	/// </summary>
	public void StormStart()
	{
		if (StormStartedSubscription != null) 
		{
			StormStartedSubscription ();
		}
	}

	/// <summary>
	/// Storm stop function called by WeatherSystem. Notifies StormStoppedSubscription subscribers.
	/// </summary>
	public void StormStop()
	{
		if (StormStoppedSubscription != null) 
		{
			StormStoppedSubscription ();
		}
	}

	/// <summary>
	/// Raft boarded function called by PlayerController. Notifies PlayerBoardRaftSubscription subscribers.
	/// </summary>
	public void RaftBoarded()
	{
		if (PlayerBoardRaftSubscription != null) 
		{
			PlayerBoardRaftSubscription ();
		}
	}
}
