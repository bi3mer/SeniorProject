using UnityEngine;
using System.Collections;

public class ShelterInteractable : InteractableObject 
{
    private const string enterShelterPrompt = "Enter Shelter";
    private const string exitShelterPrompt = "Exit Shelter";

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
        Game.Instance.PlayerInstance.Controller.SetShelterWarmthRate(Shelter.WarmthRate);
        Game.Instance.PlayerInstance.Controller.IsInShelter = true;

        Text = exitShelterPrompt;
        SetAction
        (
            delegate
            {
                ExitShelter();
            }
        );

        // TODO: have way for character to enter shelter
    }

    /// <summary>
    /// Exit the shelter.
    /// </summary>
    public void ExitShelter()
    {
        Game.Instance.PlayerInstance.Controller.IsInShelter = false;

        Text = enterShelterPrompt;
        SetAction
        (
            delegate
            {
                EnterShelter();
            }
        );

        // TODO: have way for character to visually enter shelter
    }
}
