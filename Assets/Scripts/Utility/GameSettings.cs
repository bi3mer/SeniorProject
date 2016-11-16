public class GameSettings
{
	/// <summary>
	/// Initializes a new instance of the <see cref="GameSettings"/> class.
	/// </summary>
	public GameSettings()
	{
		this.SoundOn = true;
		this.VolumeValue = 3.0f;
		this.ProceduralCityGenerationSeed = 1;
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="AssemblyCSharp.GameSettings"/> sound on.
	/// </summary>
	/// <value><c>true</c> if sound on; otherwise, <c>false</c>.</value>
	public bool SoundOn { get; set; }

	/// <summary>
	/// Gets or sets the volume value.
	/// </summary>
	/// <value>The volume value.</value>
	public float VolumeValue { get; set; }

	/// <summary>
	/// Gets or sets the procedural city generation seed.
	/// </summary>
	/// <value>The procedural city generation seed.</value>
	public int ProceduralCityGenerationSeed { get; set; }
}


