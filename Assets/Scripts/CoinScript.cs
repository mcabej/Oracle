using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinScript : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D col)
    {
        ScoreTextScript.amount += 1;
        col.gameObject.SetActive(false);
    }
}
