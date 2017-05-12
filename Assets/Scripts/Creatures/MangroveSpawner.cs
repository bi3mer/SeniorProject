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
        return new Vector3(Game.Instance.PlayerInstance.WorldPosition.x - this.floatInRadius,
                           Game.Instance.WaterLevelHeight + this.waterLevelOffset,
                           Game.Instance.PlayerInstance.WorldPosition.z - this.floatInRadius);
    }

    /// <summary>
    /// Finds a spawn location that isn't in a building
    /// </summary>
    /// <returns></returns>
    protected override Vector3 findSpawnLocation()
    {
        Vector3 location;
        bool validPositionFound = false;

        do
        {
            location = this.randomLocation();
            Vector3 above = new Vector3(location.x, location.y + aboveModifier, location.y);

            if (Physics.Raycast(above, Vector3.down, aboveModifier, this.collisionMask) == false)
            {
                validPositionFound = true;
            }
        }
        while (validPositionFound == false);

        return location;
    }
}
