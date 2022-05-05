using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private static Player s_Instance;
	public static Player Get() { return s_Instance; }

	public int m_Chips;

	private void Awake()
	{
		if(s_Instance != null){
			Destroy(gameObject);
			return;
		}
		s_Instance = this;
	}

	private void Start()
	{
		GameManager.ChangeScene(GameManager.MainMenuScene);
	}
}
