using UnityEngine;
using System.Collections;

public class DayNight : MonoBehaviour 
{
	[SerializeField]
	private Color DayColor;
	[SerializeField]
	private Color NightColor;

	private Light DirectionaLight;

	/// <summary>
	/// Updates the color.
	/// </summary>
	/// <param name="targetColor">Target color.</param>
	/// <param name="lerpConstant">Lerp constant.</param>
	private void updateColor(Color baseColor, Color targetColor)
	{
		DirectionaLight.color = Color.Lerp(baseColor, targetColor, Game.Instance.ClockInstance.GetTwelveHourPercentage());
	}

	/// <summary>
	/// Update the color of the light from day to night and vise versa
	/// </summary>
	void Update()
	{
		if(Game.Instance.ClockInstance.IsDay)
		{
			this.updateColor(this.NightColor, this.DayColor);
		}
		else
		{
			this.updateColor(this.DayColor, this.NightColor);
		}
	}

	/// <summary>
	/// Get the light the script is attached to and set 
	/// default color
	/// </summary>
	void Start()
	{
		this.DirectionaLight = this.GetComponent<Light>();
		this.DirectionaLight.color = this.NightColor;
	}
}
