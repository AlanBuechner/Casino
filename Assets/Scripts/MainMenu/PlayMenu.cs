using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMenu : MonoBehaviour
{
	[SerializeField]
	private GameObject m_MainMenu;

	[SerializeField]
	private GameObject m_BankMenu;

	public void OnBack()
	{
		m_MainMenu.SetActive(true);
		gameObject.SetActive(false);
	}

	public void OnBank()
	{
		m_BankMenu.SetActive(true);
		gameObject.SetActive(false);
	}
	public static void OnRoulette()
	{
		Debug.LogWarning("Roulette not yet implemented");
	}

	public static void OnCraps()
	{
		Debug.LogWarning("Craps not yet implemented");
	}

	public static void OnBlackJack()
	{
		GameManager.ChangeScene(GameManager.BlackJackScene);
	}

	public static void OnSlots()
	{
		GameManager.ChangeScene(GameManager.SlotsScene);
	}

	public static void OnPoker()
	{
		GameManager.ChangeScene(GameManager.PokerScene);
	}
}
