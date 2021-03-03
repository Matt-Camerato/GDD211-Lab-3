using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenEnemy : Enemy, IEntity
{
    private float attackCooldown = 0; //<--not all enemies have a cooldown, so this is specific to green enemy

    public override void Update()
    {
        base.Update(); //<--enemy moves toward player

        //additionaly, green enemy has to handle attack cooldown
        if(attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    public override void Attack(GameObject player)
    {
        //green enemy checks if cooldown is 0 before attacking
        if (attackCooldown <= 0)
        {
            base.Attack(player); //<--attacks player with given attack damage

            //if attack is successful, attack cooldown is reset
            attackCooldown = 1.5f;
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

            //green enemy drops 1 to 2 coins and has 15% chance to drop health pickup upon death
            int numCoins = Random.Range(1, 3);
            for (int i = 0; i < numCoins; i++)
            {
                var newCoin = Instantiate(coinDrop, transform.position, Quaternion.identity);
                newCoin.GetComponent<CoinDrop>().player = player.GetComponent<PlayerController>();
                newCoin.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * 150);
            }

            if (Random.Range(0f, 1f) < 0.15f)
            {
                var newHealth = Instantiate(healthDrop, transform.position, Quaternion.identity);
                newHealth.GetComponent<HealthDrop>().player = player.GetComponent<PlayerController>();
                newHealth.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * 150);
            }

            Destroy(gameObject);
        }
        else
        {
            //green enemy has damage particles since they don't die in one hit
            transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
    }


}
