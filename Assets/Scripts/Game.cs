public class Game 
{
    private static Game instance;
    private bool debugMode;

    private Game ()
    {
        PlayerInstance = new Player();
		WeatherInstance = new WeatherSystem();
        Radio RadioInstance = new Radio();
        RadioInstance.currentWeather = WeatherInstance;
		PauseInstance = new PauseSystem();
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
    /// The current city. Returns null if the city has not yet been populated.
    /// </summary>
    public City CityInstance
    {
        get;
        set;
    }

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

	private Clock clockInstance;
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
	public PauseSystem PauseInstance
	{
		get;
		private set;
	}
}
