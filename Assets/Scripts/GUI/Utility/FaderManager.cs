using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

/// <summary>
/// Can be called to fade the screen in or out.
/// 
/// TODO: Let it do what I said above. Right now it's just for fading the game out at the end!
/// </summary>
public class FaderManager : MonoBehaviour
{
    [Tooltip("The white panel that fades in and out")]
    [SerializeField]
    private Image faderImage;

    [Tooltip("The logo or central image in the fade if we so choose to have one.")]
    [SerializeField]
    private Image logoImage;

    [Tooltip("The button that takes us back to the main menu")]
    [SerializeField]
    private GameObject MainMenuButton;

    private const int mainMenuSceneNumber = 0;

    /// <summary>
    /// Fade in the ending stuff
    /// </summary>
    /// <param name="fadeTime">Time the fade takes</param>
    public void EndGameFade(float fadeTime)
    {
        //An alpha value of 1 is fully visable. It represents an color a value of 255 in the A channel. 
        DOTween.ToAlpha(() => faderImage.color, x => faderImage.color = x, 1, fadeTime);
        DOTween.ToAlpha(() => logoImage.color, y => logoImage.color = y, 1, fadeTime);
    }

    /// <summary>
    /// Enable the main menu button
    /// </summary>
    public void ShowMainMenuButton()
    {
        MainMenuButton.SetActive(true);
    }

    /// <summary>
    /// Take me to the main menu
    /// </summary>
    public void GoToMain()
    {
        SceneManager.LoadScene(mainMenuSceneNumber);
    }
}
