using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDBehavior : MonoBehaviour {

	public Slider healthSlider;
	public Slider warmthSlider;
	public Slider hungerSlider;
	private Player player;

	/// <summary>
	/// Set the HUD values to the max value.
	/// </summary>
	void Start () 
	{
		player = Game.Instance.PlayerInstance;
		healthSlider.maxValue = player.MaxHealth;
		warmthSlider.maxValue = player.MaxWarmth;
		hungerSlider.maxValue = player.MaxHunger;
	}
	
	/// <summary>
	/// Update slider values.
	/// </summary>
	void Update () 
	{
		healthSlider.value = player.Health;
		warmthSlider.value = player.Warmth;
		hungerSlider.value = player.Hunger;
	}
}
