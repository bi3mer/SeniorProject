using UnityEngine;

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
	/// Gets a value indicating whether this instance is paused.
	/// </summary>
	/// <value><c>true</c> if this instance is paused; otherwise, <c>false</c>.</value>
	public bool IsPaused 
	{
		get 
		{
			return this.IsPaused;
		}
		private set 
		{
			if (IsPaused.Equals (true)) 
			{
				Time.timeScale = 0.0f;
			} 
			else 
			{
				Time.timeScale = 1.0f;
			}
		}
	}
}
