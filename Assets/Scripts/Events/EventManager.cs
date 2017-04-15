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
	/// Raft boarded event
	/// </summary>
	public event PlayerBoardRaftDelegate PlayerBoardRaftSubscription;
	public delegate void PlayerBoardRaftDelegate();

	/// <summary>
	/// Weather updated event
	/// </summary>
	public event WeatherUpdatedDelegate WeatherUpdatedSubscription;
	public delegate void WeatherUpdatedDelegate(float precipitation);

	/// <summary>
	/// Lightning strike event
	/// </summary>
	public event WeatherLightningDelegate WeatherLightningSubscription;
	public delegate void WeatherLightningDelegate(Vector3 lightningPos);

	/// <summary>
	/// Radio music channel event
	/// </summary>
	public event RadioMusicDelegate RadioMusicSubscription;
	public delegate void RadioMusicDelegate(bool turnedOn);

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

	/// <summary>
	/// Weather updated function called by WeatherSystem. Notifies WeatherUpdatedSubscription subscribers.
	/// </summary>
	public void WeatherUpdated(float precipitation)
	{
		if (WeatherUpdatedSubscription != null) 
		{
			WeatherUpdatedSubscription (precipitation);
		}
	}


	/// <summary>
	/// Radio music turned on function called by Radio. Notifies RadioMusicSubscription subscribers.
	/// </summary>
	public void RadioMusicTurnedOn()
	{
		if (RadioMusicSubscription != null) 
		{
			RadioMusicSubscription (true);
		}
	}

	/// <summary>
	/// Radio music turned off function called by Radio. Notifies RadioMusicSubscription subscribers.
	/// </summary>
	public void RadioMusicTurnedOff()
	{
		if (RadioMusicSubscription != null) 
		{
			RadioMusicSubscription (false);
		}
	}

	/// <summary>
	/// Lightning strike called by Lightning. Notifies WeatherLightningSubscription subscribers.
	/// </summary>
	/// <param name="lightningPos">Lightning position.</param>
	public void LightningStrike(Vector3 lightningPos)
	{
		if (WeatherLightningSubscription != null)
		{
			WeatherLightningSubscription (lightningPos);
		}

	}
}
