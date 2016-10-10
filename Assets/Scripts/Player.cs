public class Player {
    public Player ()
    {
        Health = MaxHealth = 100;
        Warmth = MaxWarmth = 100;
        Hunger = MaxHunger = 100;
    }

    /// <summary>
    /// The player's current health. 
    /// Used for physical dammage.
    /// </summary>
    public float Health
    {
        get;
        set;
    }

    /// <summary>
    /// The player's maximum health. 
    /// Used for physical dammmage.
    /// </summary>
    public int MaxHealth
    {
        get;
        private set;
    }

    /// <summary>
    /// The player's current warmth. 
    /// Warmth is reduced when the player is exposed to cold.
    /// </summary>
    public float Warmth
    {
        get;
        set;
    }

    /// <summary>
    /// The player's maximum warmth. 
    /// Warmth is reduced when the player is exposed to cold.
    /// </summary>
    public int MaxWarmth
    {
        get;
        private set;
    }

    /// <summary>
    /// The player's current hunger.
    /// Hunger is reduced when the player does not eat.
    /// </summary>
    public float Hunger
    {
        get;
        set;
    }

    /// <summary>
    /// The player's maximum hunger.
    /// Hunger is reduced when the player does not eat.
    /// </summary>
    public int MaxHunger
    {
        get;
        private set;
    }
}
