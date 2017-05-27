using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;
using System.Collections;

public class EventManagerUnitTests 
{
	bool stormStart;
	bool stormStop;
	bool radioOn;
	bool radioOff;
	float fakePrecip;

	[Test]
	public void StormStartTest()
	{
		stormStart = false;
		EventManager e = new EventManager ();
		e.StormStartedSubscription += StormStartTrigger;
		e.StormStart ();
		Assert.IsTrue (stormStart);
	}

	[Test]
	public void StormStopTest()
	{
		stormStop = false;
		EventManager e = new EventManager ();
		e.StormStoppedSubscription += StormStopTrigger;
		e.StormStop ();
		Assert.IsTrue (stormStop);
	}
		
	[Test]
	public void RadioOnTest()
	{
		radioOn = false;
		EventManager e = new EventManager ();
		e.RadioMusicSubscription += TurnRadioOn;
		e.RadioMusicTurnedOn ();
		Assert.IsTrue (radioOn);
	}

	[Test]
	public void RadioOffTest()
	{
		radioOff = false;
		EventManager e = new EventManager ();
		e.RadioMusicSubscription += TurnRadioOff;
		e.RadioMusicTurnedOff ();
		Assert.IsTrue (radioOff);
	}
		
	[Test]
	public void WeatherUpdateTest()
	{
		fakePrecip = 0f;
		EventManager e = new EventManager ();
		e.WeatherUpdatedSubscription += SetPrecip;
		e.WeatherUpdated (10.0f);
		Assert.AreEqual(fakePrecip, 10.0f);
	}
		
	public void StormStartTrigger()
	{
		stormStart = true;
	}

	public void StormStopTrigger()
	{
		stormStop = true;
	}

	public void TurnRadioOn()
	{
		radioOn = true;
	}

	public void TurnRadioOff()
	{
		radioOff = true;
	}

	public void SetPrecip(float precip)
	{
		fakePrecip = precip;
	}
}
