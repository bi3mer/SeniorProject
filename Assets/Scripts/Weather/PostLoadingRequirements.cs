using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostLoadingRequirements : MonoBehaviour
{
    /// <summary>
    /// subscribe functions to run after a loading has finished
    /// </summary>
	void Start ()
    {
        Game.Instance.PauseInstance.ResumeUpdate += this.postLoading;
	}

    /// <summary>
    /// Run updates for scripts
    /// </summary>
    private void postLoading()
    {
        // City boundaries needs to be updated
        Game.Instance.CityBounds.CityBounds = Game.Instance.CityInstance.BoundingBox;

        // create pressure system
        Game.Instance.WeatherInstance.WeatherPressureSystems.Initialize(Game.Instance.CityBounds);

        // subscribe pressure system to clock updates
        Game.Instance.ClockInstance.SecondUpdate += Game.Instance.WeatherInstance.WeatherPressureSystems.UpdatePressureSystem;

        // start spawning lightning
        FindObjectOfType<Lightning>().SpawnLightning();
    }

    private void OnDestroy()
    {
        Game.Instance.ClockInstance.SecondUpdate -= Game.Instance.WeatherInstance.WeatherPressureSystems.UpdatePressureSystem;
    }
}
