using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUp : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI pickUpText;

    public Inventory inventory;
    private bool pickUpAllowed;
    public GameObject itemButton;

	private void Start () {
        pickUpText = GameObject.Find("PickUp").GetComponent<TextMeshProUGUI>();
        pickUpText.SetText("");
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
	}
	
	private void Update () {
        if(pickUpAllowed && Input.GetKeyDown(KeyCode.E))
        {
            Pickup();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("playerBody"))
        {
            pickUpText.SetText("Press 'E' to pick up");
            pickUpAllowed = true;
        }        
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("playerBody"))
        {
            pickUpText.SetText("");
            pickUpAllowed = false;
        }
    }

    private void Pickup()
    {
        for(int i = 0; i < inventory.slots.Length; i++)
        {
            if(inventory.isFull[i] == false)
            {
                inventory.isFull[i] = true;
                Instantiate(itemButton, inventory.slots[i].transform, false);
                Destroy(gameObject);
                break;
            }
        }
    }

}
