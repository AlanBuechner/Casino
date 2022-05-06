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

	[SerializeField]
	private Suit m_Suit = Suit.Clubs;
	[SerializeField]
	private int m_Card = 0;
	[SerializeField]
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
		if (card >= 1 && card <= 13)
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

	public int GetIndex()
	{
		if (m_Suit == Suit.Joker)
			return 52;
		return ((int)m_Suit * 13) + m_Card;
	}

	public void SetIndex(int index)
	{
		if(index == 52)
			m_Suit = Suit.Joker;
		else
		{
			m_Suit = (Suit)(index/13);
			m_Card = index % 13;
		}
		UpdateSprite();
	}

	void UpdateSprite()
	{
		if(m_Back)
			GetComponent<Image>().sprite = AssetManager.Get().m_CardBack;
		else if (m_Suit == Suit.Joker)
			GetComponent<Image>().sprite = AssetManager.Get().m_JokerCard;
		else
			GetComponent<Image>().sprite = AssetManager.Get().m_Cards[GetIndex()];
	}
	
}
