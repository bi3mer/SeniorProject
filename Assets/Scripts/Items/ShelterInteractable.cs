using UnityEngine;
using System.Collections;

public class ShelterInteractable : InteractableObject 
{
    private const string enterShelterPrompt = "Enter Shelter";
    private const string exitShelterPrompt = "Exit Shelter";

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
        Text = enterShelterPrompt;

        SetAction
        (
            delegate
            {
                EnterShelter();
            }
        );
    }

    /// <summary>
    /// Enter the shelter.
    /// </summary>
    public void EnterShelter()
    {
        Game.Instance.PlayerInstance.Controller.IsInShelter = true;

        Text = exitShelterPrompt;
        SetAction
        (
            delegate
            {
                ExitShelter();
            }
        );

        // TO DO: have way for character to enter shelter
    }

    /// <summary>
    /// Exit the shelter.
    /// </summary>
    public void ExitShelter()
    {
        Game.Instance.PlayerInstance.Controller.IsInShelter = false;

        Text = exitShelterPrompt;
        SetAction
        (
            delegate
            {
                EnterShelter();
            }
        );

        // TO DO: have way for character to visually enter shelter
    }

    /// <summary>
    /// Raises the trigger enter event.
    /// </summary>
    /// <param name="coll">Collider entered</param>
    public void OnTriggerEnter(Collider coll)
	{
		if (Game.Instance.PlayerInstance.Controller == coll.gameObject.GetComponent<PlayerController>()) 
		{
            Show = true;
		}
	}

	/// <summary>
	/// Raises the trigger exit event.
	/// </summary>
	/// <param name="coll">Collider exited</param>
	public void OnTriggerExit(Collider coll)
	{
		if (Game.Instance.PlayerInstance.Controller == coll.gameObject.GetComponent<PlayerController>())
		{
            Show = false;
		}
	}
}
