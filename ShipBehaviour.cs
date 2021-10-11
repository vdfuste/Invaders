using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShipBehaviour : MonoBehaviour
{
	// VARIABLES/ATTRIBUTES
	public readonly float speed = 3f;
	public int ammo = 3;
	public float shootCoolDown = 3f;
	public Transform limit;
	public Text healthText;
	public GameObject bullet;
	public Transform[] bulletParents;
	public int maxShoots = 1;

	private int health = 100;
	private float coolDown = 0;


	// FUNCTIONS/METHODS
	private void Update()
	{
		if(GameManager.play)
		{
			if(coolDown < 0)
			{
				// Disparamos cuando pulsamos Espacio o Arriba
				if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
				{
					for(int i = 0; i < bulletParents.Length; i++)
					{
						Instantiate(bullet, bulletParents[i].position, bulletParents[i].rotation);

						if(i == maxShoots - 1) i = bulletParents.Length;
					}

					coolDown = shootCoolDown;
				}
			}
			else
			{
				coolDown -= Time.deltaTime;
			}

			// Aparecer por el borde contrario
			//Comprobamos si se sale por un lado de la pantalla
			if(transform.position.x > limit.position.x)
			{
				// Guardamos la posición actual en un Vector2
				Vector2 actualPosition = transform.position;

				actualPosition.x = -limit.position.x;

				transform.position = actualPosition;
			}
			else if(transform.position.x < -limit.position.x)
			{
				// Guardamos la posición actual en un Vector2
				Vector2 actualPosition = transform.position;

				actualPosition.x = limit.position.x;

				transform.position = actualPosition;
			}

			// Movemos la nave según pulsemos izquierda o derecha
			transform.Translate(speed * Input.GetAxis("Horizontal") * Time.deltaTime, 0, 0);
		}
	}

	// "col" equivale al objeto que toque la nave
	private void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "AlienBullet")
		{
			GameManager.streak = 0;
			GameManager.damage++;
			
			UpdateHealth(-10);

			Destroy(col.gameObject);
		}
	}

	private void UpdateHealth(int amount)
	{
		health += amount;

		healthText.text = "Health " + health;

		// Si health es 0 vamos a la pantalla de Ranking/GameOver
		if(health <= 0)
		{
			SceneManager.LoadScene("Ranking");
		}
	}
}
