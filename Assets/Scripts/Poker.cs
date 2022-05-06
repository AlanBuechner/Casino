using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Poker : MonoBehaviour
{
	[SerializeField]
	private Text m_BetAmountText;

	private int m_BetAmount = 0;

	[SerializeField]
	private Transform[] m_CardTransforms;

	private Card[] m_Cards;

	[SerializeField]
	private GameObject m_CardPrefab;

	[SerializeField]
	private Button m_PlayBtn;
	[SerializeField]
	private Button m_BackBtn;

	private bool m_Playing = false;

	private bool[] m_Replace = new bool[5];

	List<int> m_Deck = new List<int>();

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

	public void OnPlay()
	{
		if (m_Playing == false)
			Deal();
		else
			Draw();
	}

	private void Deal()
	{
		m_Playing = true;

		if (m_Cards == null)
		{
			m_Cards = new Card[m_CardTransforms.Length];
			for (int i = 0; i < m_CardTransforms.Length; i++)
			{
				int index = i;
				GameObject card = Instantiate(m_CardPrefab, m_CardTransforms[i]);
				m_Cards[i] = card.GetComponent<Card>();
				card.GetComponent<Button>().onClick.AddListener(delegate { CardClick(index); });
				m_Replace[i] = false;
			}
		}

		m_Deck.Clear();
		m_Deck = new List<int>{ 9+13,10,11,12,0 };
		//for (int i = 0; i < 52; i++)
		//	m_Deck.Add(i);

		for (int i = 0; i < m_Cards.Length; i++)
		{
			//int card = Random.Range(0, m_Deck.Count);
			m_Cards[i].SetIndex(m_Deck[i]);
			//m_Deck.RemoveAt(card);
		}

		m_PlayBtn.GetComponentInChildren<Text>().text = "Draw";
	}

	private void Draw()
	{
		for(int i = 0; i < m_Cards.Length; i++)
		{
			if(m_Replace[i])
			{
				int card = Random.Range(0, m_Deck.Count);
				m_Cards[i].SetIndex(m_Deck[card]);
				m_Deck.RemoveAt(card);
				m_Replace[i] = false;
				Image cardImage = m_Cards[i].GetComponent<Image>();
				Color color = cardImage.color;
				color.a = m_Replace[i] ? 0.5f : 1.0f;
				cardImage.color = color;
			}
		}

		// find players payout
		System.Func<int>[] payoutFunctions = { RoyalFlush, StraightFlush, FourOfAKind, FullHouse, Flush, Straight, ThreeOfAKind, TwoPair, Pair }; // list of functions that return the payout multiplier
		int payout = -1;
		for (int i = 0; i < payoutFunctions.Length; i++)
			if ((payout = payoutFunctions[i]()) != -1)
				break;

		Debug.Log(payout);
		Player.Get().m_Chips += (payout + 1) * m_BetAmount;
		Beter.Get().UpdateDisplay();

		m_BetAmount = 0;
		m_BetAmountText.text = string.Format("{0:n0}", m_BetAmount);

		m_PlayBtn.GetComponentInChildren<Text>().text = "Deal";
		m_PlayBtn.interactable = false;
		m_BackBtn.interactable = true;

		m_Playing = false;
	}

	private void CardClick(int index)
	{
		if (m_Playing)
		{
			m_Replace[index] = !m_Replace[index];
			Image cardImage = m_Cards[index].GetComponent<Image>();
			Color color = cardImage.color;
			color.a = m_Replace[index] ? 0.5f : 1.0f;
			cardImage.color = color;
		}
	}

	private int RoyalFlush()
	{
		return -1;
	}

	private int StraightFlush()
	{
		return -1;
	}

	private int FourOfAKind()
	{
		Dictionary<int, int> foundcards = new Dictionary<int, int>();
		for (int i = 0; i < m_Cards.Length; i++)
		{
			int card = m_Cards[i].GetCard();
			int amount;
			if (foundcards.TryGetValue(card, out amount))
			{
				foundcards[card] = ++amount;
				if (amount >= 4)
					return 1000;
			}
			else
				foundcards.Add(card, 1);
		}
		return -1;
	}

	private int FullHouse()
	{
		Dictionary<int, int> foundcards = new Dictionary<int, int>();
		for (int i = 0; i < m_Cards.Length; i++)
		{
			int card = m_Cards[i].GetCard();
			int amount;
			if (foundcards.TryGetValue(card, out amount))
				foundcards[card] = ++amount;
			else
				foundcards.Add(card, 1);
		}

		bool two = false, three = false;
		foreach (var f in foundcards)
		{
			if (f.Value == 2)
				two = true;
			if (f.Value == 3)
				three = true;
		}

		return two && three ? 100 : -1;
	}

	private int Flush()
	{
		Card.Suit suit = m_Cards[0].GetSuit();
		for (int i = 1; i < m_Cards.Length; i++)
			if (m_Cards[i].GetSuit() != suit)
				return -1;
		return 50;
	}

	private int Straight()
	{
		int currCardCheck = m_Cards[0].GetCard();
		for (int i = 1; i < m_Cards.Length; i++)
		{
			currCardCheck++;
			if (m_Cards[i].GetCard() != currCardCheck && !(i==m_Cards.Length-1 && m_Cards[i].GetCard()==currCardCheck%13))
				return -1;
		}
		return 25;
	}

	private int ThreeOfAKind()
	{
		Dictionary<int, int> foundcards = new Dictionary<int, int>();
		for (int i = 0; i < m_Cards.Length; i++)
		{
			int card = m_Cards[i].GetCard();
			int amount;
			if (foundcards.TryGetValue(card, out amount))
			{
				foundcards[card] = ++amount;
				if (amount >= 3)
					return 10;
			}
			else
				foundcards.Add(card, 1);
		}
		return -1;
	}

	private int TwoPair()
	{
		Dictionary<int, int> foundcards = new Dictionary<int, int>();
		for (int i = 0; i < m_Cards.Length; i++)
		{
			int card = m_Cards[i].GetCard();
			int amount;
			if (foundcards.TryGetValue(card, out amount))
				foundcards[card] = ++amount;
			else
				foundcards.Add(card, 1);
		}

		int pairs = 0;
		foreach (var f in foundcards)
			if (f.Value >= 2)
				pairs++;

		return pairs >= 2 ? 5 : -1;
	}

	private int Pair()
	{
		for(int i = 0; i < m_Cards.Length-1; i++)
		{
			for(int j = i+1; j < m_Cards.Length; j++)
			{
				if (m_Cards[i].GetCard() == m_Cards[j].GetCard())
					return 1;
			}
		}
		return -1;
	}
}
