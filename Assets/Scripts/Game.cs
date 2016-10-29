public class Game 
{
    private static Game instance;

    private Game ()
    {
        PlayerInstance = new Player();
		ClockInstance  = new Clock();
		ClockInstance.Start();
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

	public Clock ClockInstance
	{
		get;
		private set;
	}
}
