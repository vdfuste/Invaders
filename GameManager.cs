using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	// VARIABLES/ATTRIBUTES
	public AlienBehaviour[] alienPrefabs;
	public Transform alienSpawn, bulletLimitObj, ship;
	public Text textScore, textCounter, timeText;
	public float nextStep = 5f;

	public static bool play = false;
	public static float bulletLimit;
	public static int streak = 0;
	public static int damage = 0;

	//private AlienBehaviour[] aliens; 
	private List<AlienBehaviour> aliens = new List<AlienBehaviour>();
	private float stepCounter = 0;

	private float counter = 0f;
	private int seconds = 0, minutes = 0;

	// MOCK DATA
	int cols = 5;
	int rows = 3;

	public static int currentLevel = 0;

	List<int>[] levels = 
	{
		new List<int>()
		{
			0, 0, 2, 0, 0,
			0, 0, 1, 0, 0,
			0, 1, 0, 1, 0,
		},
		new List<int>()
		{
			1, 0, 2, 0, 1,
			0, 1, 0, 1, 0,
		},
		new List<int>()
		{
			0, 2, 0, 2, 0,
			1, 0, 3, 0, 1,
			0, 1, 1, 1, 0,
			0, 0, 1, 0, 0,
		},
		new List<int>()
		{
			0, 2, 0, 2, 0,
			1, 0, 3, 0, 1,
			0, 1, 1, 1, 0,
			0, 0, 1, 0, 0,
			0, 0, 3, 0, 0,
			0, 2, 0, 2, 0,
		},
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
		LoadLevel();
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

			// Comprobamos si ya hemos superado el nivel
			bool changeLevel = true;
			for(int i = 0; i < aliens.Count; i++)
			{
				if(aliens[i] != null)
				{
					changeLevel = false;
					i = aliens.Count;
				}
			}

			if(changeLevel)
			{
				if(currentLevel < levels.Length - 1)
				{
					currentLevel++;

					LoadLevel();
				}
				else SceneManager.LoadScene("Ranking");
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
					for(int i = aliens.Count - cols; i < aliens.Count; i++)
					{
						for(int j = 0; j < rows; j++)
						{
							int n = i - cols * j;

							//Debug.Log("i: " + i + ", j: " + j);

							if(aliens[n] != null)
							{
								if(aliens[n].transform.position.y < bulletLimit && Random.Range(0, 100) == 0) aliens[n].Shoot();

								j = rows;
							}
						}
					}
				}

				stepCounter += Time.deltaTime;
			}
			else
			{
				for(int i = 0; i < aliens.Count; i++)
				{
					// Si la variable aliens[i] es Null quiere decir que no tiene ninguna referencia
					if(aliens[i] != null)
					{
						aliens[i].StepDown();

						if(aliens[i].transform.position.y <= ship.position.y)
						{
							SceneManager.LoadScene("Ranking");

							i = aliens.Count;
						}
					}
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

	private void LoadLevel()
	{
		aliens.Clear();
		
		rows = levels[currentLevel].Count / cols;

		if(currentLevel != 0) alienSpawn.position = new Vector2(alienSpawn.position.x, 9 + rows);

		for(int j = 0; j < rows; j++)
		{
			Vector2 newPos = alienSpawn.position;
			newPos.x -= 1.5f;
			newPos.y -= 1.5f * j;

			for(int i = 0; i < cols; i++)
			{
				newPos.x += 1.5f;

				int n = j * cols + i;

				if(levels[currentLevel][n] > 0)
				{
					int alienType = levels[currentLevel][n] - 1;

					aliens.Add(Instantiate(alienPrefabs[alienType], newPos, Quaternion.identity));
				}
				else aliens.Add(null);
			}
		}
	}
}
