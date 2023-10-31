using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Creature : MonoBehaviour
{
    public float maxHealth = 100;
    public float maxFood = 100;
    public float currHealth = 100;
    public float currFood = 100;
    public float hungerTime = 5f;
    public float attackRange = 2f; //range in which creature will attack the target
    public float damage;
    public float movementSpeed = 1f; //movement speed of the creature
    public float attackCooldown = 3f; //attack cooldown in seconds
    public float attackSpeed = 1f;
    public bool isAttacking; //is true if creature is currently in an attack
    public bool inAttackCooldown; //is true if creature is currently in an attack cooldown

    public Collider2D attackBox;
    public GameObject corpse;
    public GameObject idMaker;
    public static Action<int> PlayerKill;

    protected virtual void Start()
    {
        currHealth = maxHealth;
        currFood = maxFood;
        isAttacking = false;
        inAttackCooldown = false;
        attackBox.enabled = false;

        StartCoroutine(Hunger());
        StartCoroutine(Starve());
        StartCoroutine(Regen());
        StartCoroutine(AttackCooldown());
    }

    /*protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if(other.name == "attack")
        {
           TakeDamage(other.transform.parent.gameObject.GetComponent<Creature>().damage);
        }
    }*/

    //adds a cooldown for creature attacks
    public IEnumerator AttackCooldown()
    {
        while (true)
        {
            yield return new WaitUntil(() => inAttackCooldown);
            yield return new WaitForSeconds(attackCooldown);

            inAttackCooldown = false;
        }
    }

    public virtual IEnumerator Attack()
    {
        yield return null;
    }

    //if type is true then it adjusts health else it adjusts food
    public void normalizeBars(bool type)
    {
        if(type)
        {
            if(currHealth < 0)
            {
                currHealth = 0;
            }
            else if(currHealth > maxHealth)
            {
                currHealth = maxHealth;
            }
        }
        else
        {
            if(currFood < 0)
            {
                currFood = 0;
            }
            else if(currFood > maxFood)
            {
                currFood = maxFood;
            }
        }
    }

    public virtual void TakeDamage(float enemyDamage) 
    {
        currHealth -= enemyDamage;
        normalizeBars(true);
    }

    protected virtual IEnumerator Hunger() 
    {
        while(true) 
        {
            yield return new WaitForSeconds(hungerTime);

            if(currFood > 0) 
            {
                currFood -= 5;
                normalizeBars(false);
            }
        }
    }

    protected virtual IEnumerator Starve() 
    {
        while(true) 
        {
            yield return new WaitUntil(() => currFood <= 0);

            while(currFood <= 0) 
            {
                yield return new WaitForSeconds(3f);

                currHealth -= 10;
                normalizeBars(true);
            }
        }
    }

    protected virtual IEnumerator Regen() 
    {
        while(true) 
        {
            yield return new WaitUntil(() => currFood >= (maxFood * .7) && currHealth < maxHealth);
            yield return new WaitForSeconds(3f);

            currHealth += 10;
            normalizeBars(true);
        }
    }

    protected virtual void Die(Player playerComponent) 
    {
        if(playerComponent != null)
        {
            if(this.GetComponent<Herbivore>() != null)
            {
                PlayerKill?.Invoke(2);
            }
            else if(this.GetComponent<Carnivore>() != null)
            {
                PlayerKill?.Invoke(3);
            }
        }
        
       GameObject newObj = Instantiate(corpse, transform.position, Quaternion.identity);
       newObj.name = "Corpse" + idMaker.GetComponent<IdMaker>().getID();
       Destroy(gameObject);
    }
}
