using UnityEngine;
using System.Collections;

public class Attribute
{
	private string attributeName;
	private float attributeValue;

	public Attribute(string name, float value)
	{
		attributeName = name;
		attributeValue = value;
	}

	public string Name 
	{
		get 
		{
			return attributeName;
		}
		set 
		{
			attributeName = value;
		}
	}

	public float Value 
	{
		get 
		{
			return attributeValue;
		}
		set 
		{
			attributeValue = value;
		}
	}
}
