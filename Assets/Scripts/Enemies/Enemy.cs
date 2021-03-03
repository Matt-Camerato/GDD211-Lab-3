using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public GameObject deathParticles;
    public GameObject coinDrop;
    public GameObject healthDrop;

    public int health;
    public int attackDamage;
    public float moveSpeed;

    public virtual void Update()
    {
        //all enemies always move towards player at all times and flip sprite based on walking direction
        if (transform.position.x < player.position.x)
        {
            //facing right
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if(transform.position.x > player.position.x)
        {
            //facing left
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if(!GetComponent<BoxCollider2D>().IsTouching(player.GetComponent<BoxCollider2D>()) && !player.GetComponent<PlayerController>().dead)
        {
            //enemy only moves if not colliding with player already and player isn't dead
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    public virtual void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //enemies call attack when touching player
            Attack(collision.gameObject);
        }
    }

    public virtual void Attack(GameObject player)
    {
        IEntity playerEntity = player.GetComponent<IEntity>();
        if(playerEntity != null)
        {
            //all enemies damage player through IEntity and cause knockback
            playerEntity.Damaged(attackDamage);
            var knockbackDir = transform.position - player.transform.position;
            player.transform.position -= knockbackDir.normalized * 0.4f;
        }
    }
}
