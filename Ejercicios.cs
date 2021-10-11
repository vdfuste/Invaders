using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ejercicios : MonoBehaviour
{
	// VARIABLES/ATTRIBUTES
	
	// FUNCTIONS/METHODS
	void Start()
	{
		Factorial(3);
	}

	/*
	Factorial:
		Factorial de 5 es  5 * 4 * 3 * 2

	Ejemplos:
		Factorial(3);  =>  6
		Factorial(5);  =>  120
	*/

	void Factorial(int num)
	{
		int result = 1;

		// Lógica para calcular el factorial de num
		for(int i = num; i > 1; i--)
		{
			result = result * i;
		}

		Debug.Log(num + "! = " + result);
	}
}
