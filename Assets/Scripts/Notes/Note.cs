using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// Base class for all notes, allowing for us to generate multiple notes of the same model.
/// </summary>
public class Note 
{
    /// <summary>
	/// Gets or sets the title of the note
	/// </summary>
	/// <value>The title of the note</value>
	public string Type 
	{
		get; 
		set;
	}

    /// <summary>
	/// Gets or sets the model that represents the item in the world.
	/// </summary>
	/// <value>The world model.</value>
    public string WorldModel
    {
    	get;
    	set;
    }


}