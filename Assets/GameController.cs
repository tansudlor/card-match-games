using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameController : MonoBehaviour
{
    public GameObject GridLayout;
    public GameObject CardInstance;
    public TextMeshProUGUI ScoreText;
    public CardItem cardItem1;
    public CardItem cardItem2;
    private int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        Deck deck = new Deck(3);
        deck.Shuffle();
        for (int i = 0; i < deck.Cards.Count; i++)
        {
            //Debug.Log("afterShuffle" + deck.Cards[i].Number);
            GameObject card = Instantiate(CardInstance, GridLayout.transform);
            var cardItem = card.GetComponent<CardItem>();
            cardItem.Number = deck.Cards[i].Number.ToString();
            cardItem.OnClickOpen = OnClickDataReciveve;

        }

    }

    private void Update()
    {
        
    }


    void OnClickDataReciveve(CardItem cardItem)
    {

        if (cardItem1 != null && cardItem2 != null)
        {
            return;
        }

        if (cardItem1 == null)
        {
            cardItem1 = cardItem;
            cardItem1.isOpen  = true;
            Debug.Log("cardItem1 " + cardItem1.Number);
            return;
        }

        if (cardItem1 != null)
        {
            cardItem2 = cardItem;
            cardItem2.isOpen = true;
        }

        Debug.Log("cardItem2 " + cardItem2.Number);

        if (cardItem1 == null || cardItem2 == null)
        {
            return;
        }

        CheckMatchCard();
    }



    void CheckMatchCard()
    {
        if (cardItem1.Number == cardItem2.Number)
        {
            score++;
            ScoreText.text = "Score : " + score;
            StartCoroutine(ScoreUp());
        }
        else
        {
            StartCoroutine(WaitForClose());
        }
    }

    IEnumerator WaitForClose()
    {
        yield return new WaitForSeconds(0.75f);
        cardItem1.isOpen = false;
        cardItem2.isOpen = false;
        cardItem1.GetComponent<Animation>().Play("cardFlipClose");
        cardItem2.GetComponent<Animation>().Play("cardFlipClose");
        yield return new WaitForSeconds(0.05f);
        ClearCard();
    }

    IEnumerator ScoreUp()
    {
        yield return new WaitForSeconds(0.75f);
        cardItem1.FrontSide.SetActive(false);
        cardItem2.FrontSide.SetActive(false);
        cardItem1.image.raycastTarget = false;
        cardItem2.image.raycastTarget = false;
        yield return new WaitForSeconds(0.05f);
        ClearCard();
    }

    void ClearCard()
    {
        cardItem1 = null;
        cardItem2 = null;
    }
}
