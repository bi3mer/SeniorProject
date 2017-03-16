using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemAttributeUI : MonoBehaviour 
{
	[Tooltip("Text that displays the attribute name")]
	public Text AttributeName;

	/// <summary>
	/// This may be set by attributes directly, so public.
	/// </summary>
	[Tooltip("Drives what the maximum on the item attribute is. The negative value of this will be used for the minimum once the value falls below 0.")]
	public int RangeValue;

	[Tooltip("Text that displays the value of the attribute")]
	[SerializeField]
	private Text attributeValue;

	[Tooltip("Panel that contains the value of the attribute. Should only display on hover")]
	[SerializeField]
	private GameObject numberHoverPanel;

	[Tooltip("Slider that graphically displays the value of the attribute")]
	[SerializeField]
	private Slider attributeSlider;

	[Tooltip("The fill image used by the slider")]
	[SerializeField]
	private Image fillImage;

	[Tooltip("The gradient of colors that the fill image will be, depending on the attribute's value")]
	[SerializeField]
	private Gradient possibleColors;

	/// <summary>
	/// Sets the name of the attribute.
	/// </summary>
	/// <param name="name">Name.</param>
	public void SetAttributeName(string name)
	{
		AttributeName.text = name;
	}

	/// <summary>
	/// Sets the attribute value.
	/// </summary>
	/// <param name="value">Value.</param>
	public void SetAttributeValue(float value)
	{
		if(value >= 0)
		{
			attributeSlider.maxValue = Mathf.Max(value, RangeValue);
			attributeSlider.minValue = 0;

			// the color of the stat should be in the upper half of the possible colors gradient if the stat is positive
			// so the minimum value is 0.5f, and the percentage to max needs halved
			fillImage.color = possibleColors.Evaluate((value/ (2f * attributeSlider.maxValue)) + 0.5f);
		}
		else
		{
			attributeSlider.minValue = Mathf.Min(value, -RangeValue);
			attributeSlider.maxValue = 0;

			// the color of the stat should be in the lower half of the possible colors gradient if the stat is positive
			// so the minimum value is 0f and the , so the percentage to max needs to be halved
			fillImage.color = possibleColors.Evaluate((value / (2f * Mathf.Abs(attributeSlider.minValue))));
		}

		attributeSlider.value = value;
		attributeValue.text = attributeSlider.value.ToString();
	}

	/// <summary>
	/// Shows the hover panel with the slider value.
	/// </summary>
	public void ShowHover()
	{
		numberHoverPanel.SetActive(true);
	}

	/// <summary>
	/// Hides the hover panel with the slider value.
	/// </summary>
	public void HideHover()
	{
		numberHoverPanel.SetActive(false);
	}
}
