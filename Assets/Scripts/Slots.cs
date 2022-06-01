using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slots : MonoBehaviour
{
	[SerializeField] private Text m_BetAmountText;
	[SerializeField] private Text betWin;
	[SerializeField] private Text betWinAmount;
	[SerializeField] private Button m_PlayBtn;
	[SerializeField] private Button m_BackBtn;

	[SerializeField] private Image WheelOneImage;
	[SerializeField] private Image WheelTwoImage;
	[SerializeField] private Image WheelThreeImage;

	private int m_BetAmount = 0;	
	private bool m_Playing = false;

	private int[] SlotChance;

	public void OnPlay()
    {
		SlotChance = new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 6, 6 };

		int wheelOneIndex = Random.Range(0, SlotChance.Length);
		int wheelTwoIndex = Random.Range(0, SlotChance.Length);
		int wheelThreeIndex = Random.Range(0, SlotChance.Length);

		int wheelOne = SlotChance[wheelOneIndex];
		int wheelTwo = SlotChance[wheelTwoIndex];
		int wheelThree = SlotChance[wheelThreeIndex];

		WheelOneImage.sprite = AssetManager.Get().SlotSprites[wheelOne - 1];
		WheelTwoImage.sprite = AssetManager.Get().SlotSprites[wheelTwo - 1];
		WheelThreeImage.sprite = AssetManager.Get().SlotSprites[wheelThree - 1];

		if(wheelOne == wheelTwo && wheelTwo == wheelThree)
        {
			// Cherry win ratio
			if(wheelOne == 1)
            {
				OnWin(m_BetAmount);

				//Debug.Log("All 1s");
            }
			// Plum win ratio
			else if(wheelOne == 2)
            {
				OnWin(m_BetAmount * 2);

				//Debug.Log("All 2s");
			}
			// Lemon win ratio
			else if(wheelOne == 3)
            {
				OnWin(m_BetAmount * 5);

				//Debug.Log("All 3s");
			}
			// Bell win ratio
			else if (wheelOne == 4)
			{
				OnWin(m_BetAmount * 10);

				//Debug.Log("All 4s");
			}
			// Bar win ratio
			else if (wheelOne == 5)
			{
				OnWin(m_BetAmount * 100);

				//Debug.Log("All 5s");
			}
			// Seven win ratio
			else if (wheelOne == 6)
			{
				OnWin(m_BetAmount * 500);

				//Debug.Log("All 6s");
			}

		}
        else
        {
			betWin.gameObject.SetActive(true);
			betWin.text = "You lost";
			betWinAmount.gameObject.SetActive(true);
			betWinAmount.text = m_BetAmount.ToString();

			m_BetAmount = 0;
			m_BetAmountText.text = "0";

			//Debug.Log(wheelOne + ", " + wheelTwo + ", " + wheelThree);
		}


		m_PlayBtn.interactable = false;
		m_BackBtn.interactable = true;

	}

	public void OnWin(int winAmount)
    {
		Player.Get().m_Chips += winAmount;

		betWin.gameObject.SetActive(true);
		betWin.text = "You Won";
		betWinAmount.gameObject.SetActive(true);
		betWinAmount.text = winAmount.ToString();

		m_BetAmount = 0;
		m_BetAmountText.text = "0";
	}

	public void OnBetPlace(int amount)
	{
		if (!m_Playing)
		{
			if (Player.Get().m_Chips >= amount)
			{
				Player.Get().m_Chips -= amount;
				m_BetAmount += amount;
				m_BetAmountText.text = string.Format("{0:n0}", m_BetAmount);
				Beter.Get().UpdateDisplay();

				m_PlayBtn.interactable = true;
				m_BackBtn.interactable = false;
			}
		}
	}
}
