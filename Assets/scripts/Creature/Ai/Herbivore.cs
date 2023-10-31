using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herbivore : AiCreature
{

    protected override void Start()
    {
        base.Start();

        rollStats(new Vector2(9.5f, 12f), new Vector2(.008f, .01f), new Vector2(2.3f, 3f), new Vector2(45, 65), new Vector2(.5f, 1.1f));
    }

    protected override void Update()
    {
        if(!hasTarget && !isFollowing && !inAttackCooldown && !retreating)
        {
            targetColliders = Physics2D.OverlapCircleAll(transform.position, aggroRange);

            foreach(Collider2D targetCollider in targetColliders)
            {
                if(targetCollider != null && targetCollider.gameObject.name != gameObject.name && targetCollider.gameObject.GetComponent<Plant>() != null && currFood < maxFood)
                {
                    //Debug.Log("new: " + targetCollider);
                    hasTarget = true;
                    target = targetCollider.transform;
                    targetName = target.gameObject.name;

                    break;
                }
                else
                {
                    hasTarget = false;
                    target = null;
                }
            }
        }

        if(hasTarget)
        {
            if(GameObject.Find(targetName) == null)
            {
                hasTarget = false;
                target = null;
                isFollowing = false;
                inAttackCooldown = false;
            }
            else
            {
                distance = Vector3.Distance(target.position, transform.position);

                if(distance > aggroRange)
                {
                    hasTarget = false;
                    target = null;
                }
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if(other.name == "attack" )
        {
            GameObject parent = other.transform.parent.gameObject;
            Creature creatureStats = parent.GetComponent<Creature>();
            Player playerComponent = parent.GetComponent<Player>();

            TakeDamage(creatureStats.damage);

            if(currHealth <= 0)
            {
                Die(playerComponent);
                return;
            }

            if(!retreating)
            {
                retreatRoll(other.transform.parent.gameObject);
            }

            //rb.AddForce((rb.transform.position - other.transform.position).normalized * 1f, ForceMode2D.Impulse);
        }
    }

    /*public override IEnumerator Idle()
    {
        while (true)
        {
            yield return new WaitUntil(() => (distance <= aggroRange && currFood < maxFood && hasTarget && !retreating));
            //Debug.Log("IDLE");

            isFollowing = true;

            StartCoroutine(Move());

            //if target is in attack range and their attack is not currently on cooldown, attack
            /*if (distance <= attackRange && isAttacking != true && hasTarget == true)
            {
                isAttacking = true;
                StartCoroutine(Attack());

            } */
        //}
    //}

    public void retreatRoll(GameObject enemy)
    {
        int rollRetreat = (int) UnityEngine.Random.Range(1, 100);

        if(rollRetreat <= retreatChance)
        {
            retreating = true;
            setDestination();
            StartCoroutine(Retreat());
        }
        else if(enemy.GetComponent<Herbivore>() == null)
        {
            hasTarget = true;
            target = enemy.transform;
            targetName = enemy.name;
        }
    }

    //moves towards target
    /*public override IEnumerator Move()
    {
        //Debug.Log("MOVED");
        while (hasTarget && distance <= aggroRange && !isAttacking && !retreating)
        {
            LookTo(target.position);
            transform.position = Vector2.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);

            //Vector2 motion = (player.position - transform.position).normalized;
            //rb.MovePosition(transform.position + (motion * Time.deltaTime * movementSpeed));

            yield return null;
        }

        isFollowing = false;
    }*/
}
