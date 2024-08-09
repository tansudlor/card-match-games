using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject GridLayout;
    public GameObject CardInstance;

    // Start is called before the first frame update
    void Start()
    {
        Deck deck = new Deck(5);
        for (int i = 0; i < deck.Cards.Count; i++)
        {
            Debug.Log("beforeShuffle" + deck.Cards[i].Number);
        }

        deck.Shuffle();
        for (int i = 0; i < deck.Cards.Count; i++)
        {
            Debug.Log("afterShuffle" + deck.Cards[i].Number);

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
