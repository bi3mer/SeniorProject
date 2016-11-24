using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class ItemStackUI : MonoBehaviour 
{
	public UnityAction Action;
	public Text ItemName;
	public Text ItemAmount;

	/// <summary>
	/// Initialize the specified UI item itemName and amount.
	/// </summary>
	/// <param name="itemName">Item name.</param>
	/// <param name="amount">Amount.</param>
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
