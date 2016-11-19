public class Game 
{
    private static Game instance;

    private Game ()
    {
        PlayerInstance = new Player();
		WeatherInstance = new WeatherSystem();
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
	/// Gets the weather instance.
	/// </summary>
	/// <value>The weather instance.</value>
	public WeatherSystem WeatherInstance
	{
		get;
		private set;
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
	/// Gets the pause instance.
	/// </summary>
	/// <value>The pause instance.</value>
	public PauseSystem PauseInstance
	{
		get;
		private set;
	}
}
