using UnityEngine;
using System.Collections;

public class FishAgentConfig : MonoBehaviour 
{
	public static float MaxRadius = 20f;
	public static float MaxWeight = 300f;

    // TODO: add line breaks in inspector and create better names
	[Header("Find Nearby Radius")]
    public float CohesionRadius = 20f;
	public float SeparationRadius = 10f;
	public float AlignmentRadius = 3f;
	public float WanderRadius = 2f;
	public float AvoidRadius = 0f;

	[Header("Weights for Given Behavior")]
	public float CohesionWeight = 80f;
	public float SeparationWeight = 70f;
	public float AlignmentWeight = 56f;
	public float WanderWeight = 30f;
	public float AvoidWeight = 0f;

	[Header ("Physics")]
	public float MaxAcceleration = 2f;
	public float MaxVelocity = 2f;

    [Header ("Smoothing Movement")]
	public float Jitter = 2f;
	public float WanderDistanceRadius = 2f;

	[Header("Vision")]
	[Tooltip("Max angle that the agent can see from its forward direction")]
	public float MaxFieldOfViewAngle = 180f;

	[Header("Appearance")]
	[Tooltip("Difference between water level and agent")]
	public float waterLevelOffset = -1;

	/// <summary>
	/// Randomizes the paramaters
	/// </summary>
	public void RandomizeSelf()
	{
		// Radius
		this.CohesionRadius = Random.Range(0, FishAgentConfig.MaxRadius);
		this.SeparationRadius = Random.Range(0, FishAgentConfig.MaxRadius);
		this.AlignmentRadius = Random.Range(0, FishAgentConfig.MaxRadius);
		this.WanderRadius = Random.Range(0, FishAgentConfig.MaxRadius);
		this.AvoidRadius = Random.Range(0, FishAgentConfig.MaxRadius);

		// Weights
		this.CohesionWeight = Random.Range(0, FishAgentConfig.MaxWeight);
		this.SeparationWeight = Random.Range(0, FishAgentConfig.MaxWeight);
		this.AlignmentWeight = Random.Range(0, FishAgentConfig.MaxWeight);
		this.WanderWeight = Random.Range(0, FishAgentConfig.MaxWeight);
		this.AvoidWeight = Random.Range(0, FishAgentConfig.MaxWeight);

		// Smooth Movement
		this.Jitter = Random.Range(0, FishAgentConfig.MaxWeight);
		this.WanderDistanceRadius = Random.Range(0, FishAgentConfig.MaxRadius);
	}
}