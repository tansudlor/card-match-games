using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    public List<CardData> Cards = new List<CardData>();

    private int pairs;
    private int[] numberArray;
    public Deck(int pairs)
    {
        this.pairs = pairs;
        GenerateDeck();

    }

    private void GenerateDeck()
    {
        numberArray = new int[pairs];

        for (int i = 0; i < pairs; i++)
        {
            numberArray[i] = i + 1;
        }

        foreach (var number in numberArray)
        {
            CardData card = new CardData();
            card.Number = number;
            Cards.Add(card);
            Cards.Add(card);
        }
    }

    public void Shuffle()
    {

        for (int i = 0; i < pairs*2; i++)
        {
            int r1 = Random.Range(0, Cards.Count);
            int r2 = Random.Range(0, Cards.Count);
            (Cards[r1], Cards[r2]) = (Cards[r2], Cards[r1]);
        }

    }
}




public class CardData
{
    public int Number { get; set; }
}
