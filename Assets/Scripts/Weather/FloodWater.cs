using UnityEngine;
using System.Collections;

public class FloodWater : MonoBehaviour 
{
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

			this.updateTideLevel();
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

			this.updateTideLevel();
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
		Vector3 newPosition = new Vector3(0,  this.CurrentLevelOffset, 0);

		// update tide position
		// TODO: Make this smoother movement (lerp)?
		this.gameObject.transform.Translate(newPosition);
	}

	/// <summary>
	/// Set the base water level at start
	/// </summary>
	void Start()
	{
		this.baseCurrentLevel = this.gameObject.transform.position.y;
	}
}
