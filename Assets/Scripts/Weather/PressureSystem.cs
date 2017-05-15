using UnityEngine;

public class PressureSystem 
{
	public Vector2 Position;
	public float Pressure; 
	public bool IsHighPressure;

    // water tag
    private const string waterTag = "Water";

    /// <summary>
    /// Updates the pressure systems pressure based on whether it is over water
    /// or land. 
    /// </summary>
    public void UpdatePressure()
    {
        const float distance = 50f;

        // subtract 2 so we can go just below the water surface and not risk missing anything
        Vector3 start = VectorUtility.twoDimensional3d(this.Position, Game.Instance.WaterLevelHeight - 2f + distance);

        // check for a collision
        RaycastHit hit;
        if (Physics.Raycast(start, Vector3.down, out hit, distance))
        {
            // 0.5f is used for systems that aren't going in their natural direction
            if (hit.collider.CompareTag(waterTag))
            {
                if (this.IsHighPressure)
                {
                    this.Pressure -= 0.5f;
                }
                else
                {
                    this.Pressure -= 1;
                }
            }
            else
            {
                if (this.IsHighPressure)
                {
                    this.Pressure += 1;
                }
                else
                {
                    this.Pressure += 0.5f;
                }
            }
        }
    }
}
