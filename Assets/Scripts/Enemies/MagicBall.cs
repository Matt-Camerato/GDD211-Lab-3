using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MonoBehaviour
{
    public Vector3 targetPosition;
    public int attackDamage;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 2f * Time.deltaTime); //<--ball moves towards target position when spawned
        if(transform.position == targetPosition)
        {
            Destroy(gameObject); //if ball gets to target position without hitting player, it is destroyed
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            IEntity playerEntity = collision.GetComponent<IEntity>();
            if (playerEntity != null)
            {
                playerEntity.Damaged(attackDamage);
                var knockbackDir = transform.position - collision.transform.position;
                collision.transform.position -= knockbackDir.normalized * 0.4f;
            }

            Destroy(gameObject);
        }
    }
}
