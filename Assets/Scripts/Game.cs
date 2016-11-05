public class Game 
{
    private static Game instance;

    private Game ()
    {
        PlayerInstance = new Player();
		WeatherInstance = new WeatherSystem();
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
}
