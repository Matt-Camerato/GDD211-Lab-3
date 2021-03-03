using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardEnemy : Enemy, IEntity
{
    [SerializeField] private GameObject magicBall; //<--this is prefab for projectile that wizard throws at enemy

    private float attackCooldown = 0; //<--not all enemies have a cooldown, so this is specific to wizard enemy

    private bool inRange; //<--this determines if wizard is close enough to player to fire projectile

    public override void Update()
    {
        //wizard enemy stops a certain distance from the player to fire projectiles, so can completely override base update method

        //wizard enemy still flips to face player
        if (transform.position.x < player.position.x)
        {
            //facing right
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (transform.position.x > player.position.x)
        {
            //facing left
            GetComponent<SpriteRenderer>().flipX = true;
        }

        //first check if player is dead or not
        if (!player.GetComponent<PlayerController>().dead)
        {
            if(Vector3.Distance(transform.position, player.position) > 4)
            {
                //if not in range, move towards player
                transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                //if in range, attack player
                Attack(player.gameObject);
            }
                
        }

        //additionaly, wizard enemy has to handle attack cooldown
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    public override void OnCollisionStay2D(Collision2D collision)
    {
        //this will be left blank since wizard doesn't attack when touching player
    }

    public override void Attack(GameObject player)
    {
        //wizard enemy checks if cooldown is 0 before attacking
        if (attackCooldown <= 0)
        {
            //instead of directly attacking player, wizard spawns magic projectile and sends it at player's current position
            var newMagicBall = Instantiate(magicBall, transform.position, Quaternion.identity);
            newMagicBall.GetComponent<MagicBall>().targetPosition = player.transform.position;
            newMagicBall.GetComponent<MagicBall>().attackDamage = attackDamage;

            //if attack is successful, attack cooldown is reset
            attackCooldown = 2f;
        }
    }

    public void Damaged(int damageAmount)
    {
        GetComponent<Animator>().SetTrigger("damaged");

        health -= damageAmount;
        if (health <= 0)
        {
            //enemy is killed and death particles appear
            Instantiate(deathParticles, transform.position, Quaternion.identity);

            //wizard enemy drops 3 coins and has 25% chance to drop health pickup upon death
            for (int i = 0; i < 3; i++)
            {
                var newCoin = Instantiate(coinDrop, transform.position, Quaternion.identity);
                newCoin.GetComponent<CoinDrop>().player = player.GetComponent<PlayerController>();
                newCoin.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * 150);
            }

            if (Random.Range(0f, 1f) < 0.25f)
            {
                var newHealth = Instantiate(healthDrop, transform.position, Quaternion.identity);
                newHealth.GetComponent<HealthDrop>().player = player.GetComponent<PlayerController>();
                newHealth.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * 150);
            }

            Destroy(gameObject);
        }
        else
        {
            //wizard enemy has damage particles since they don't die in one hit
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
    }


}
