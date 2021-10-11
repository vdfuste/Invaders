using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistant : MonoBehaviour
{
	public static int score = 0;

	private bool isCreated = false;

	private void Awake()
	{
		if(isCreated == false)
		{
			DontDestroyOnLoad(gameObject);

			isCreated = true;
		}
	}
}
