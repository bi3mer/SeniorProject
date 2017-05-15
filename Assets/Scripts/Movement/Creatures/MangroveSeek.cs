using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MangroveSeek : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    private const string oceanTag = "Water";

    bool collided = false;

    /// <summary>
    /// Seek player during a storm
    /// </summary>
	void Update ()
    {
        if (!this.collided && Game.Instance.WeatherInstance.OnGoingStorm)
        {
        	// update position with seek behavior
			this.transform.position += (Game.Player.WorldPosition - this.transform.position).normalized * Time.deltaTime * this.speed;
        }
	}

	/// <summary>
	/// Raises the collision enter event.
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnTriggerEnter(Collider other)
	{
		this.collided = !other.CompareTag(MangroveSeek.oceanTag);
	}

	/// <summary>
	/// Raises the active event.
	/// </summary>
	void OnActive()
	{
		// tree starts moving again once it has been re-activated by the 
		// creature manager
		this.collided = false;
	}
}
