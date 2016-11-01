using UnityEngine;
using System.Collections;

public class FloodWater : MonoBehaviour 
{
	[Range(0.0f, 1.0f)]
	public float LerpSpeed = 1;

	private float baseCurrentLevel;
	public float BaseCurrentLevel
	{
		get
		{
			return this.baseCurrentLevel;
		}
		set
		{
			// update parameter
			this.baseCurrentLevel = value;
		}
	}

	private float currentLevelOffset = 0;
	public float CurrentLevelOffset
	{
		get
		{
			return this.currentLevelOffset;
		}
		set
		{
			// update parameter
			this.currentLevelOffset = value;
		}
	}

	public float CurrentLevel
	{
		get
		{
			return this.currentLevelOffset + this.baseCurrentLevel;
		}
		set
		{
			// pass
		}
	}

	/// <summary>
	/// Updates the tide level based on class members
	/// </summary>
	private void updateTideLevel()
	{
		// create new position
		Vector3 newPosition = this.gameObject.transform.position;
		newPosition.y += this.currentLevelOffset;

		// Update height of the tide
		this.transform.position = Vector3.Lerp(this.gameObject.transform.position, newPosition, this.LerpSpeed);
	}

	/// <summary>
	/// Set the base water level at start
	/// </summary>
	void Start()
	{
		this.baseCurrentLevel = this.gameObject.transform.position.y;

		// 1000 is a nice number for the slider in the unity editor. If
		// this value is at, for example, .4 it will go all the way to
		// the top of the building. This 1000 keeps the tide from getting
		// out of control and makes the slider more useful for editing.
		this.LerpSpeed /= 1000;
	}

	/// <summary>
	/// Update the water level
	/// </summary>
	void FixedUpdate()
	{
		this.updateTideLevel();
	}
}
