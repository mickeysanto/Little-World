using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    public GameObject item;
    private Transform player;
    public string name;

    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    public void SpawnDroppedItem()
    {
        Vector2 playerpos = new Vector2(player.position.x, player.position.y - 1f);
        Instantiate(item, playerpos, Quaternion.identity).name = name;
    }
}
