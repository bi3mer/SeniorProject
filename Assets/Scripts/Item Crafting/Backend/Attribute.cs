using UnityEngine;
using System.Collections;

public class Attribute
{
	public Attribute(string name, float value)
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
	public Attribute GetDuplicate()
	{
		return new Attribute(Name, Value);
	}
}
