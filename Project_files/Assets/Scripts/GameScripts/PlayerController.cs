using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public Transform groundCheck;
    public GameObject attackHitBox;
    
    public List<string> inventory;

    public GameObject enemyObject;
    private EnemyController enemy;

    public float playerHealth;
    public int damage;
    public int playerMaxHealth = 3;
    private int coins;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isGrounded;
    private Transform playerPos;
    private bool isAttacking = false;


    void Start()
    {
        inventory = new List<string>();
        rb = GetComponent<Rigidbody2D>();
        playerPos = GetComponent<Transform>();
        attackHitBox.SetActive(false);
        animator = GetComponent<Animator>();
        enemy = enemyObject.GetComponent<EnemyController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            isAttacking = true;
            animator.Play("Torchman_attack");
            StartCoroutine(DoAttack());
        }
    }

    IEnumerator DoAttack()
    {
        attackHitBox.SetActive(true);
        yield return new WaitForSeconds(.4f);
        attackHitBox.SetActive(false);
        isAttacking = false;
    }

    void FixedUpdate()
    {
        if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }


        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            rb.velocity = new Vector2(speed * Time.fixedDeltaTime, rb.velocity.y);
            if (!isAttacking && isGrounded)
            {
                animator.Play("Torchman_walk");
            }

            transform.localScale = new Vector2(1, 1);

        }
        else if (Input.GetKey("a") || Input.GetKey("left"))
        {
            rb.velocity = new Vector2(-speed * Time.fixedDeltaTime, rb.velocity.y);
            if (!isAttacking && isGrounded)
            {
                animator.Play("Torchman_walk");
            }

            transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            if (!isAttacking && isGrounded)
            {
                animator.Play("Torchman_idle");
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("weapon"))
        {
            print(enemy.damage);
            playerHealth -= enemy.damage;

            if (playerHealth <= 0)
            {
                Destroy(gameObject);
                SceneManager.LoadScene("GameOver");
            }
        }

        if (collision.gameObject.CompareTag("Collectable"))
        {
            string itemType = collision.gameObject.GetComponent<CollectableScript>().itemType;
            int plusHealth = collision.gameObject.GetComponent<CollectableScript>().plusHealth;
            int coinScore = collision.gameObject.GetComponent<CollectableScript>().coinScore;
            print("we have collected a" + itemType);

            if (itemType == "hp")
            {
                if (playerMaxHealth > playerHealth)
                {
                    playerHealth += plusHealth;
                    inventory.Add(itemType);
                    Destroy(collision.gameObject);
                }
            }
            else if (itemType == "coin")
            {
                coins += coinScore;
                inventory.Add(itemType);
                Destroy(collision.gameObject);

                if (coins == 10)
                {
                    damage += 1;
                }
            }

            print("Inventory lenght: " + inventory.Count);
            print("Current player health: " + playerHealth);
            print("Current players coins: " + coins);
        }
    }

}
