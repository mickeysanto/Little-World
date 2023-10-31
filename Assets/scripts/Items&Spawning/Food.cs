using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Food : MonoBehaviour
{
    public float hp = 10f; //damage needed to take in order for food to be ate
    public float sustinance = 5f; //amount of points the food replenishes the food bar of Creature that ate it
    public static Action<int> AteFood;

    public void Start()
    {

    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        //if what collided with the food is a Creature's attack hitbox
        if(other.name == "attack")
        {
            GameObject parent = other.transform.parent.gameObject;
            Creature creatureStats = parent.GetComponent<Creature>();
            Player playerComponent = parent.GetComponent<Player>();

            //Creature that hit food gets some food bar replenishment
            creatureStats.currFood += sustinance;
            creatureStats.normalizeBars(false);

            //sets player's food bar with updated numbers
            if(playerComponent != null)
            {
                playerComponent.foodBar.SetHealth(creatureStats.currFood);
            }

            hp -= creatureStats.damage;

            if(hp <= 0)
            {
                Die(playerComponent);
            }
        }
    }

    //what happens when the food ios completely eaten
    protected void Die(Player playerComponent) 
    {
        if(playerComponent != null)
        {
            AteFood?.Invoke(1);
        }

        Destroy(gameObject);
    }
}
