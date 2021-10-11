using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	// VARIABLES/ATTRIBUTES
	public AlienBehaviour[] alienPrefabs;
	public Transform alienSpawn;
	public Transform bulletLimitObj;
	public Text textScore, textCounter, timeText;
	public float nextStep = 5f;

	public static bool play = false;
	public static float bulletLimit;
	public static int streak = 0;
	public static int damage = 0;

	private AlienBehaviour[] aliens; 
	private float stepCounter = 0;

	private float counter = 0f;
	private int seconds = 0, minutes = 0;

	// MOCK DATA
	int cols = 3;
	int rows = 2;
	int[] types = {
		0, 3, 0,
		2, 1, 2,
	};

	// FUNCTIONS/METHODS
	private void Awake()
	{
		bulletLimit = bulletLimitObj.position.y;
	}

	private void OnDestroy()
	{
		counter = 0;
		seconds = 0;
		minutes = 0;

		play = false;
	}

	private void Start()
	{
		aliens = new AlienBehaviour[cols * rows];

		for(int j = 0; j < rows; j++)
		{
			Vector2 newPos = alienSpawn.position;
			newPos.x -= 1.5f;
			newPos.y -= 1.5f * j;

			for(int i = 0; i < cols; i++)
			{
				newPos.x += 1.5f;

				int n = j * cols + i;

				if(types[n] > 0)
				{
					int alienType = types[n] - 1;

					aliens[n] = Instantiate(alienPrefabs[alienType], newPos, Quaternion.identity);
				}
			}
		}
	}

	private void Update()
	{
		if((int)(counter % 60) < 1)
		{
			counter += Time.deltaTime;
		}
		else
		{
			seconds++;
			
			if(seconds == 60)
			{
				minutes++;
				seconds = 0;
			}

			counter = 0;
		}

		if(play)
		{
			timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

			bool changeScene = true;

			for(int i = 0; i < aliens.Length; i++)
			{
				if(aliens[i] != null)
				{
					changeScene = false;
					i = aliens.Length;
				}
			}

			if(changeScene)
			{
				SceneManager.LoadScene("Ranking");
			}

			// Actualizar el score
			textScore.text = Persistant.score.ToString("000000");

			// Los aliens bajan a cada N segundos
			if(stepCounter < nextStep)
			{
				// Disparos aleatorios
				if(Random.Range(0, 10) == 0)
				{
					// Solo dispararán los aliens que estén más abajo de cada columna
					for(int i = aliens.Length - cols; i < aliens.Length; i++)
					{
						for(int j = 0; j < rows; j++)
						{
							int n = i - cols * j;

							if(aliens[n] != null)
							{
								if(Random.Range(0, 100) == 0) aliens[n].Shoot();

								j = rows;
							}
						}
					}
				}

				stepCounter += Time.deltaTime;
			}
			else
			{
				for(int i = 0; i < aliens.Length; i++)
				{
					// Si la variable aliens[i] es Null quiere decir que no tiene ninguna referencia
					if(aliens[i] != null) aliens[i].StepDown();
				}

				stepCounter = 0;
			}
		}
		else
		{
			if(seconds < 3)
			{
				textCounter.text = (3 - seconds).ToString();
			}
			else
			{
				play = true;

				textCounter.enabled = false;

				seconds = 0;
			}
		}
	}
}
