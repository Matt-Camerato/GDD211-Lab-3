using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : Item
{
    public override void Collect()
    {
        player.health += 20;
        if(player.health > 100)
        {
            player.health = 100;
        }
        base.Collect();
    }
}
