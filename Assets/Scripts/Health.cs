﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int NumOfHearts = 3;

    public Image[] Hearts;
    public Sprite FullHeart;
    public Sprite EmptyHeart;
    public GameManagerScript gameManager;

    public void ReduceHealth(int damage)
    {
        NumOfHearts -= damage;
    }
    
    void Update()
    {
        if (NumOfHearts == 0)
        {
            FindObjectOfType<GameManagerScript>().GameOver();
        }

        for (int i = 0; i < Hearts.Length; i++)
        {
            if (i < NumOfHearts)
            {
                Hearts[i].sprite = FullHeart;
            }
            else
            {
                Hearts[i].sprite = EmptyHeart;
            }
        }
    }
}
