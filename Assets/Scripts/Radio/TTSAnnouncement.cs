using System;

public class TTSAnnouncement : MysteryAnnouncement
{
	/// <summary>
	/// Gets or sets the text that the TTS will say.
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

