using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The loading screen object. Closes when finished loading.")]
    private GameObject loadingScreen;

    [SerializeField]
    [Tooltip("The progress bar to show game loading progress.")]
    private Slider progressBar;

    [SerializeField]
    [Tooltip("The text to show current game loading percentage.")]
    private Text progressText;

    [SerializeField]
    [Tooltip("Ease of interpolating progress bar value.")]
    [Range(0, 1)]
    private float progressBarEase;

    [SerializeField]
    [Tooltip("The flavor text to be updated periodically.")]
    private Text flavorText;

    [SerializeField]
    [Tooltip("The file where flavor texts are stored.")]
    private string flavorTextFilePath;

    [SerializeField]
    [Tooltip("Amount of time before the flavor text advances.")]
    private float flavorTextDisplayTime;

    private LoadingScreenFlavorTextManager flavorTextManager;
    private float flavorTextTimer;

    private float testTimeout;
    private GameLoaderTask testTask;

    /// <summary>
    /// Open loading screen and set initial values.
    /// </summary>
	void Start ()
    {
        // TEST VALUES
        testTask = Game.Instance.Loader.CreateGameLoaderTask("Test");
        testTimeout = 0f;
        // ENDTEST

        loadingScreen.SetActive(true);

        if (progressBar != null)
        {
            progressBar.value = 0f;
        }
        
        if (progressText != null)
        {
            progressText.text = GetPercentageString(0f);
        } 

        if (flavorText != null)
        {
            flavorTextManager = new LoadingScreenFlavorTextManager(flavorTextFilePath);
            flavorText.text = flavorTextManager.GetRandom().Text;
            flavorTextTimer = 0f;
        }
	}
	
    /// <summary>
    /// Update loading screen according to percentage loaded.
    /// </summary>
	void Update ()
    {
        // TEST VALUES WILL BE REMOVED
        if (testTimeout <= 0f)
        {
            testTask.PercentageComplete += UnityEngine.Random.Range(0.05f, 0.3f);
            testTimeout = UnityEngine.Random.Range(0.5f, 2f);
        }

        testTimeout -= Time.deltaTime;
        // END TEST

	    if (progressText != null)
        {
            progressText.text = GetPercentageString(Game.Instance.Loader.PercentageComplete);
        }

        if (progressBar != null)
        {
            progressBar.value = Mathf.Lerp(progressBar.value, Game.Instance.Loader.PercentageComplete, progressBarEase);

            // Close screen if we've loaded completely.
            // If we're lerping we won't get 100% of the way there.
            if (progressBar.value >= 0.99f)
            {
                loadingScreen.SetActive(false);
            }
        }

        if (flavorText != null)
        {
            flavorTextTimer += Time.deltaTime;
            if (flavorTextTimer >= flavorTextDisplayTime)
            {
                flavorTextTimer = 0f;
                flavorText.text = flavorTextManager.GetRandom().Text;
            }
        }
	}

    /// <summary>
    /// Returns a formatted percentage string.
    /// </summary>
    /// <param name="percentage">The value.</param>
    /// <returns>The formatted string.</returns>
    private string GetPercentageString (float percentage)
    {
        return String.Format("{0:P0}", percentage);
    }
}
