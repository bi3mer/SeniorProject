using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemAttributeUI : MonoBehaviour 
{
	[Tooltipe("Text that displays the attribute name")]
	public Text AttributeName;

	[Tooltip("Text that displays the value of the attribute")]
	public Text AttributeValue;

	[Tooltip("Panel that contains the value of the attribute. Should only display on hover")]
	public GameObject NumberHoverPanel;

	[Tooltip("Slider that graphically displays the value of the attribute")]
	public Slider AttributeSlider;

	[Tooltip("The fill image used by the slider")]
	public Image FillImage;

	[Tooltip("The gradient of colors that the fill image will be, depending on the attribute's value")]
	public Gradient PossibleColors;

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
		AttributeSlider.value = value;
		AttributeValue.text = AttributeSlider.value.ToString();
		FillImage.color = PossibleColors.Evaluate(AttributeSlider.value / AttributeSlider.maxValue);
	}

	public void ShowHover()
	{
		NumberHoverPanel.SetActive(true);
	}

	public void HideHover()
	{
		NumberHoverPanel.SetActive(false);
	}
}
