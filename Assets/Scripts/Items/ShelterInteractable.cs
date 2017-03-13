using UnityEngine;
using System.Collections;

public class ShelterInteractable : InteractableObject 
{
    private const string playerTag = "Player";

    /// <summary>
    /// Gets or sets the shelter category object.
    /// </summary>
    public ShelterCategory Shelter
    {
        get;
        set;
    }

    /// <summary>
    /// Sets up shelter.
    /// </summary>
    public override void SetUp()
    {
        base.SetUp();
    }

    /// <summary>
    /// Enter the shelter.
    /// </summary>
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == playerTag)
        {
            Game.Instance.PlayerInstance.Controller.PlayerStatManager.WarmthRate.SetUnitsInShelter(Shelter.WarmthRate);
            Game.Instance.PlayerInstance.Controller.IsInShelter = true;
        }
    }

    /// <summary>
    /// Exit the shelter.
    /// </summary>
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == playerTag)
        {
            Game.Instance.PlayerInstance.Controller.IsInShelter = false;
        }
    }
}
