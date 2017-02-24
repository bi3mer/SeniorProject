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
	/// Gets or sets the FMOD event path for this clip.
	/// If it's pre-generated, this comes in the YAML. Otherwise, this is created by TTSAnnouncementGenerator.
	/// </summary>
	/// <value>The event path.</value>
	public String EventPath
	{
		get;
		set;
	}
}

