using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BlackJack : MonoBehaviour
{
    private List<int> deck = new List<int>();
    private List<int> shuffledDeck = new List<int>();
    private List<GameObject> playerHand = new List<GameObject>();
    private List<GameObject> playerSplit1 = new List<GameObject>();
    private List<GameObject> playerSplit2 = new List<GameObject>();
    private List<GameObject> dealerHand = new List<GameObject>();
    
    int bet = 0;
    int bet1 = 0;
    int bet2 = 0;
    bool isPlaying = false;

    [SerializeField]
    Text BetAmountText;

    [SerializeField]
    Transform[] CardTransforms;

    [SerializeField]
    GameObject CardPrefab;

    [SerializeField]
    Button playBtn;

    [SerializeField]
    Button backBtn;

    [SerializeField]
    Transform PlayerHandStart;

    [SerializeField]
    Transform PlayerSplit1Start;

    [SerializeField]
    Transform PlayerSplit2Start;

    [SerializeField]
    Transform DealerHandStart;

    // Update is called once per frame
    void Update()
    {

    }

    public void OnBetPlace(int amount)
    {
        if (!isPlaying)
        {
            if (Player.Get().m_Chips >= amount)
            {
                Player.Get().m_Chips -= amount;
                bet += amount;
                BetAmountText.text = bet.ToString();
                Beter.Get().UpdateDisplay();

                playBtn.interactable = true;
                backBtn.interactable = false;
            }
        }
    }

    public void OnPlay()
	{
        if (!isPlaying && bet != 0)
		{
            isPlaying = true;
            CreateDeck();
            ShuffleDeck();
            Deal();
            backBtn.interactable = false;
		}
	}

    public void CreateDeck()
	{
        for (int i = 0; i < 52; i++)
        {
            deck.Add(i);
        }
    }

    public void ShuffleDeck()
	{
        shuffledDeck.Clear();
        for (int i = 0; i < deck.Count; i++)
        {
            int card = Random.Range(0, deck.Count);
            shuffledDeck.Add(deck[card]);
            deck.RemoveAt(card);
        }
    }

    public GameObject DrawCard(Transform cardPos)
	{
        GameObject card = Instantiate(CardPrefab, cardPos);
        
        Card newCard = card.GetComponent<Card>();
        newCard.SetIndex(shuffledDeck[0]);

        shuffledDeck.RemoveAt(0);

        return card;
    }
    public void ClearAll(List<GameObject> gos)
	{
        foreach (GameObject go in gos)
		{
            Destroy(go);
		}
        gos.Clear();
	}

    public void Deal()
	{
        ClearAll(playerHand);
        ClearAll(dealerHand);
        ClearAll(playerSplit1);
        ClearAll(playerSplit2);


        for (int i = 0; i < 4; i++)
		{
            if (i % 2 == 0)
			{
                playerHand.Add(DrawCard(PlayerHandStart));
			}
            else
            {
                dealerHand.Add(DrawCard(DealerHandStart));
            }
        }

        //Player Hand
        for (int i = 0; i < playerHand.Count; i++)
		{
            playerHand[i].transform.position += Vector3.right * (i * 50);
        }
        //Split one
        for (int i = 0; i < playerSplit1.Count; i++)
        {
            playerSplit1[i].transform.position += Vector3.right * (i * 50); 
        }
        //split two
        for (int i = 0; i < playerSplit2.Count; i++)
        {
           
            playerSplit2[i].transform.position += Vector3.right * (i * 50);
        }
        //dealer
        for (int i = 0; i < dealerHand.Count; i++)
        {
            if (i == 0)
			{
                dealerHand[i].GetComponent<Card>().SetBack(true);
			}

            dealerHand[i].transform.position += Vector3.right * (i * 50);
            //new Vector3(dealerHand[i].transform.position.x + (i * 50), dealerHand[i].transform.position.y, dealerHand[i].transform.position.z);
        }
    }

    public void Hit()
    {
        if (isPlaying && playerSplit1.Count == 0)
		{
            playerHand.Add(DrawCard(PlayerHandStart));
            playerHand[playerHand.Count - 1].transform.position += Vector3.right * ((playerHand.Count - 1) * 50);
		}

        if (Score(playerHand) == -1)
		{
            Hold();
        }
    }

    public void Split()
	{
        if (playerSplit1.Count != 0 && playerSplit1[0].GetComponent<Card>().GetCard() == playerSplit1[1].GetComponent<Card>().GetCard())

        {
            playerSplit2.Add(playerSplit1[1]);
            playerSplit1.RemoveAt(1);
            playerSplit2.Add(DrawCard(PlayerSplit2Start));
            playerSplit1.Add(DrawCard(PlayerSplit1Start));
            playerSplit2[0].transform.position = PlayerSplit2Start.position;
            Player.Get().m_Chips -= bet;
            Beter.Get().UpdateDisplay();

            for (int i = 0; i < playerSplit2.Count; i++)
            {
                playerSplit2[i].transform.position += Vector3.right * (i * 50);
            }

            for (int i = 0; i < playerSplit1.Count; i++)
            {
                playerSplit1[i].transform.position += Vector3.right * (i * 50);
            }
        }
		else if (playerHand.Count == 2 && playerHand[0].GetComponent<Card>().GetCard() == playerHand[1].GetComponent<Card>().GetCard())
		{
            playerSplit1.Add(playerHand[1]);
            playerHand.RemoveAt(1);
            playerHand.Add(DrawCard(PlayerHandStart));
            playerSplit1.Add(DrawCard(PlayerSplit1Start));
            playerSplit1[0].transform.position = PlayerSplit1Start.position;
            Player.Get().m_Chips -= bet;
            Beter.Get().UpdateDisplay();

            for (int i = 0; i < playerSplit1.Count; i++)
            {
                playerSplit1[i].transform.position += Vector3.right * (i * 50);
            }

            for (int i = 0; i < playerHand.Count; i++)
            {
                playerHand[i].transform.position += Vector3.right * (i * 50);
            }
        }
	}

    public void DoubleDown()
	{
        if (playerHand.Count <= 2 && playerSplit1.Count == 0)
		{
            Player.Get().m_Chips -= bet;
            Beter.Get().UpdateDisplay();
            bet *= 2;
            playerHand.Add(DrawCard(PlayerHandStart));
            playerHand[playerHand.Count - 1].transform.position += Vector3.right * ((playerHand.Count - 1) * 50);
            Hold();
		}
    }

    public void Hold()
	{
        isPlaying = false;

        dealerHand[0].GetComponent<Card>().SetBack(false);

        while (Score(dealerHand) != -1 && Score(dealerHand) < 17)
		{
            dealerHand.Add(DrawCard(DealerHandStart));
            dealerHand[dealerHand.Count-1].transform.position += Vector3.right * ((dealerHand.Count-1) * 50);

        }

        Debug.Log(Score(playerHand));
        Debug.Log(Score(dealerHand));
        
        if (Score(playerHand) > Score(dealerHand))
		{
            if (playerHand.Count == 2 && Score(playerHand) == 21)
			{
                Player.Get().m_Chips += bet * 4;
			}
            else
			{
                Player.Get().m_Chips += bet * 2;
			}
            Beter.Get().UpdateDisplay();
		}

        if (playerSplit1.Count != 0)
		{
            if (Score(playerSplit1) > Score(dealerHand))
            {
                if (playerSplit1.Count == 2 && Score(playerSplit1) == 21)
                {
                    Player.Get().m_Chips += bet * 4;
                }
                else
                {
                    Player.Get().m_Chips += bet * 2;
                }
                Beter.Get().UpdateDisplay();
            }
        }

        if (playerSplit2.Count != 0)
        {
            if (Score(playerSplit2) > Score(playerSplit2))
            {
                if (playerSplit2.Count == 2 && Score(playerHand) == 21)
                {
                    Player.Get().m_Chips += bet * 4;
                }
                else
                {
                    Player.Get().m_Chips += bet * 2;
                }
                Beter.Get().UpdateDisplay();
            }
        }

        backBtn.interactable = true;

        bet = 0;
        BetAmountText.text = string.Format("{0:n0}", bet);
	}

    public int Score(List<GameObject> cardHand)
	{
        List<Card> aces = new List<Card>();

        int score = 0;
        foreach(var card in cardHand)
		{
            int addedScore = card.GetComponent<Card>().GetCard();
            if (addedScore > 10)
			{
                addedScore = 10;
			}
            else if (addedScore == 1)
			{
                addedScore = 11;
                aces.Add(card.GetComponent<Card>());
			}
            score += addedScore;
		}

        if (score > 21 && aces.Count != 0)
		{
            int i = 0;
            while (score > 21 && i < aces.Count)
            {
                score -= 10;
                i++;
            }
        }

        if (score > 21) score = -1;

        return score;
	}
}
