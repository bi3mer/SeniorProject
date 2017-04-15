using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemAttributeUI : MonoBehaviour 
{
	[Tooltip("Text that displays the attribute name")]
	public Text AttributeName;

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
		attributeSlider.value = value;
		attributeValue.text = attributeSlider.value.ToString();
		fillImage.color = possibleColors.Evaluate(attributeSlider.value / attributeSlider.maxValue);
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
