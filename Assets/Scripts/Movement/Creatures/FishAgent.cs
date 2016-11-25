using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof (FishAgentConfig))]
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (Collider))]
public class FishAgent : MonoBehaviour 
{
    public Vector3 velocity;

	// Move towards wehn wondering
	private Vector3 WanderTarget;
    
    // Rigid body
    private Rigidbody2D rb;
    
    private Vector3 acceleration;
	protected LayerMask predatorLayer;
	protected LayerMask agentLayer;
	protected FishAgentConfig config;

	protected virtual void setLayers()
	{
		// Get predator layer
		this.predatorLayer = LayerMask.GetMask(new string[] {"Predator"});
		
		// Get agent layer
		this.agentLayer = LayerMask.GetMask(new string[] {"Agent"});
	}

    /// <summary>
    /// Initialize with basic info
    /// </summary>
    void Start ()
    {
        // Start with random velocity
        this.velocity = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-2,2));

		// set layers
		this.setLayers();

		// Get configuration
		this.config = this.GetComponent<FishAgentConfig>();
		this.config.RandomizeSelf();
	}
	
    /// <summary>
    /// Update movement of agent
    /// </summary>
	void FixedUpdate ()
    {
        // Update acceleration
        this.acceleration = Vector3.ClampMagnitude(this.Combine(), this.config.MaxAcceleration);

        // Euler Forward Integration
        this.velocity = Vector3.ClampMagnitude(this.velocity + this.acceleration * Time.deltaTime, this.config.MaxVelocity);

        // Set new position
        this.transform.position = this.transform.position + (Vector3) (this.velocity * Time.deltaTime);

        // TODO I'm keeping this around in case we want to add this in the future.
        // Keep agent in world bounds
//        this.transform.position = World.Instance.WrapAround(this.transform.position);

        // set rotation
		transform.rotation = Quaternion.LookRotation(this.velocity + this.transform.position);// Quaternion.AngleAxis(angle, this.velocity);
	}

    /// <summary>
    /// Go to center of neighbors
    /// </summary>
    /// <returns>Center Point</returns>
    public Vector3 Cohesion()
    {
        // Cohesion behaavior
        Vector3 result = new Vector3();
        
        // Get all neighbors
		Collider2D[] neighbors = Physics2D.OverlapCircleAll(this.transform.position, this.config.CohesionRadius, this.agentLayer);

        // check if neighbors is full or not
        if (neighbors.Length > 0)
        {
			// Num agents
			int agentCount = 0;

            for (int i = 0; i < neighbors.Length; ++i)
            {
                result += neighbors[i].transform.position;
                ++agentCount;
            }

            // Divide by count
            if (agentCount > 0)
            {
                result /= agentCount;
            }
            
            // Look center
            result = result - this.transform.position;

            // normalize vector
            result.Normalize();
        }

        // Return result
        return result;
    }

    /// <summary>
    /// Move away from neighbors
    /// </summary>
    /// <returns></returns>
    public Vector3 Separation()
    {
        // Separation result
        Vector3 result = new Vector3();

        // Get all neighbors
		Collider2D[] neighbors = Physics2D.OverlapCircleAll(this.transform.position, this.config.SeparationRadius, this.gameObject.layer);

        // check if neighbors is full or not
        for (int i = 0; i < neighbors.Length; ++i)
        {
            Vector3 towardsMe = this.transform.position - neighbors[i].transform.position;

            // Contribution depends on distance
            if (towardsMe.magnitude > 0)
            {
                result = towardsMe.normalized / towardsMe.magnitude;
            }

            // Normalize
            result.Normalize();
        }

        // return result
        return result;
    }

    /// <summary>
    /// Rotate in direction of movement
    /// </summary>
    /// <returns></returns>
    public Vector3 Allignment()
    {
        Vector3 result = new Vector3();

        // Get all neighbors
		Collider2D[] neighbors = Physics2D.OverlapCircleAll(this.transform.position, this.config.SeparationRadius, this.gameObject.layer);

        // check if neighbors is full or not
        if (neighbors.Length > 0)
        {
            for (int i = 0; i < neighbors.Length; ++i)
            {
				result += neighbors[i].gameObject.GetComponent<FishAgent>().velocity;
            }

            // Nomalize vector
            result.Normalize();
        }

        return result;
    }

	/// <summary>
	/// Flee from all enemies
	/// </summary>
	public Vector3 AvoidEnemies()
	{
		Vector3 result = new Vector3();
		
		Collider2D[] enemies = Physics2D.OverlapCircleAll(this.transform.position, this.config.AvoidRadius, this.predatorLayer);

		for (int i = 0; i < enemies.Length; ++i)
		{
			result += this.Flee(enemies[i].transform.position);
		}
		
		return result;
	}

	/// <summary>
	/// Smooth out movement
	/// </summary>
	/// <returns></returns>
	public Vector3 Wander()
	{
		float jitter = this.config.Jitter * Time.deltaTime;

		this.WanderTarget += new Vector3(this.randomBinomial() * jitter, this.randomBinomial() * jitter, 0);
		
		this.WanderTarget.Normalize();
		this.WanderTarget *= this.config.WanderRadius;
		Vector3 targetInLocalSpace = this.WanderTarget + new Vector3(0, 0, this.config.WanderDistanceRadius);
		Vector3 targetInWorldSpace = this.transform.TransformPoint(targetInLocalSpace);
		return (targetInWorldSpace - this.transform.position).normalized;
	}

    /// <summary>
    /// Use Allignment, Cohesion, and Separation to define behavior with diferent proportions based on importance
    /// </summary>
    /// <returns>Vector with correct behavior</returns>
    public virtual Vector3 Combine()
    {
        return this.config.CohesionWeight * this.Cohesion() 
             + this.config.SeparationWeight * this.Separation()
             + this.config.AllignmentWeight * this.Allignment()
             + this.config.WanderWeight * this.Wander();
//			 + this.config.AvoidWeight * this.AvoidEnemies();
    }

    /// <summary>
    /// Check if agent is in field of fiew for this agent
    /// </summary>
    /// <param name="agent"></param>
    /// <returns></returns>
    public bool InFieldOfView(Vector3 agent)
    {
        return Vector3.Angle(this.velocity, agent - this.transform.position) <= this.config.MaxFieldOfViewAngle;
    } 

	/// <summary>
	/// Randoms binomial with higher likelihood of 0
	/// </summary>
	/// <returns>The binomial.</returns>
    private float randomBinomial()
    {
        return Random.Range(0f, 1f) - Random.Range(0f, 1f);
    }

    /// <summary>
    /// Run opposite direction from the target
    /// </summary>
    /// <param name="targ"></param>
    /// <returns></returns>
    public Vector3 Flee(Vector3 targ)
    {
        Vector3 desiredVel = (this.transform.position - targ).normalized * this.config.MaxVelocity;
        return desiredVel - this.velocity;
    }
}
