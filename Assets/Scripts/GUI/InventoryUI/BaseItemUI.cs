using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class BaseItemUI : MonoBehaviour 
{
	public UnityAction Action;
	public Text ItemName;
	public Text ItemAmount;

	public void Initialize(string itemName, int amount)
	{
		if (itemName.Length != 0) 
		{
			ItemName.text = itemName;
		}

		if (amount != null) 
		{
			ItemAmount.text = amount.ToString ();
		}
	}
}
