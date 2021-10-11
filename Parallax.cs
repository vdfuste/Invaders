using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
	public float speed = 3f;

	private Material material;
	
	void Start()
	{
		material = GetComponent<Renderer>().material;
	}

	void Update()
	{
		material.SetTextureOffset("_MainTex", new Vector2(0, speed/10 * Time.time));
	}
}
