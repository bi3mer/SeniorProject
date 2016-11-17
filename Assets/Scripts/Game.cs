public class Game 
{
    private static Game instance;

    private Game ()
    {
        PlayerInstance = new Player();
		WeatherInstance = new WeatherSystem();
		RadioInstance = new Radio();
		RadioInstance.currentWeather = WeatherInstance;
		PauseInstance = new PauseSystem();
		GameSettingsInstance = new GameSettings ();
		Scheme = new ControlScheme ();
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
	/// Gets the weather instance.
	/// </summary>
	/// <value>The weather instance.</value>
	public WeatherSystem WeatherInstance
	{
		get;
		private set;
	}

	/// <summary>
	/// The clock instance.
	/// </summary>
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
				this.clockInstance.Start();
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
	/// Gets the radio instance.
	/// </summary>
	/// <value>The radio instance.</value>
	public Radio RadioInstance 
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
