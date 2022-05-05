using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
	private static Bank s_Instance;
	public static Bank Get() { return s_Instance; }

	public int m_Chips;

	private void Awake()
	{
		if(s_Instance != null){
			Destroy(gameObject);
			return;
		}
		s_Instance = this;
	}
}
