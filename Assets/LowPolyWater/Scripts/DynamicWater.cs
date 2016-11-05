using UnityEngine;
using System.Collections;

public class DynamicWater : MonoBehaviour {

    Material waveMaterial;
    GlobalReferences globals;

	public float ParticleSystemYVel;


	public ParticleSystem part;
	public ParticleCollisionEvent[] collisionEvents;


    //public GameObject splashPrefab;

	// Use this for initialization
	void Start ()
    {
        globals = GameObject.Find("GlobalReferences").GetComponent<GlobalReferences>();
        waveMaterial = gameObject.GetComponent<Renderer>().sharedMaterial;
	
		collisionEvents = new ParticleCollisionEvent[16];

	}
	
	// Update is called once per frame
	void Update () {
        for(int i = 0; i < globals.wavesIsMoving.Length; i++)
        {
            if (globals.wavesIsMoving[i])
            {
				globals.collisionVectors[i].w = globals.waveTimers[i] * globals.SplashSpeed;
                waveMaterial.SetVector("_CollisionVectors" + i.ToString(), globals.collisionVectors[i]);
				if (globals.collisionVectors[i].w >= globals.SplashStayTime)
                {
                    globals.wavesIsMoving[i] = false;
                    globals.collisionVectors[i].w = 0.0f;
                    globals.waveTimers[i] = 0.0f;
                }
                globals.waveTimers[i] += Time.deltaTime;
            }
        }
	}

    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Box")
        {
            //GameObject splash = Instantiate(splashPrefab, coll.transform.position, Quaternion.identity) as GameObject;
            //splash.transform.rotation = Quaternion.EulerAngles(-90, 0, 0);
            //Destroy(splash, 2.0f);
            Rigidbody rigB = coll.GetComponent<Rigidbody>();
            globals.wavesIsMoving[globals.waveCounter] = true;
            globals.waveTimers[globals.waveCounter] = 0.0f;
            globals.collisionVectors[globals.waveCounter].x = coll.transform.position.x;
            globals.collisionVectors[globals.waveCounter].y = coll.transform.position.z;
            globals.collisionVectors[globals.waveCounter].z = rigB.velocity.y * rigB.mass * 0.01f;
            globals.collisionVectors[globals.waveCounter].w = 0.0f;
            globals.waveCounter++;
			if(globals.waveCounter >= globals.ArrayLength)
            {
                globals.waveCounter = 0;
            }

        }
    }

	void OnParticleCollision(GameObject coll) 
	{
			//GameObject splash = Instantiate(splashPrefab, coll.transform.position, Quaternion.identity) as GameObject;
			//splash.transform.rotation = Quaternion.EulerAngles(-90, 0, 0);
			//Destroy(splash, 2.0f);

			int safeLength = part.GetSafeCollisionEventSize();
			if (collisionEvents.Length < safeLength)
				collisionEvents = new ParticleCollisionEvent[safeLength];

			int numCollisionEvents = part.GetCollisionEvents(gameObject, collisionEvents);
			Rigidbody rigB = coll.GetComponent<Rigidbody>();
	

			int i = 0;
			while (i < numCollisionEvents) 
			{
				globals.wavesIsMoving [globals.waveCounter] = true;
				globals.waveTimers [globals.waveCounter] = 0.0f;
				globals.collisionVectors [globals.waveCounter].x = collisionEvents[i].intersection.x;
				globals.collisionVectors [globals.waveCounter].y = collisionEvents[i].intersection.z;
				globals.collisionVectors [globals.waveCounter].z = ParticleSystemYVel * rigB.mass * 0.01f;
				globals.collisionVectors [globals.waveCounter].w = 0.0f;
				globals.waveCounter++;
				if (globals.waveCounter >= globals.ArrayLength) {
					globals.waveCounter = 0;
				}
				i++;
			}

	  }

}
