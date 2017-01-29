using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{
    /// <summary>
    /// The life span of the fire.
    /// </summary>
    public float FireLife
    {
        get;
        set;
    }

    void Start()
    {
        StartCoroutine(UpdateFireLife());
    }

    void Update()
    {
        if (FireLife == 0)
        {
            // TO DO: kill fire
        }
    }

    /// <summary>
    /// Update fire life.
    /// </summary>
    /// <returns></returns>
    public IEnumerator UpdateFireLife()
    {
        yield return new WaitForSeconds(1);
    }

    /// <summary>
    /// When next to fire set IsByFire bool to true.
    /// </summary>
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Game.Instance.PlayerInstance.Controller.IsByFire = true;
            // TO DO: Add variable for player to know fire is close
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
            // TO DO: Change variable for player to know what fire is not close
        }
    }
}
