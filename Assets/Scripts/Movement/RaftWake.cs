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
	
	void Update ()
    {
        for (int i = 0; i < emitters.Length; ++i)
        {
            updateEmitter(emitters[i]);
        }
	}

    private void updateEmitter(ParticleSystem emitter)
    {
        var em = emitter.emission;

        // return if we're no longer moving
        if (raftRigidbody.velocity.magnitude < stopTolerence)
        {
            em.enabled = false;
            return;
        }

        em.enabled = true;

        // rotate opposite the direction of movement
        emitter.transform.rotation = Quaternion.LookRotation(-raftRigidbody.velocity.normalized);

        // set wake speed
        emitter.startSpeed = raftRigidbody.velocity.magnitude;

        // set lifetime
        emitter.startLifetime = raftRigidbody.velocity.magnitude;
    }
}
