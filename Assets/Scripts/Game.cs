public class Game 
{
    private static Game instance;
	private bool debugMode;

	/// <summary>
	/// Initializes a new instance of the <see cref="Game"/> class.
	/// </summary>
    private Game ()
    {
        PlayerInstance = new Player();
		WeatherInstance = new WeatherSystem();
		PauseInstance = new PauseSystem();
		GameSettingsInstance = new GameSettings ();
		Scheme = GameSettingsInstance.Scheme;
    }
 
    /// <summary>
    /// The current game instance. If there is no instance, one is created.
    /// </summary>
    public static Game Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Game();
            }
            return instance;
        }
    }

    /// <summary>
    /// The current player instance.
    /// </summary>
    public Player PlayerInstance
    {
        get;
        private set;
    }


	/// <summary>
	/// Turns on debug mode.
	/// </summary>
	public bool DebugMode
	{
		get
		{
			return debugMode;
		}
		set
		{
			debugMode = value;
			if (DebugModeSubscription != null)
			{
				DebugModeSubscription();
			}
		}
	}

	public delegate void DebugModeDelegate();
	public event DebugModeDelegate DebugModeSubscription;

	/// <summary>
	/// Gets the weather instance.
	/// </summary>
	/// <value>The weather instance.</value>
	public WeatherSystem WeatherInstance
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets a value indicating whether this instance is radio active.
	/// </summary>
	/// <value><c>true</c> if this instance is radio active; otherwise, <c>false</c>.</value>
    public bool IsRadioActive
    {
        get
        {
            return RadioInstance != null;
        }
    }

    /// <summary>
    /// Gets or sets the radio instance.
    /// </summary>
    /// <value>The radio instance.</value>
    public Radio RadioInstance
    {
        get;
        set;
    }

	private Clock clockInstance;

	/// <summary>
	/// Gets the clock instance.
	/// </summary>
	/// <value>The clock instance.</value>
	public Clock ClockInstance
	{
		get
		{
			if(this.clockInstance == null)
			{
				this.clockInstance = UnityEngine.MonoBehaviour.FindObjectOfType<Clock>();
			}

			return this.clockInstance;
		}
	}

	/// <summary>
	/// Gets or private sets the pause instance.
	/// </summary>
	/// <value>The pause instance.</value>
	public PauseSystem PauseInstance
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the game settings instance.
	/// </summary>
	/// <value>The game settings instance.</value>
	public GameSettings GameSettingsInstance 
	{
		get;
		private set;
	}


	/// <summary>
	/// Gets the control scheme configured for the game.
	/// </summary>
	/// <value>Player control scheme</value>
	public ControlScheme Scheme 
	{
		get;
		set;
	}
}