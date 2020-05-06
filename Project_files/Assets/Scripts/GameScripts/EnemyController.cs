using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health;
    public float damage;

    public GameObject[] droppableItem;
    public GameObject player;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("weapon"))
        {
            health -= playerController.damage;

            if (health <= 0)
            {
                GameObject drop = Instantiate(droppableItem[Random.Range(0,droppableItem.Length)]);
                drop.transform.position = transform.position;
                Destroy(gameObject);
            }
        }
    }

}
