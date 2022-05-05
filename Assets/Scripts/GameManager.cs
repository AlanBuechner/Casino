using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private static GameManager s_Instance;
	public static GameManager Get() { return s_Instance; }

	[SerializeField]
	private string m_MainMenuSceneName;
	[SerializeField]
	private string m_PokerSceneName;
	[SerializeField]
	private string m_BlackJackSceneName;
	[SerializeField]
	private string m_SlotsSceneName;

	public static string MainMenuScene { get { return Get().m_MainMenuSceneName; } }
	public static string PokerScene { get { return Get().m_PokerSceneName; } }
	public static string BlackJackScene { get { return Get().m_BlackJackSceneName; } }
	public static string SlotsScene { get { return Get().m_SlotsSceneName; } }

	private void Awake()
	{
		if(s_Instance != null){
			Destroy(gameObject);
			return;
		}
		s_Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public static void ChangeScene(string scene)
	{
		SceneManager.LoadScene(scene);
	}

	public static void RetrunToMainMenu()
	{
		SceneManager.LoadScene(MainMenuScene);
	}
}
