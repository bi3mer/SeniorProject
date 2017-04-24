using UnityEngine;
using System.Collections;

public class ItemAttribute
{
	public ItemAttribute(string name, float value)
	{
		Name = name;
		Value = value;
	}

	public string Name 
	{
		get;
		set;
	}

	public float Value 
	{
		get;
		set;
	}

	/// <summary>
	/// Gets the duplicate of the attribute
	/// </summary>
	/// <returns>The duplicate.</returns>
	public ItemAttribute GetDuplicate()
	{
		return new ItemAttribute(Name, Value);
	}
}
