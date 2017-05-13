using UnityEngine;
using System.Collections;

/// <summary>
/// Controlls the fog based on time of day and weather conditions.
/// </summary>
public class FogController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The color of the camera fog based on the time of day, with 0 and 1 being midnight.")]
    private Gradient cameraFogColors;

	[SerializeField]
	private FogMode cameraFogMode;

	[SerializeField]
	[Range(0, 25)]
	private int cameraFogStart;

	[SerializeField]
	[Range(0, 25)]
	private int cameraFogEnd;

    [SerializeField]
    [Tooltip("The color of the particle fog based on the time of day, with 0 and 1 being midnight.")]
    private Gradient particleFogColors;

	[SerializeField]
    [Tooltip("Color modifier based on storm intensity.")]
    private Gradient stormColorModifier;

    [SerializeField]
    [Tooltip("Threshold at which fog becomes very dense.")]
    private float fogThreshold;

    [SerializeField]
    [Tooltip("The particle system generating fog.")]
    private ParticleSystem fogSystem;

	private const float hoursInADay = 24f;

	void Start ()
	{
		RenderSettings.fog = true;
		RenderSettings.fogMode = cameraFogMode;
		RenderSettings.fogStartDistance = cameraFogStart;
		RenderSettings.fogEndDistance = cameraFogEnd;
	}

    /// <summary>
    /// Calculates time of day and updates values.
    /// </summary>
	void Update ()
    {
		float time = Game.Instance.ClockInstance.CurrentGameTimeInHours / hoursInADay;
		float intensity = 0;// Get intensity 
        updateCameraFog(time, intensity);
        updateParticleFogColor(time, intensity);
        //updateParticleFogAmount();
	}

    /// <summary>
    /// Sets camera render fog based on time of day.
    /// </summary>
    /// <param name="time">Time of day.</param>
    /// <param name="intensity">Intensity of storm.</param>
    private void updateCameraFog (float time, float intensity)
    {
		RenderSettings.fogColor = cameraFogColors.Evaluate(time) * stormColorModifier.Evaluate(intensity);
    }

    /// <summary>  
    /// Sets particle-based fog based on time of day.
    /// </summary>
    /// <param name="time">Time of day.</param>
    /// <param name="intensity">Intensity of storm.</param>
    private void updateParticleFogColor (float time, float intensity)
    {
		Color color = particleFogColors.Evaluate(time) * stormColorModifier.Evaluate(intensity);
		ParticleSystem.ColorOverLifetimeModule colorModule = fogSystem.colorOverLifetime;

		Gradient gradient = new Gradient();
		gradient.SetKeys(
			new GradientColorKey[] { 
				new GradientColorKey(color, 0.0f), 
				new GradientColorKey(color * Color.gray, 1.0f) 
			},
			new GradientAlphaKey[] { 
				new GradientAlphaKey(0.0f, 0.0f), 
				new GradientAlphaKey(1.0f, 0.1f), 
				new GradientAlphaKey(1.0f, 0.9f),
				new GradientAlphaKey(0.0f, 1.0f) 
			}
		);

		colorModule.color = gradient;
    }

    /// <summary>
    /// Updates the severity of the fog.
    /// Fog forms when the difference between the air temperature and dewpoint is fogThreshold, or less. 
    /// </summary>
    private void updateParticleFogAmount ()
    {
		float diff = Mathf.Abs(Game.Instance.WeatherInstance.CurrentTemperature - Game.Instance.WeatherInstance.CurrentDewPoint);
        if (diff <= fogThreshold)
        {
            float fogStrenth = 1.0f - diff / fogThreshold;
            // TODO: Increase fog amount proportional to fogStrength
        }
    }
}