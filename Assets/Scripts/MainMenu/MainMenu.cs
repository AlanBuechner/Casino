using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
	private static MainMenu s_Instance;
	public static MainMenu Get() { return s_Instance; }

	[SerializeField]
	private GameObject m_PlayMenu;

	private void Awake()
	{
		if(s_Instance != null){
			Destroy(gameObject);
			return;
		}
		s_Instance = this;
	}

	public void OnPlay()
	{
		m_PlayMenu.SetActive(true);
		gameObject.SetActive(false);
	}

	public void OnExit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
