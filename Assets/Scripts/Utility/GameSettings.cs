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
		this.Scheme = new ControlScheme ();
		setInitialControlScheme ();
	}

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="AssemblyCSharp.GameSettings"/> sound on.
	/// </summary>
	/// <value><c>true</c> if sound on; otherwise, <c>false</c>.</value>
	public bool SoundOn 
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the volume value.
	/// </summary>
	/// <value>The volume value.</value>
	public float VolumeValue 
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the procedural city generation seed.
	/// </summary>
	/// <value>The procedural city generation seed.</value>
	public int ProceduralCityGenerationSeed 
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Gets or sets the scheme.
	/// </summary>
	/// <value>The scheme.</value>
	public ControlScheme Scheme 
	{ 
		get; 
		set; 
	}

	/// <summary>
	/// Sets the initial control scheme.
	/// </summary>
	private void setInitialControlScheme()
	{
		Scheme.Forward = UnityEngine.KeyCode.W;
		Scheme.Back = UnityEngine.KeyCode.S;
		Scheme.Right = UnityEngine.KeyCode.D;
		Scheme.Left = UnityEngine.KeyCode.A;
		Scheme.ForwardSecondary = UnityEngine.KeyCode.UpArrow;
		Scheme.BackSecondary = UnityEngine.KeyCode.DownArrow;
		Scheme.RightSecondary = UnityEngine.KeyCode.RightArrow;
		Scheme.LeftSecondary = UnityEngine.KeyCode.LeftArrow;
		Scheme.Sprint = UnityEngine.KeyCode.LeftShift;
		Scheme.Jump = UnityEngine.KeyCode.Space;
		Scheme.Action = UnityEngine.KeyCode.F;
		Scheme.UseTool = UnityEngine.KeyCode.Mouse0;
		Scheme.CameraRight = UnityEngine.KeyCode.E;
		Scheme.CameraLeft = UnityEngine.KeyCode.Q;
		Scheme.CameraZoomAxis = "Mouse ScrollWheel";
		Scheme.Pause = UnityEngine.KeyCode.Escape;
		Scheme.Inventory = UnityEngine.KeyCode.Tab;
		Scheme.Radio = UnityEngine.KeyCode.R;
		Scheme.Crafting = UnityEngine.KeyCode.C;
	}
}


