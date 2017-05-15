public class Game 
{
    private static Game instance;
	private bool debugMode;

    private Game ()
    {
		this.CityBounds = new CityBoundaries();
	    PlayerInstance = new Player();
		EventManager = new EventManager ();
		PauseInstance = new PauseSystem();
		WorldItemFactoryInstance = new WorldItemFactory();
		ItemFactoryInstance = new ItemFactory();
		NoteFactoryInstance = new NoteFactory();
		GameSettingsInstance = new GameSettings ();
		Scheme = GameSettingsInstance.Scheme;
        DeathManagerInstance = new DeathManager();
		AnnouncementFactoryInstance = new AnnouncementFactory ();
		WeatherInstance = new WeatherSystem(this.CityBounds, PauseInstance);

        Loader = new GameLoader();
        Loader.GameLoadedEvent += () => {
            PauseInstance.Resume();
        };

        PauseInstance.Pause();
    }

    /// <summary>
    /// Resets the game from the start.
    /// </summary>
    public void Reset ()
    {
		// stop all sounds so they can be started again
		FMODUnity.RuntimeManager.GetBus("bus:/").stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);

        PlayerInstance.ResetStatus();
        PlayerInstance.Inventory.Reset();

        Loader.Reset();

        this.PauseInstance.ResetDelegates();
		EventManager = new EventManager ();
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
    /// Shorcut to calling Instance.PlayerInstance
    /// </summary>
    public static Player Player
    {
        get
        {
            return Instance.PlayerInstance;
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
    /// Gets the city bounds.
    /// </summary>
    /// <value>The city bounds.</value>
    public CityBoundaries CityBounds
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
	/// Gets the event manager instance.
	/// </summary>
	/// <value>The event manager instance.</value>
	public EventManager EventManager
	{
		get;
		private set;
	}

    public bool IsRadioActive
    {
        get
        {
            return RadioInstance != null;
        }
    }

    public Radio RadioInstance
    {
        get;
        set;
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

				// check if clock instance was found
				if(this.clockInstance == null)
				{
					UnityEngine.Debug.LogError("No clock instance found in scene.");
					
					// throw error if not found
					throw new System.NullReferenceException();
				}
			}

			return this.clockInstance;
		}
	}

	/// <summary>
	/// The flood water instance.
	/// </summary>
	private FloodWater floodWaterInstance;

	/// <summary>
	/// Gets the height of the water level.
	/// </summary>
	/// <value>The height of the water level.</value>
	public float WaterLevelHeight
	{
		get
		{
			if(this.floodWaterInstance == null)
			{
				this.floodWaterInstance = UnityEngine.MonoBehaviour.FindObjectOfType<FloodWater>();

				// check if object was found in scene 
				if(this.floodWaterInstance == null)
				{
					UnityEngine.Debug.LogError("No flood water instance found in scene.");

					// throw error
					throw new System.NullReferenceException();
				}
			}

			return this.floodWaterInstance.WaterLevelHeight;
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
	/// Gets the item factory instance which stores all data about items.
	/// </summary>
	/// <value>The item factory instance.</value>
	public ItemFactory ItemFactoryInstance
	{
		get;
		private set;
	}

	/// Gets the game settings instance.
	/// </summary>
	/// <value>The game settings instance.</value>
	public GameSettings GameSettingsInstance 
	{
		get;
		private set;
	}

	/// <summary>
	/// Gets the item factory instance which creates items in the world.
	/// </summary>
	/// <value>The world item factory instance.</value>
	public WorldItemFactory WorldItemFactoryInstance
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

	/// <summary>
	/// Gets or sets the game view instance.
	/// </summary>
	/// <value>The game view instance.</value>
    public GameViewBehavior GameViewInstance
    {
    	get;
    	set;
    }

    /// <summary>
    /// Tracks and reports game loading.
    /// </summary>
    public GameLoader Loader
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the death manager instance.
    /// </summary>
    /// <value>The death manager instance.</value>
    public DeathManager DeathManagerInstance
    {
    	get;
    	private set;
    }

	/// <summary>
	/// Gets or sets the item pool instance.
	/// </summary>
	/// <value>The item pool instance.</value>
    public ItemPoolManager ItemPoolInstance 
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the announcement factory instance.
	/// </summary>
	/// <value>The announcement factory instance.</value>
	public AnnouncementFactory AnnouncementFactoryInstance 
	{
		get;
		set;
	}

	/// <summary>
	/// Gets or sets the note factory instance.
	/// </summary>
	/// <value>The note factory instance.</value>
	public NoteFactory NoteFactoryInstance 
	{
		get;
		private set;
	}
}
