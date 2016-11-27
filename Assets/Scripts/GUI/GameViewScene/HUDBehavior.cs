using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDBehavior : MonoBehaviour 
{
	[SerializeField]
	private Slider healthSlider;
	[SerializeField]
	private Slider warmthSlider;
	[SerializeField]
	private Slider hungerSlider;
	private Player player;

	/// <summary>
	/// Set the HUD values to the max value.
	/// </summary>
	void Start () 
	{
		player = Game.Instance.PlayerInstance;
		healthSlider.value = healthSlider.maxValue = player.MaxHealth;
		warmthSlider.value = warmthSlider.maxValue = player.MaxWarmth;
		hungerSlider.value = hungerSlider.maxValue = player.MaxHunger;
	}

	/// <summary>
	/// Updates the health slider.
	/// </summary>
	public void UpdateHealthSlider()
	{
		healthSlider.value = player.Health;
	}

	/// <summary>
	/// Updates the warmth slider.
	/// </summary>
	public void UpdateWarmthSlider()
	{
		warmthSlider.value = player.Warmth;
	}

	/// <summary>
	/// Updates the hunger slider.
	/// </summary>
	public void UpdateHungerSlider()
	{
		hungerSlider.value = player.Hunger;
	}
}
