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
    [Tooltip("The color of the particle fog based on the time of day, with 0 and 1 being midnight.")]
    private Gradient particleFogColors;

    [SerliazeField]
    [Tooltip("Color modifier based on storm intensity.")]
    private Gradient stormColorModifier;

    [SerializeField]
    [Tooltip("Threshold at which fog becomes very dense.")]
    private float fogThreshold;

    [SerializeField]
    [Tooltip("The particle system generating fog.")]
    private ParticleSystem fogSystem;

    /// <summary>
    /// Calculates time of day and updates values.
    /// </summary>
	void Update ()
    {
        float time = Game.Instance.ClockInstance.CurrentGameTimeInHours / Clock.TwentyFourHours;
        float intensity = // Get intensity 
        updateCameraFog(time, intensity);
        updateParticleFogColor(time, intensity);
        updateParticleFogAmount();
	}

    /// <summary>
    /// Sets camera render fog based on time of day.
    /// </summary>
    /// <param name="time">Time of day.</param>
    /// <param name="intensity">Intensity of storm.</param>
    private void updateCameraFog (float time, float intensity)
    {
        RenderSettings.fogColor = cameraFogColors.Evaluate(time) * stormColorModifier(intensity);
    }

    /// <summary>  
    /// Sets particle-based fog based on time of day.
    /// </summary>
    /// <param name="time">Time of day.</param>
    /// <param name="intensity">Intensity of storm.</param>
    private void updateParticleFogColor (float time, float intensity)
    {
        Color color = particleFogColors.Evaluate(time) * stormColorModifier(intensity);
        GradientColorKeys[] keys = fogSystem.colorOverLifetime.colorKeys;
        keys[0].color = color;
        keys[1].color = color * Color.gray; // Darkens color by 50%;
    }

    /// <summary>
    /// Updates the severity of the fog.
    /// Fog forms when the difference between the air temperature and dewpoint is fogThreshold, or less. 
    /// </summary>
    private void updateParticleFogAmount ()
    {
        float diff = Math.Abs(Game.Instance.WeatherSystem.CurrentTemperature - Game.Instance.WeatherSystem.CurrentDewPoint);
        if (diff <= fogThreshold)
        {
            float fogStrenth = 1.0f - diff / fogThreshold;
            // TODO: Increase fog amount proportional to fogStrength
        }
    }
}