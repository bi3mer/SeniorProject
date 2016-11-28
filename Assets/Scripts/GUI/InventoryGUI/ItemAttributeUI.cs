using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemAttributeUI : MonoBehaviour 
{
	public Text AttributeName;
	public Slider AttributeValue;

	/// <summary>
	/// Sets up attribute UI.
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="value">Value.</param>
	public void SetUpAttributeUI(string name, float value)
	{
		AttributeName.text = name;
		AttributeValue.value = value;
	}

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
		AttributeValue.value = value;
	}
}
