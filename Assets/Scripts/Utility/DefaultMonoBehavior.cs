using UnityEngine;
using System.Collections;

public class DefaultMonoBehavior : MonoBehaviour 
{
	private static DefaultMonoBehavior instance;
	public static DefaultMonoBehavior Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<DefaultMonoBehavior>();
			}
			return instance;
		}
	}
}
