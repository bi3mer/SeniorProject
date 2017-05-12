using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MangroveSeek : MonoBehaviour
{
    [SerializeField]
    private float speed = 1f;

    /// <summary>
    /// Seek player during a storm
    /// </summary>
	void FixedUpdate ()
    {
        if (Game.Instance.WeatherInstance.OnGoingStorm)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, Game.Player.WorldTransform.position, this.speed * Time.deltaTime);
        }
	}
}
