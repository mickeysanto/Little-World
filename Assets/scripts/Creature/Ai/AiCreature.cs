using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCreature : Creature
{
    public float aggroRange = 5f; //range in which creature will lock onto target to attack
    public float retreatChance;
    public bool attackPause = false; //enemey will actually stop after attack if true
    public bool isFollowing;
    public bool hasTarget;
    public bool retreating;
    public Collider2D[] targetColliders;

    public Transform target; //the target's transform
    protected string targetName;
    public float distance; //distance from target
    protected Rigidbody2D rb;

    public float range = .35f;
    public float maxDistance = 25f;
    Vector2 wayPoint;

    protected override void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isFollowing = false;
        hasTarget = false;
        retreating = false;
        targetColliders = null;
        target = null;
        setDestination();

        base.Start();

        rollStats(new Vector2(9.5f, 12f), new Vector2(.01f, .015f), new Vector2(2.3f, 3f), new Vector2(5, 8), new Vector2(.5f, 1.1f));

        //StartCoroutine(LookTo());
        StartCoroutine(Idle());
        StartCoroutine(Wander());
    }

    protected virtual void rollStats(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 e)
    {
        Stats stats = new Stats(a, b, c, d, e);
        stats.rollStats();

        damage = stats.damage; //a
        movementSpeed = stats.speed; //b
        attackCooldown = stats.attackCool; //c
        retreatChance = stats.retreatChance; //d
        attackSpeed = stats.attackSpeed; //e
    }



    //defaulted to work with Carnivore 
    protected virtual void Update()
    {
        if(!isFollowing && !inAttackCooldown && !hasTarget && !retreating)
        {
            targetColliders = Physics2D.OverlapCircleAll(transform.position, aggroRange);

            foreach(Collider2D targetCollider in targetColliders)
            {
                if(targetCollider != null && targetCollider.gameObject.name != gameObject.name && targetCollider.gameObject.GetComponent<CreatureType>() != null && currFood < maxFood)
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

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if(other.name == "attack")
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
                retreatRoll();
            }

            //rb.AddForce((rb.transform.position - other.transform.position).normalized * 1f, ForceMode2D.Impulse);
        }
    }

    public void retreatRoll()
    {
        int rollRetreat = (int) UnityEngine.Random.Range(1, 100);

        if(rollRetreat <= retreatChance)
        {
            retreating = true;
            setDestination();
            StartCoroutine(Retreat());
        }
    }

    public IEnumerator Retreat()
    {
        while(transform.position.x != wayPoint.x && transform.position.y != wayPoint.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, wayPoint, 400 * movementSpeed * Time.deltaTime);
            LookTo(new Vector3(wayPoint.x, wayPoint.y, 0));

            yield return null;
        }

        retreating = false;
    }

    public IEnumerator Wander()
    {
        while(true)
        {
            yield return new WaitUntil(() => !hasTarget && !isFollowing && !isAttacking && !retreating);

            transform.position = Vector2.MoveTowards(transform.position, wayPoint, 300 * movementSpeed * Time.deltaTime);
            LookTo(new Vector3(wayPoint.x, wayPoint.y, 0));

            if(Vector2.Distance(transform.position, wayPoint) < range)
            {
                setDestination();
            }
            
        }
    }

    //creates a new waypoit for the creature to move to while wandering
    protected void setDestination() 
    {
        wayPoint = new Vector2(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance));
    }

    public virtual IEnumerator Idle()
    {
        while (true)
        {
            yield return new WaitUntil(() => (distance <= aggroRange && currFood < maxFood && hasTarget && !retreating));
            //Debug.Log("IDLE");

            isFollowing = true;

            StartCoroutine(Move());

            //if target is in attack range and their attack is not currently on cooldown, attack
            if (distance <= attackRange && !isAttacking  && !inAttackCooldown && hasTarget)
            {
                StartCoroutine(Attack());
            }
        }
    }

    //moves towards target
    public virtual IEnumerator Move()
    {
        while(hasTarget && distance <= aggroRange && distance > attackRange && !isAttacking && !retreating)
        {
            if(hasTarget)
            {
                LookTo(target.position);
                transform.position = Vector2.MoveTowards(transform.position, target.position, movementSpeed * Time.deltaTime);
            }

            yield return null;
        }

        isFollowing = false;
    }

    public override IEnumerator Attack()
    {
        isAttacking = true;
        attackBox.enabled = true;
        LookTo(target.position);

        Vector3 ogPosition = attackBox.transform.localPosition;
        Vector3 attackEnd = new Vector3(ogPosition.x, ogPosition.y + (attackRange/8), ogPosition.z);

        for(float i = 0; i < 1; i += Time.deltaTime / attackSpeed)
        {
            attackBox.transform.localPosition = Vector3.Lerp(attackBox.transform.localPosition, attackEnd, i);
            yield return null;
        }
        
        attackBox.enabled = false;
        attackBox.transform.localPosition = ogPosition;
        isAttacking = false;
        inAttackCooldown = true;
    }

    //makes creature look at target
    public void LookTo(Vector3 target)
    {
        /*while(true)
        {
            //yield return new WaitUntil(() => (distance <= aggroRange && currFood < maxFood && hasTarget == true));
            transform.up = target.position - transform.position;
        }*/

        transform.up = target - transform.position;
    }
}
