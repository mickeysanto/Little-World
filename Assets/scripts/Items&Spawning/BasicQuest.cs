using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasicQuest : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI questText;
    private Transform player;

    public GameObject spawnable;
    
    public string text;

    void Start()
    {
        questText = GameObject.Find("Quest").GetComponent<TextMeshProUGUI>();
        questText.SetText("");
        player = GameObject.Find("Player").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name.Equals("playerBody"))
        {
            questText.SetText(text);
        }  

        if(collision.gameObject.name.Equals("peanut"))
        {
            Instantiate(spawnable, new Vector2(player.position.x, player.position.y - 1f), Quaternion.identity).name = "Ally";
            Destroy(gameObject); 
            Destroy(collision.gameObject);
        }    
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("playerBody"))
        {
            questText.SetText("");
        }
    }
}
