using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// Base class for all notes.
/// </summary>
public class Note 
{
    /// <summary>
	/// Gets or sets the type of the note
	/// </summary>
	/// <value>The type of the note</value>
	public string Type 
	{
		get; 
		set;
	}

    /// <summary>
	/// Gets or sets the title of the note
	/// </summary>
	/// <value>The title of the note</value>
	public string Title 
	{
		get; 
		set;
	}

	/// <summary>
	/// Gets or sets the body text of the note
	/// </summary>
	/// <value>The body text of the note</value>
	public string Text 
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