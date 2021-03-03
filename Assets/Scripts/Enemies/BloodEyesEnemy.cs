using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEyesEnemy : Enemy, IEntity
{
    public override void Attack(GameObject player)
    {
        base.Attack(player); //<--attacks player with given attack damage

        //blood eyes enemy dies upon attacking once
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void Damaged(int damageAmount)
    {
        //blood eyes enemy is killed in one hit and death particles appear
        Instantiate(deathParticles, transform.position, Quaternion.identity);

        //blood eyes enemy drops 3 to 5 coins and has 40% chance to drop health pickup upon death
        int numCoins = Random.Range(3, 6);
        for(int i = 0; i < numCoins; i++)
        {
            var newCoin = Instantiate(coinDrop, transform.position, Quaternion.identity);
            newCoin.GetComponent<CoinDrop>().player = player.GetComponent<PlayerController>();
            newCoin.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * 150);
        }

        if(Random.Range(0f, 1f) < 0.40f)
        {
            var newHealth = Instantiate(healthDrop, transform.position, Quaternion.identity);
            newHealth.GetComponent<HealthDrop>().player = player.GetComponent<PlayerController>();
            newHealth.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * 150);
        }

        Destroy(gameObject);
    }
}
