using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public PlayerController player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Collect();
        }
    }

    public virtual void Collect()
    {
        //items will override this function, performing their item-specific action before calling the method's base functionality
        Destroy(gameObject);
    }
}
