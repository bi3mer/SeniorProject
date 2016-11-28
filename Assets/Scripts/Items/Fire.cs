using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{
	/// <summary>
    /// When next to fire set IsByFire bool to true.
    /// </summary>
	public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Game.Instance.PlayerInstance.Controller.IsByFire = true;
        }
	}

    /// <summary>
    /// When not next to fire set IsByFire bool to false.
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Game.Instance.PlayerInstance.Controller.IsByFire = false;
        }
    }
}
