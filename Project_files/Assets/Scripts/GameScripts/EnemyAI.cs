using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed;
    public Transform player;
	public float agroRange;
	public Transform castPoint;
	public Transform groundDetection;
	public float groundCheckDistance;
	
	public GameObject attackHitBox;
	private bool isAttacking = false;
	
	private Animator animator; 
	private Rigidbody2D rb;
	private bool isFacingLeft = false;
	private bool isAgro = false;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		attackHitBox.SetActive(false);
	}

	void Update()
    {
		if (CanSeePlayer(agroRange))
		{
			isAgro = true;
		}
		else
		{
			if (isAgro)
			{
				Invoke("StopChasingPlayer", 0.15f);
			}
			else
			{
				Patrol();
			}
		}

		if (isAgro)
		{
			ChasePlayer();
		}
    }

	bool CanSeePlayer(float distance)
	{
		bool val = false;
		var castDist = distance;
		
		if(isFacingLeft)
		{
			castDist = -distance;
		}

		Vector2 endPos = castPoint.position + Vector3.right * castDist;

		RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Action"));

		if (hit.collider != null)
		{
			if (hit.collider.gameObject.CompareTag("Player"))
			{
				val = true;
			}
			else
			{
				val = false;
			}
			Debug.DrawLine(castPoint.position, endPos, Color.red);

		}
		else
		{
		Debug.DrawLine(castPoint.position, endPos, Color.blue);
		}

		return val;
	}

	void ChasePlayer()
	{
		float playerEnemyDis = transform.position.x - player.position.x;
		
		if (playerEnemyDis < 0 && playerEnemyDis > -0.7f)
		{
			rb.velocity = new Vector2(0, 0);
			transform.eulerAngles = new Vector3(0, 0, 0);
			isFacingLeft = false;
			
			if (!isAttacking)
			{
				isAttacking = true;
				animator.Play("attack");
				StartCoroutine(DoAttack());
			}
		}
		else if (playerEnemyDis < 0)
		{
			rb.velocity = new Vector2(speed, 0);
			transform.eulerAngles = new Vector3(0, 0, 0);
			isFacingLeft = false;
		}
		else if (playerEnemyDis > 0 && playerEnemyDis < 0.7f)
		{
			rb.velocity = new Vector2(0, 0);
			transform.eulerAngles = new Vector3(0, -180, 0);
			isFacingLeft = true;

			if (!isAttacking)
			{
				isAttacking = true;
				animator.Play("attack");
				StartCoroutine(DoAttack());
			}
		}
		
		else if (playerEnemyDis > 0)
		{
			rb.velocity = new Vector2(-speed, 0);
			transform.eulerAngles = new Vector3(0, -180, 0);
			isFacingLeft = true;
		} 
	}

	IEnumerator DoAttack()
	{
		attackHitBox.SetActive(true);
		yield return new WaitForSeconds(1.2f);
		attackHitBox.SetActive(false);
		isAttacking = false;
		animator.Play("idle");
	}

	void StopChasingPlayer()
	{
		isAgro = false;
		animator.Play("walk");
	}

	void Patrol()
	{
		if (isFacingLeft == false)
		{
			rb.velocity = new Vector2(speed / 2, 0);
		}
		else
		{
			rb.velocity = new Vector2(-speed / 2, 0);
		}

		RaycastHit2D groundHit = Physics2D.Raycast(groundDetection.position, Vector2.down,2.0f);
		if(groundHit.collider == false)
		{
			if (isFacingLeft == false)
			{
				
				transform.eulerAngles = new Vector3(0, -180, 0);
				isFacingLeft = true;
			} 
			else
			{
				transform.eulerAngles = new Vector3(0, 0, 0);
				isFacingLeft = false;
			}
		}
		Debug.DrawLine(groundDetection.position, groundDetection.position + Vector3.down * groundCheckDistance, Color.green);
	}
}
