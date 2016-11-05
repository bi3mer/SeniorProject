public class Game 
{
    private static Game instance;

    private Game ()
    {
        PlayerInstance = new Player();
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
}
