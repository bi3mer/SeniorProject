using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof (FishAgentConfig))]
[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (Collider))]
public class FishAgent : MonoBehaviour 
{
	/// <summary>
	/// The velocity of the agent
	/// </summary>
    public Vector3 Velocity;

	// Move towards when wondering
	private Vector3 WanderTarget;
    
    // Rigid body
    private Rigidbody2D rb;
    
    private Vector3 acceleration;
	protected LayerMask predatorLayer;
	protected LayerMask agentLayer;
	protected FishAgentConfig config;

	private const string predatorLayerString   = "Predator";
	private const string fisghAgentLayerString = "Agent";

	protected virtual void setLayers()
	{
		// Get predator layer
		this.predatorLayer = LayerMask.GetMask(new string[] {FishAgent.predatorLayerString});
		
		// Get agent layer
		this.agentLayer = LayerMask.GetMask(new string[] {FishAgent.predatorLayerString});
	}

    /// <summary>
    /// Initialize with basic info
    /// </summary>
    void Start ()
    {
   		// set layers
		this.setLayers();

		// Get configuration
		this.config = this.GetComponent<FishAgentConfig>();
		this.config.RandomizeSelf();

		// Start with random velocity
		this.Velocity = new Vector3(Random.Range(-this.config.MaxVelocity, this.config.MaxVelocity), 
			                        Random.Range(-this.config.MaxVelocity, this.config.MaxVelocity), 
			                        Random.Range(-this.config.MaxVelocity, this.config.MaxVelocity));
	}
	
    /// <summary>
    /// Update movement of agent
    /// </summary>
	void FixedUpdate ()
    {
        // Update acceleration
        this.acceleration = Vector3.ClampMagnitude(this.Combine(), this.config.MaxAcceleration);

        // Euler Forward Integration
		this.Velocity = Vector3.ClampMagnitude(this.Velocity + this.acceleration * Time.deltaTime, this.config.MaxVelocity);

        // Set new position
		this.transform.position = this.transform.position + (Vector3) (this.Velocity * Time.deltaTime);

        // TODO I'm keeping this around in case we want to add this in the future.
        // Keep agent in world bounds
//        this.transform.position = World.Instance.WrapAround(this.transform.position);

        // set rotation
		transform.rotation = Quaternion.LookRotation(this.Velocity + this.transform.position);// Quaternion.AngleAxis(angle, this.velocity);
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
            result -= this.transform.position;

            result.Normalize();
        }

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
	public Vector3 Alignment()
    {
        Vector3 result = new Vector3();

        // Get all neighbors
		Collider2D[] neighbors = Physics2D.OverlapCircleAll(this.transform.position, this.config.SeparationRadius, this.gameObject.layer);

        // check if neighbors is full or not
        if (neighbors.Length > 0)
        {
            for (int i = 0; i < neighbors.Length; ++i)
            {
            	// TODO add id system to reduce runtime hit
				result += neighbors[i].gameObject.GetComponent<FishAgent>().Velocity;
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
		Vector3 wanderTarget = (targetInWorldSpace - this.transform.position);
		return wanderTarget.normalized;
	}

    /// <summary>
    /// Use Allignment, Cohesion, and Separation to define behavior with diferent proportions based on importance
    /// </summary>
    /// <returns>Vector with correct behavior</returns>
    public virtual Vector3 Combine()
    {
        return this.config.CohesionWeight * this.Cohesion() 
             + this.config.SeparationWeight * this.Separation()
			 + this.config.AllignmentWeight * this.Alignment()
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
		return Vector3.Angle(this.Velocity, agent - this.transform.position) <= this.config.MaxFieldOfViewAngle;
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
		return desiredVel - this.Velocity;
    }
}