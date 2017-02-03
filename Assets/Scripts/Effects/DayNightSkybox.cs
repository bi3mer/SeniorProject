using UnityEngine;
using System.Collections;

/// <summary>
/// This script modifies the _Blend variable in the custom shader, Skybox/Blended based on the Game.Insance's clock.
/// </summary>
public class DayNightSkybox : MonoBehaviour
{
    [SerializeField]
    private Material Skybox;

    private const string SkyboxBlendName = "_Blend";

    /// <summary>
    ///  Update the skybox.
    ///  A blend of 0 is day and 1 is night.
    /// </summary>
	void Update ()
    {
        // This goes up to 24. Representing one day.
        // Start at midnight so we want 1.
        // At high noon we want 0
        float blendVal = Mathf.Abs(Game.Instance.ClockInstance.CurrentGameTimeInHours - 12f) / 12f;
        Skybox.SetFloat(SkyboxBlendName, blendVal);
	}
}
