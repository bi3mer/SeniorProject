using UnityEngine;
using System.Collections;

public class PressureSystemVisualization : MonoBehaviour 
{
	[SerializeField]
	private float defaultHeight = 10f;

	[SerializeField]
	private float defaultRadius = 5f;

	/// <summary>
	/// Update gizmos to reflect location of pressure systems
	/// </summary>
	void OnDrawGizmos () 
	{
		PressureSystems system = Game.Instance.WeatherInstance.WeatherPressureSystems;

        float min = system.PressureConstants[(int) PressureConstantIndexes.PressureHurricane];
        float max = system.PressureConstants[(int) PressureConstantIndexes.PressureSiberia];

        for (int i = 0; i < system.LocalPressureSystems.Count; ++i)
		{
			Vector3 center = new Vector3(system.LocalPressureSystems[i].Position.x,
			                             this.defaultHeight,
			                             system.LocalPressureSystems[i].Position.y);

            // get pressure
            float pressure = system.LocalPressureSystems[i].Pressure;

            // get how low/high it is compared to lowest and highest values
            float strength = (max - pressure) / (max - min);

            // set color of pressure system
            Gizmos.color = Color.Lerp(Color.red, Color.blue, strength);


			Gizmos.DrawSphere(center, this.defaultRadius);
		}
	}
}
