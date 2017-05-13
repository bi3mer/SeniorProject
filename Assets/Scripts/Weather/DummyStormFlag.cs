using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// THIS IS A DUMMY SCIPT IT WILL BE REMOVED BEFORE MERGING
/// </summary>

public class DummyStormFlag : MonoBehaviour 
{
	private float delay = 5f;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(SetOffStorm());	
	}
	
	public IEnumerator SetOffStorm()
	{
		while(!Game.Instance.Loader.GameLoaded)
		{
			yield return null;
		}

		float current = 0;

		while(current < delay)
		{
			current += Time.deltaTime;
			yield return null;
		}

		Game.Instance.EventManager.StormStart();
	}
}
