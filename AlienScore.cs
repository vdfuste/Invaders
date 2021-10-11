using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienScore : MonoBehaviour
{
	public float speed = 2f;
	
	int counter = 0;
	
	private void Update()
	{
		if(counter < 100)
		{
			transform.Translate(speed * Vector2.up * Time.deltaTime);

			counter++;
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
