using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public PlayerController player;
    public Image[] hearts;
    public Sprite activeHeart;
    public Sprite inactiveHeart;
    private float currentHealth;
    private int numOfHearts;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        numOfHearts = player.playerMaxHealth;
    }

    private void Update()
    {
        currentHealth = player.playerHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = activeHeart;
            }
            else
            {
                hearts[i].sprite = inactiveHeart;
            }

            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }


}
