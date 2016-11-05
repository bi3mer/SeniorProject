using UnityEngine;
using System.Collections;

public class GlobalReferences : MonoBehaviour {

	public float SplashSpeed = 1f;
	public float SplashStayTime = 1f;

	public int ArrayLength = 20;

    public int waveCounter;
    public bool[] wavesIsMoving;
    public float[] waveTimers;
    public Vector4[] collisionVectors;

    void Start()
    {
        waveCounter = 0;
		wavesIsMoving = new bool[ArrayLength];
		waveTimers = new float[ArrayLength];
		collisionVectors = new Vector4[ArrayLength];
        for (int i = 0; i < collisionVectors.Length; i++)
        {
            collisionVectors[i] = new Vector4(0, 0, 0, 0);
        }
    }
}
