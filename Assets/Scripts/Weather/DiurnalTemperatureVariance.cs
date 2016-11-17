/// <summary>
/// A gamified version of:
/// https://en.wikipedia.org/wiki/Diurnal_temperature_variation
/// </summary>
public class DiurnalTemperatureVariance
{
	private const float nightTimeTemeprature = -5.0f;
	private const float dayTimeTemeprature   = 5.0f;

	/// <summary>
	/// Gets a float representing the temperature change at that particular time
	/// of day. This is Diurnal some it will go up and down following the same 
	/// pattern as the tide. 
	/// </summary>
	/// <value>The temperature variance.</value>
	public static float TemperatureVariance
	{
		get
		{
			if(Game.Instance.ClockInstance.IsDay)
			{
				return UnityEngine.Mathf.Lerp(DiurnalTemperatureVariance.nightTimeTemeprature,
				                              DiurnalTemperatureVariance.dayTimeTemeprature,
				                              Game.Instance.ClockInstance.GetTwelveHourPercentage());
			}
			else
			{
				return UnityEngine.Mathf.Lerp(DiurnalTemperatureVariance.dayTimeTemeprature,
				                              DiurnalTemperatureVariance.nightTimeTemeprature,
				                              Game.Instance.ClockInstance.GetTwelveHourPercentage());
			}
		}
	}
}
