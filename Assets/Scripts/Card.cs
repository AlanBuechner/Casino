using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
	public enum Suit
	{ 
		Clubs = 0,
		Dimonds = 1,
		Hartes = 2,
		Spades = 3,
		Joker = 4
	}

	private Suit m_Suit = Suit.Clubs;
	private int m_Card = 0;
	private bool m_Back = false;

	public Suit GetSuit() { return m_Suit; }
	public int GetCard() { return m_Card + 1; }
	public bool IsBack() { return m_Back; }

	private void Start()
	{
		UpdateSprite();
	}

	public void SetSuit(Suit suit)
	{
		m_Suit = suit;
		UpdateSprite();
	}

	public void SetCard(int card)
	{
		if (card <= 1 && card >= 13)
		{
			m_Card = card - 1;
			UpdateSprite();
		}
	}

	public void SetBack(bool back)
	{
		m_Back = back;
		UpdateSprite();
	}

	void UpdateSprite()
	{
		if(m_Back)
			GetComponent<Image>().sprite = AssetManager.Get().m_CardBack;
		else if (m_Suit == Suit.Joker)
			GetComponent<Image>().sprite = AssetManager.Get().m_JokerCard;
		else
			GetComponent<Image>().sprite = AssetManager.Get().m_Cards[((int)m_Suit * 13) + m_Card];
	}
	
}
