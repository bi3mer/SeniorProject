using UnityEngine;
using System.Collections;

public class RaftWake : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Particle systems to be controlled by raft movement.")]
    private ParticleSystem[] emitters;

    [SerializeField]
    [Tooltip("Raft rigidbody.")]
    private Rigidbody raftRigidbody;

    [SerializeField]
    [Tooltip("Tolerence for checking if the raft is stopped.")]
    private float stopTolerence;
	
    /// <summary>
    /// Update each particle system according to raft movement
    /// </summary>
	void Update ()
    {
        for (int i = 0; i < emitters.Length; ++i)
        {
            updateEmitter(emitters[i]);
        }
	}

    /// <summary>
    /// Update the values of the particle system depending on the state of the raft
    /// </summary>
    /// <param name="emitter">The particle system.</param>
    private void updateEmitter(ParticleSystem emitter)
    {
        // return if we're no longer moving
        if (raftRigidbody.velocity.magnitude < stopTolerence)
        {
            return;
        }

        // rotate opposite the direction of movement
        emitter.transform.rotation = Quaternion.LookRotation(-raftRigidbody.velocity.normalized);

        // set wake speed
        emitter.startSpeed = raftRigidbody.velocity.magnitude;

        // set lifetime
        emitter.startLifetime = raftRigidbody.velocity.magnitude;
    }
}
