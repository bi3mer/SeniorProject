using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathFlavorText : MonoBehaviour 
{
	public Text FlavorText;

	[SerializeField]
    [Tooltip("The file where flavor texts are stored.")]
    private string flavorTextFilePath;

	private LoadingScreenFlavorTextManager flavorTextManager;

	// Use this for initialization
	void Start () 
	{
		if (FlavorText != null)
        {
            flavorTextManager = new LoadingScreenFlavorTextManager(flavorTextFilePath);
            FlavorText.text = flavorTextManager.GetRandom().Text;
        }
	}
}
