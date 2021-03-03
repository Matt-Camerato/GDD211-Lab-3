using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinDrop : Item
{
    public override void Collect()
    {
        player.coinCount++;
        base.Collect();
    }
}
