using UnityEngine;
using System.Collections;

/// <summary>
/// This script modifies the _Blend variable in the custom shader, Skybox/Blended based on the Game.Insance's clock.
/// </summary>
public class DayNightSkybox : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The skybox material that's used in the lighting tab")]
    private Material skybox;

    private const string skyboxBlendName = "_Blend";

    /// <summary>
    ///  Update the skybox.
    ///  A blend of 0 is day and 1 is night.
    /// </summary>
	void Update ()
    {
        // This goes up to 24. Representing one day.
        // Start at midnight so we want 1.
        // At high noon we want 0
		float currentTime = Game.Instance.ClockInstance.CurrentTime % Game.Instance.ClockInstance.TwelveHours;

		if(Game.Instance.ClockInstance.IsDay)
		{
			skybox.SetFloat(skyboxBlendName, 1f - currentTime / Game.Instance.ClockInstance.TwelveHours);
		}
		else
		{
			skybox.SetFloat(skyboxBlendName, currentTime / Game.Instance.ClockInstance.TwelveHours);
		}
	}
}
