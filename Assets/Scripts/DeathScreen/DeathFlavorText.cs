using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathFlavorText : MonoBehaviour 
{
	[SerializeField]
    [Tooltip("The text object that will be modified to have the flavor text.")]
	public Text flavorText;

	[SerializeField]
    [Tooltip("The file where flavor texts are stored.")]
    private string flavorTextFilePath;

	private LoadingScreenFlavorTextManager flavorTextManager;

	/// <summary>
	/// Starts this instance.
	/// </summary>
	void Start () 
	{
		if (flavorText != null)
        {
            flavorTextManager = new LoadingScreenFlavorTextManager(flavorTextFilePath);
            flavorText.text = flavorTextManager.GetRandom().Text;
        }
	}
}
