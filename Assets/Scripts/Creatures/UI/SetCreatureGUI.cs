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
	/// Sets the text in the text object if it is not empty
	/// </summary>
	/// <param name="textObject">Text object.</param>
	/// <param name="baseString">Base string.</param>
	/// <param name="spawner">Spawner.</param>
	private void setText(Text textObject, string baseString, CreatureSpawner spawner)
	{
		baseString += " ";

		if(spawner != null)
		{
			baseString += spawner.Count;
		}
		else
		{
			baseString += "not found.";
		}

		textObject.text = baseString;
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update()
	{
		this.setText(this.fishCount,  this.fishCountString,  this.fishSpawner);
		this.setText(this.jellyCount, this.jellyCountString, this.jellySpawner);
		this.setText(this.whaleCount, this.whaleCountString, this.whaleSpawner);
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
