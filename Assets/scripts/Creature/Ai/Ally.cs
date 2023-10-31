using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : AiCreature
{
    public GameObject player;
    public float followDistance = 5f; //how close to the player the ally follows

    public GameObject attacker; //creature that has tried to attack player
    public float attackerDistance; //distance from attacker to player
    public float targetDistance; //distance from attacker to ally

    public bool isProtecting; //is true if ally is protecting player from attacker

    protected override void Start()
    {
        player = GameObject.Find("playerBody");
        isProtecting = false;

        StartCoroutine(Follow());
        StartCoroutine(Protect());
        StartCoroutine(AttackCooldown());
    }

    protected override void Update()
    {
        distance = Vector3.Distance(player.transform.position, transform.position);
        
        //if there is something attacking the player
        if(attacker != null)
        {   
            isProtecting = true;
            attackerDistance = Vector3.Distance(attacker.transform.position, player.transform.position);
            targetDistance = Vector3.Distance(attacker.transform.position, transform.position);

            if(attackerDistance >= 5.1f)
            {
                attacker = null;
                isProtecting = false;
            }
        }
    }

    private void OnEnable() 
    {
        Player.Attacked += PlayerAttacked;
    }

    private void OnDisable() 
    {
        Player.Attacked -= PlayerAttacked;
    }

    private void PlayerAttacked(GameObject attacker)
    {
        this.attacker = attacker;
    }

    public IEnumerator Follow()
    {
        while(true)
        {
            yield return new WaitUntil(() => !isProtecting);

            if(distance > followDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, movementSpeed * Time.deltaTime);
                LookTo(player.transform.position);
            }
           
        }
    }

    public IEnumerator Protect()
    {
        while(true)
        {
            yield return new WaitUntil(() => isProtecting);

            StartCoroutine(Move());
            
            //if target is in attack range and their attack is not currently on cooldown, attack
            if (attacker != null && targetDistance <= attackRange && !isAttacking  && !inAttackCooldown)
            {
                StartCoroutine(Attack());
            }
        }
    }

        //moves towards target
    public override IEnumerator Move()
    {
        while(isProtecting && attacker != null && !isAttacking && targetDistance > attackRange)
        {
            LookTo(attacker.transform.position);
            transform.position = Vector2.MoveTowards(transform.position, attacker.transform.position, movementSpeed/10 * Time.deltaTime);

            yield return null;
        }
    }

    public override IEnumerator Attack()
    {
        isAttacking = true;
        attackBox.enabled = true;
        LookTo(attacker.transform.position);

        Vector3 ogPosition = attackBox.transform.localPosition;
        Vector3 attackEnd = new Vector3(ogPosition.x, ogPosition.y + attackRange, ogPosition.z);

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

    protected override void Die(Player playerComponent)
    {

    }

    protected override IEnumerator Hunger()
    {
        yield return null;
    }
}
