using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBehaviour : MonoBehaviour
{
	// VARIABLES/ATTRIBUTES
	public GameObject bullet;
	public Transform bulletParent;
	public TextMesh alienScoreText;


	// FUNCTIONS/METHODS
	// "col" equivale al objeto que toque al alien
	private void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Bullet")
		{
			// Actualizamos el valor de score
			int addScore = 10;

			if(gameObject.name[5] == '2') addScore = 20;
			else if(gameObject.name[5] == '3') addScore = 50;

			GameManager.streak++;

			int finalScore = addScore * GameManager.streak;

			Persistant.score += finalScore;

			Instantiate(alienScoreText, transform.position, Quaternion.identity);
			
			// Este destroy se destruye a si mismo
			Destroy(gameObject);

			// Este destroy destruye la bala que le toque
			Destroy(col.gameObject);
		}
	}

	public void Shoot()
	{
		Instantiate(bullet, bulletParent.position, bulletParent.rotation);
	}

	public void StepDown()
	{
		transform.Translate(0, -0.5f, 0);
	}
}
