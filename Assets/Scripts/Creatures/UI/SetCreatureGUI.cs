using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetCreatureGUI : MonoBehaviour 
{
	[SerializeField]
	private Text fishCount;
	private string fishCountString;
	private FishSpawner fishSpawner;

	[SerializeField]
	private Text jellyCount;
	private string jellyCountString;
	private JellyFishSpawner jellySpawner;

	[SerializeField]
	private Text whaleCount;
	private string whaleCountString;
	private WhaleSpawner whaleSpawner;

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		this.fishCount.text  = this.fishCountString  + " " + this.fishSpawner.Count;
		this.jellyCount.text = this.jellyCountString + " " + this.jellySpawner.Count;
		this.whaleCount.text = this.whaleCountString + " " + this.whaleSpawner.Count;
	}

	/// <summary>
	/// Initialize text fields
	/// </summary>
	void Start()
	{
		// set base text fields
		this.fishCountString  = this.fishCount.text;
		this.jellyCountString = this.jellyCount.text;
		this.whaleCountString = this.whaleCount.text;

		// find spawners
		this.fishSpawner      = GameObject.FindObjectOfType<FishSpawner>();
		this.jellySpawner     = GameObject.FindObjectOfType<JellyFishSpawner>();
		this.whaleSpawner     = GameObject.FindObjectOfType<WhaleSpawner>();
	}
}
