using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : Creature
{   
    public HealthBar healthBar;
    public HealthBar foodBar;
    public GameObject attacker; //creature that has tried to attack player
    public float attackerDistance; //distance from attacker to player
    public static Action<GameObject> Attacked;

    protected override void Start()
    {
        base.Start();
        attacker = null;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("AAA");
        GameObject other = collision.gameObject;

        if(other.name == "attack")
        {
            GameObject parent = other.transform.parent.gameObject;

            if(attacker == null)
            {
                attacker = parent;
                Attacked?.Invoke(attacker);
            }

            TakeDamage(parent.GetComponent<Creature>().damage);

            if(currHealth <= 0)
            {
                Die(this.GetComponent<Player>());
            }
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && !inAttackCooldown && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        //if there is something attacking the player
        if(attacker != null)
        {   
            attackerDistance = Vector3.Distance(attacker.transform.position, transform.position);

            if(attackerDistance >= 5.1f)
            {
                attacker = null;
            }
        }
    }

    public override IEnumerator Attack()
    {
        isAttacking = true;
        attackBox.enabled = true;

        Vector3 ogPosition = attackBox.transform.localPosition;
        Vector3 attackEnd = new Vector3(ogPosition.x, ogPosition.y + attackRange, ogPosition.z);

        for(float i = 0; i < 1; i += Time.deltaTime / attackSpeed)
        {
            attackBox.transform.localPosition = Vector3.Lerp(attackBox.transform.localPosition, attackEnd, i);
            yield return null;

            if(attackBox.transform.localPosition == attackEnd)
            {
                break;
            }
        }
        
        attackBox.enabled = false;
        attackBox.transform.localPosition = ogPosition;
        isAttacking = false;
        inAttackCooldown = true;
    }

    public override void TakeDamage(float damage) 
    {
        currHealth -= damage;
        normalizeBars(true);
        healthBar.SetHealth(currHealth);
    }

    protected override IEnumerator Hunger() 
    {
        while(true) 
        {
            yield return new WaitForSeconds(4f);

            if(currFood > 0) 
            {
                currFood -= 10;
                normalizeBars(false);
                foodBar.SetHealth(currFood);
            }
        }
    }

    protected override IEnumerator Starve() 
    {
        while(true) 
        {
            yield return new WaitUntil(() => currFood <= 0);

            while(currFood <= 0) 
            {
                yield return new WaitForSeconds(3f);

                currHealth -= 10;
                normalizeBars(true);
                healthBar.SetHealth(currHealth);
            }
        }
    }

    protected override IEnumerator Regen() 
    {
        while(true) 
        {
            yield return new WaitUntil(() => currFood >= (maxFood * .7) && currHealth < maxHealth);
            yield return new WaitForSeconds(3f);

            currHealth += 10;
            normalizeBars(true);
            healthBar.SetHealth(currHealth);
        }
    }

    protected override void Die(Player playerComponent) 
    {

    }
}
