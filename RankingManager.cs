using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RankingManager : MonoBehaviour
{
	public Text[] scores;
	public Text newName, newScoreText;
	public InputField inputName;
	public GameObject newScoreInfo, gameOverTitle;

	private string[] names;
	private int[] dataScores;
	int scorePos = 5;

	private void Awake()
	{
		//PlayerPrefs.DeleteAll();
		
		if(!PlayerPrefs.HasKey("Score01"))
		{
			for(int i = 0; i < scores.Length; i++)
			{
				PlayerPrefs.SetString(scores[i].name, "AAA 000000");
			}
		}

		names = new string[scores.Length];
		dataScores = new int[scores.Length];

		// Recuperamos los datos del ranking
		for(int i = 0; i < scores.Length; i++)
		{
			string data = PlayerPrefs.GetString(scores[i].name);

			names[i] = "" + data[0] + data[1] + data[2];
			dataScores[i] = int.Parse("" + data[4] + data[5] + data[6] + data[7] + data[8] + data[9]);
			
			//Debug.Log("name: " + names[i] + ", score: " + dataScores[i] + "; data: " + data);
		}

		// Buscamos la posición del score en el ranking
		for(int i = 0; i < dataScores.Length; i++)
		{
			if(dataScores[i] < Persistant.score)
			{
				scorePos = i;
				i = dataScores.Length;
			}
		}

		newScoreText.text = Persistant.score.ToString("000000");

		if(scorePos < 5)
		{
			newScoreInfo.SetActive(true);
			gameOverTitle.SetActive(false);

			inputName.Select();

			// Desplazamos los scores más bajos al score actual
			for(int i = dataScores.Length - 1; i > scorePos; i--)
			{
				names[i] = names[i - 1];
				dataScores[i] = dataScores[i - 1];
			}

			// Guardamos el score actual
			dataScores[scorePos] = Persistant.score;

			names[scorePos] = "   ";
		}

		// Actualizamos los textos del ranking
		for(int i = 0; i < scores.Length; i++)
		{
			string newScore = names[i] + " " + dataScores[i].ToString("000000");

			scores[i].text = newScore;
		}
	}

	private void Update()
	{
		if(newName.text.Length == 3 && Input.GetKeyDown(KeyCode.Return))
		{
			if(scorePos < 5)
			{
				names[scorePos] = newName.text;

				// Actualizamos los textos del ranking
				for(int i = 0; i < scores.Length; i++)
				{
					string newScore = names[i] + " " + dataScores[i].ToString("000000");

					PlayerPrefs.SetString(scores[i].name, newScore);
				}
			}

			Persistant.score = 0;

			SceneManager.LoadScene("Game");
		}
		else inputName.Select();
	}
}
