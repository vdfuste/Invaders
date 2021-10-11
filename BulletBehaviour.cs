using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
	// VARIABLES/ATTRIBUTES
	public float speed = 10f;


	// FUNCTIONS/METHODS
	private void Start()
	{
		if(gameObject.tag == "AlienBullet") speed *= -1;
	}

	private void Update()
	{
		//if(Mathf.Abs(transform.position.y) < 5.5f)
		if(
			transform.position.y < GameManager.bulletLimit 
			&& transform.position.y > -GameManager.bulletLimit
		){
			// Movemos la bala hacia arriba
			transform.Translate(0, speed * Time.deltaTime, 0);
		}
		else
		{
			// La bala se autodestruye
			// gameObject hace referencia al objeto que tiene este script
			Destroy(gameObject);
		}
	}
}
