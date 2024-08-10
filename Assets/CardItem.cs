using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardItem : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI Text;
    public string Number;
    public GameObject FrontSide;
    public Image image;
    public Action<CardItem> OnClickOpen;
    public bool isOpen = false;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        Text.text = Number;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isOpen)
        {
            return;
        }
        Debug.Log("Object clicked!");
        this.gameObject.GetComponent<Animation>().Play("cardFlipOpen");
        audioSource.Play();
        OnClickOpen?.Invoke(this);
    }


}
