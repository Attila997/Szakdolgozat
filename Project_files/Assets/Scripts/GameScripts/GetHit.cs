using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetHit : MonoBehaviour
{
    bool isShake = false;
    public float shakeAmount;
    private Vector2 currentPos;

    // Update is called once per frame
    void Update()
    {
        currentPos = transform.position;
        if (isShake)
        {
            transform.position = currentPos + Random.insideUnitCircle * shakeAmount;
        }

        if(transform.position.y < -10f)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("GameOver");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "AttackHitBox")
        {
            isShake = true;
            Invoke("StopShake", .1f);
        }
    }

    void StopShake()
    {
        isShake = false;
        transform.position = currentPos;
    }
}
