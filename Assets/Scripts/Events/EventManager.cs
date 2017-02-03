using UnityEngine;
using System;
using System.Collections;

public class EventManager
{
	/// <summary>
	/// Initializes a new instance of the <see cref="EventManager"/> class.
	/// </summary>
	private EventManager()
	{

	}

	/// <summary>
	/// The instance.
	/// </summary>
	private static EventManager instance;

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static EventManager Instance 
	{
		get 
		{
			if (instance == null) 
			{
				instance = new EventManager ();
			}

			return instance;
		}
	}

	/// <summary>
	/// Storm started event -- can be subscribed to by calling EventManager.Instance.StormStartedSubscription += functionToCall
	/// </summary>
	public delegate void StormStartedDelegate();
	public event StormStartedDelegate StormStartedSubscription;

	/// <summary>
	/// Storm stopped event
	/// </summary>
	public delegate void StormStoppedDelegate();
	public event StormStoppedDelegate StormStoppedSubscription;

	/// <summary>
	/// Storm stopped event
	/// </summary>
	public delegate void PlayerBoardRaftDelegate();
	public event PlayerBoardRaftDelegate PlayerBoardRaftSubscription;


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
