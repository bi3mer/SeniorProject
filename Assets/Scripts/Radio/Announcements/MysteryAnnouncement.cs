using System;

/// <summary>
/// Mystery announcement base class
/// </summary>
public class MysteryAnnouncement
{
	/// <summary>
	/// Gets or sets the category.
	/// </summary>
	/// <value>The category.</value>
	public AnnouncementCategory Category
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the event path for this clip.
	/// If it's pre-generated, this comes in the YAML.
	/// </summary>
	/// <value>The event path.</value>
	public String EventPath
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the text that the TTS bot will say.
	/// </summary>
	/// <value>The text.</value>
	public string Text 
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the voice that the announcement will be in.
	/// </summary>
	/// <value>The voice.</value>
	public string Voice 
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the speed of the TTS playback.
	/// </summary>
	/// <value>The speed.</value>
	public string Speed 
	{
		get;
		set;
	}
}

