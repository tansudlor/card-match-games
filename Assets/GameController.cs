using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject GridLayout;
    public GameObject CardInstance;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI Turn;
    public TextMeshProUGUI Match;
    public CardItem cardItem1;
    public CardItem cardItem2;
    public TMP_InputField Row;
    public TMP_InputField Column;
    public TMP_InputField Pair;
    public AudioSource audioSource;
    public AudioClip Correct;
    public AudioClip Mismatch;
    public AudioClip GameOver;
    private int score = 0;
    private int turn = 0;
    private int match = 0;
    private int gameOverScore = 0;

    public int[] CardDataID;

    // Start is called before the first frame update
    void Start()
    {
        StartGame(true);
        Pair.text = "4";
    }

    public void StartGame(bool start = false)
    {
        match++;
        Match.text = match.ToString();

        ClearBoard();
        // var cardNum = int.Parse(Row.text) + int.Parse(Column.text);
        // Deck deck = new Deck(cardNum / 2);
        var cardNum = 0;
        if (start == true) 
        {
            cardNum = 4;

        }
        else
        {
            cardNum = int.Parse(Pair.text);
        }
          
        if(cardNum > 26)
        {
            cardNum = 26;
        }
        Deck deck = new Deck(cardNum);
        gameOverScore = cardNum ;
        deck.Shuffle();
        var constraintCountGrid = 13;
        /*if (int.Parse(Row.text) > int.Parse(Column.text))
        {
            constraintCountGrid = int.Parse(Row.text);
        }
        else
        {
            constraintCountGrid = int.Parse(Column.text);
        }*/
        GridLayout.GetComponent<GridLayoutGroup>().constraintCount = constraintCountGrid;

        CreateCard(deck.Cards);
    }

    void ClearBoard()
    {
        for (int i = 0; i < GridLayout.transform.childCount; i++)
        {
            Destroy(GridLayout.transform.GetChild(i).gameObject);
        }
    }

    void CreateCard(List<CardData> Cards)
    {
        CardDataID = new int[Cards.Count];
        for (int i = 0; i < Cards.Count; i++)
        {
            GameObject card = Instantiate(CardInstance, GridLayout.transform);
            var cardItem = card.GetComponent<CardItem>();
            cardItem.Number = Cards[i].Number.ToString();
            cardItem.OnClickOpen = OnClickDataReciveve;
            CardDataID[i] = Cards[i].Number;
        }
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
            cardItem1.isOpen = true;
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
            ScoreText.text = score.ToString();
            audioSource.clip = Correct;
            audioSource.Play();
            StartCoroutine(ScoreUp());
        }
        else
        {
            audioSource.clip = Mismatch;
            audioSource.Play();
            StartCoroutine(WaitForClose());
        }
        turn++;
        Turn.text = turn.ToString();
        if (gameOverScore == score)
        {
            audioSource.clip = GameOver;
            audioSource.Play();
            for (int i = 0; i < GridLayout.transform.childCount; i++)
            {
                Destroy(GridLayout.transform.GetChild(i).gameObject);
            }
        }
    }

    IEnumerator WaitForClose()
    {
        CardItem cardTemp1 = cardItem1;
        CardItem cardTemp2 = cardItem2;
        ClearCard();
        Debug.Log(cardTemp1.Number);
        Debug.Log(cardTemp2.Number);
        yield return new WaitForSeconds(0.75f);
        cardTemp1.isOpen = false;
        cardTemp2.isOpen = false;
        cardTemp1.audioSource.Play();
        cardTemp2.audioSource.Play();
        cardTemp1.GetComponent<Animation>().Play("cardFlipClose");
        cardTemp2.GetComponent<Animation>().Play("cardFlipClose");
        yield return new WaitForSeconds(0.05f);

    }

    IEnumerator ScoreUp()
    {
        CardItem cardTemp1 = cardItem1;
        CardItem cardTemp2 = cardItem2;
        ClearCard();
        Debug.Log(cardTemp1.Number);
        Debug.Log(cardTemp2.Number);
        yield return new WaitForSeconds(0.75f);
        cardTemp1.FrontSide.SetActive(false);
        cardTemp2.FrontSide.SetActive(false);
        cardTemp1.image.raycastTarget = false;
        cardTemp2.image.raycastTarget = false;
        yield return new WaitForSeconds(0.05f);

    }

    void ClearCard()
    {
        cardItem1 = null;
        cardItem2 = null;
    }

    public void Save()
    {
        var save = string.Join(",", CardDataID) + $":{turn},{match},{score}";
        PlayerPrefs.SetString("save", save);
        Debug.Log(save);
    }

    public void Load()
    {
        var load = PlayerPrefs.GetString("save");
        if (!string.IsNullOrEmpty(load))
        {
            var loadData = load.Split(":");
            int[] cardData = loadData[0].Split(',').Select(int.Parse).ToArray();
            int[] param = loadData[1].Split(',').Select(int.Parse).ToArray();
            Debug.Log("cardData.Length " + cardData.Length);
            List<CardData> cardDatas = new List<CardData>();
            for (int i = 0; i < cardData.Length; i++)
            {
                CardData cardData1 = new CardData();
                cardData1.Number = cardData[i];
                cardDatas.Add(cardData1);
            }

            ClearBoard();
            CreateCard(cardDatas);

            Turn.text = param[0].ToString();
            Match.text = param[1].ToString();
            ScoreText.text = param[2].ToString();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
