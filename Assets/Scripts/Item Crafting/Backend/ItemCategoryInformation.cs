using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemCategoryInformation
{
	string categoryName;
	Dictionary<string, float> attributes;

	public ItemCategoryInformation(string name)
	{
		categoryName = name;
	}

	/// <summary>
	/// Adds an attribute that belongs in this category.
	/// </summary>
	/// <param name="attrNAme">Attr N ame.</param>
	/// <param name="attrValue">Attr value.</param>
	public void AddAttribute(string attrNAme, float attrValue)
	{
		attributes.Add (attrNAme, attrValue);
	}

	public Dictionary<string, float> Attributes
	{
		get
		{
			return attributes;
		}
		set
		{
			attributes = value;
		}
	}
}
