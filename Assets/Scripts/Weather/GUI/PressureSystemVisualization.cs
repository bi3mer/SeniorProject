using UnityEngine;
using System.Collections;

public class PressureSystemVisualization : MonoBehaviour 
{
	[SerializeField]
	private float defaultHeight = 10f;

	[SerializeField]
	private float defaultRadius = 1f;

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
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(center, this.defaultRadius);
		}
	}
}