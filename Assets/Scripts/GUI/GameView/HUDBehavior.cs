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

	[SerializeField]
	private int healthSliderID;

	[SerializeField]
	private int warmthSliderID;

	[SerializeField]
	private int hungerSliderID;

	[SerializeField]
	private float disabledIconOpacity;

	[SerializeField]
	[Tooltip("The sprites on each hud used to create the ripples")]
	private Image[] rippleSprites;

	[SerializeField]
	[Tooltip("The icons for each slider")]
	private Image[] hudIconsSprites;

	[SerializeField]
	[Tooltip("Colors of the line. Provide in same order as line renderer.")]
	private Color[] lineColors;

	[SerializeField]
	private float pulseTime;

	[SerializeField]
	[Tooltip("Opacity of ripple through time")]
	private AnimationCurve opacity;

	[SerializeField]
	[Tooltip("Ripple increased size")]
	private AnimationCurve rippleSize;

	[SerializeField]
	[Tooltip("Rate at which time of rippling decreases")]
	private float rippleTimeDecreaseRate;

	[SerializeField]
	[Tooltip("Amount of units before rippleIncreaseRate is applied")]
	private int rippleIncreaseUnits;

	[SerializeField]
	[Tooltip("Units of stat before ripples occur to warn player")]
	private int rippleThreshold;

	[SerializeField]
	[Tooltip("Opacity of icon when it is 0")]
	private float iconZeroOpacity;

	private int vertexCount;

	private float[] rippleState;

	private RectTransform[] rippleTransforms;

	/// <summary>
	/// Set the HUD values to the max value.
	/// </summary>
	void Start () 
	{
		player = Game.Instance.PlayerInstance;
		healthSlider.value = healthSlider.maxValue = player.MaxHealth;
		warmthSlider.value = warmthSlider.maxValue = player.MaxWarmth;
		hungerSlider.value = hungerSlider.maxValue = player.MaxHunger;

		rippleState = new float[rippleSprites.Length];
		rippleTransforms = new RectTransform[rippleSprites.Length];

		for(int i = 0; i < rippleSprites.Length; ++i)
		{
			rippleState[i] = 0;
			rippleTransforms[i] = rippleSprites[i].GetComponent<RectTransform>();
			rippleSprites[i].gameObject.SetActive(false);
		}
	}

	/// <summary>
	/// Updates the health slider.
	/// </summary>
	public void UpdateHealthSlider()
	{
		setHudIconColors(healthSliderID, healthSlider.value, player.Health);

		healthSlider.value = player.Health;

		if(healthSlider.value < rippleThreshold && healthSlider.value > 0 && rippleState[healthSliderID] == 0)
		{
			StartCoroutine(pulse(healthSliderID, healthSlider.value));
		}
	}

	/// <summary>
	/// Updates the warmth slider.
	/// </summary>
	public void UpdateWarmthSlider()
	{
		setHudIconColors(warmthSliderID, warmthSlider.value, player.Warmth);

		warmthSlider.value = player.Warmth;

		if(warmthSlider.value < rippleThreshold && warmthSlider.value > 0 && rippleState[warmthSliderID] == 0)
		{
			StartCoroutine(pulse(warmthSliderID, warmthSlider.value));
		}
	}

	/// <summary>
	/// Updates the hunger slider.
	/// </summary>
	public void UpdateHungerSlider()
	{
		setHudIconColors(hungerSliderID, hungerSlider.value, player.Hunger);

		hungerSlider.value = player.Hunger;

		if(hungerSlider.value < rippleThreshold && hungerSlider.value > 0 && rippleState[hungerSliderID] == 0)
		{
			StartCoroutine (pulse(hungerSliderID, hungerSlider.value));
		}
	}

	/// <summary>
	/// Creates a pulse around the specified slider. Pulse speed is based on the currentValue.
	/// </summary>
	/// <param name="slider">Slider id.</param>
	/// <param name="currentValue">Current value of the slider. The lower the shorter the pulse time.</param>
	private IEnumerator pulse(int slider, float currentValue)
	{
		float currentTime = 0f;
		float totalTime = pulseTime * Mathf.Pow(rippleTimeDecreaseRate, (Mathf.FloorToInt((rippleThreshold - currentValue) / rippleIncreaseUnits)));
		rippleSprites[slider].gameObject.SetActive(true);
		rippleState[slider] = 1;

		while (currentTime < totalTime)
		{
			setRippleSize(slider, rippleSize.Evaluate(currentTime/totalTime));
			setRippleColors(slider, opacity.Evaluate(currentTime/totalTime));
			currentTime += Time.deltaTime;
			yield return null;
		}

		rippleState[slider] = 0;
		rippleSprites[slider].gameObject.SetActive(false);
	}

	/// <summary>
	/// Sets the size of the ripple.
	/// </summary>
	/// <param name="slider">Slider id.</param>
	/// <param name="radialScale">Radial scale.</param>
	private void setRippleSize(int slider, float radialScale)
	{
		rippleTransforms[slider].localScale = Vector3.one * radialScale;
	}

	/// <summary>
	/// Sets the ripple colors.
	/// </summary>
	/// <param name="slider">Slider id.</param>
	/// <param name="opacity">Opacity.</param>
	private void setRippleColors(int slider, float opacity)
	{
		lineColors[slider].a = opacity;
		rippleSprites[slider].color = lineColors[slider];
	}

	/// <summary>
	/// Fades out the icon when stat is at 0. Brings it back when stat goes back to being above zero.
	/// </summary>
	/// <param name="slider">Slider.</param>
	/// <param name="sliderValue">Slider value.</param>
	/// <param name="newValue">New value.</param>
	private void setHudIconColors(int slider, float sliderValue, float newValue)
	{
		Color targetColor = hudIconsSprites[slider].color;

		if(newValue > 0 && sliderValue == 0)
		{
			// return to full opacity if fade before
			targetColor.a = 1;
			hudIconsSprites[slider].color = targetColor;
		}
		else if(newValue == 0 && sliderValue > 0)
		{
			targetColor.a = iconZeroOpacity;
			hudIconsSprites[slider].color = targetColor;
		}
	}
}
