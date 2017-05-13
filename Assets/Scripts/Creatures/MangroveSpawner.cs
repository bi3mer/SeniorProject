using UnityEngine;

public class MangroveSpawner : CreatureSpawner
{
    private const float aboveModifier = 50f;

    [SerializeField]
    private LayerMask collisionMask;

    /// <summary>
    /// Get a random location on the map around the player in the configured 
    /// radius and height
    /// </summary>
    /// <returns></returns>
    private Vector3 randomLocation()
    {
    	Vector3 position = Game.Player.WorldPosition;
        return new Vector3(Random.Range(position.x - this.floatInRadius, position.x + this.floatInRadius),
                           Game.Instance.WaterLevelHeight + this.waterLevelOffset,
			               Random.Range(position.z - this.floatInRadius, position.z + this.floatInRadius));
    }

    /// <summary>
    /// Finds a spawn location that isn't in a building
    /// </summary>
    /// <returns></returns>
    protected override Vector3 findSpawnLocation()
    {
        Vector3 location = this.randomLocation();
        Vector3 above;

        do
        {
            location = this.randomLocation();
            above    = new Vector3(location.x, location.y + aboveModifier, location.y);
        }
		while (!Physics.Raycast(above, Vector3.down, aboveModifier, this.collisionMask));

        return location;
    }
}
