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

		for(int i = 0; i < system.LocalPressureSystems.Count; ++i)
		{
			Vector3 center = new Vector3(system.LocalPressureSystems[i].Position.x,
			                             this.defaultHeight,
			                             system.LocalPressureSystems[i].Position.y);

			// set color of pressure system
			if(system.LocalPressureSystems[i].IsHighPressure)
			{
				Gizmos.color = Color.red;
			} 
			else
			{
				Gizmos.color = Color.blue;
			}

			Gizmos.DrawSphere(center, this.defaultRadius);
		}
	}
}
