using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Class for the Item UI. To be attached to the sliders that show the value of each attribute in an item.
/// </summary>
public class AttributeStat : MonoBehaviour 
{

	// slider attached to this gameobject
	private Slider slider;

	// the label that is paired with the slider
	public Text Label;

	/// <summary>
	/// Awake this instance. Gets the slider component.
	/// </summary>
	void Awake () {
		slider = GetComponent<Slider> ();
	}

	/// <summary>
	/// Changes the text displayed by the stat's label.
	/// </summary>
	/// <param name="labelText">Label text.</param>
	public void SetLabel(string labelText)
	{
		Label.text = labelText;
	}

	/// <summary>
	/// Sets the value of the slider.
	/// </summary>
	/// <param name="val">Value.</param>
	public void SetValue(float val)
	{
		slider.value = val;
	}

}
