using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankMenu : MonoBehaviour
{
	[SerializeField]
	private GameObject m_PlayMenu;

	[SerializeField]
	private Text m_CasinoChipsDisplay;
	[SerializeField]
	private Text m_BankChipsDisplay;

	[SerializeField]
	private InputField m_DepositIF;
	[SerializeField]
	private InputField m_WithdrawIF;

	private void Start()
	{
		UpdateDisplays();
	}

	public void OnBack()
	{
		m_PlayMenu.SetActive(true);
		gameObject.SetActive(false);
	}

	private void UpdateDisplays()
	{
		m_CasinoChipsDisplay.text = string.Format("{0:n0}", Player.Get().m_Chips);
		m_BankChipsDisplay.text = string.Format("{0:n0}", Bank.Get().m_Chips);
	}

	public void OnDeposit()
	{
		int amount;
		if(int.TryParse(m_DepositIF.text, out amount) && amount <= Player.Get().m_Chips)
		{
			Player.Get().m_Chips -= amount;
			Bank.Get().m_Chips += amount;
		}
		UpdateDisplays();
	}

	public void OnWithdraw()
	{
		int amount;
		if (int.TryParse(m_WithdrawIF.text, out amount))
		{
			Bank.Get().m_Chips -= amount;
			Player.Get().m_Chips += amount;
		}
		UpdateDisplays();
	}
}
