using UnityEngine;
using System.Collections;

public class TargetScript : MonoBehaviour
{
    // Used to check if the target has been hit
    public bool isHit = false;
    public int health;
    public int maxHealth;
    public Animator anim;
    public Transform player;  // Reference to the player's position
    public float moveSpeed = 3f; // Speed at which the enemy moves towards the player
    public float attackRange = 1.5f; // Distance at which the enemy can attack
    public float detectionRange = 10f; // Distance at which the enemy detects the player
    public float attackCooldown = 1f; // Time between attacks
    private bool canAttack = true;

    private void Start()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;  // Automatically find the player by its tag
    }

    private void Update()
    {
        // If the target is hit
        if (isHit)
        {
            damageHealth(1);
            isHit = false;
        }

        // Check if the player is within detection range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange && health > 0)
        {
            // Move towards the player if within detection range
            MoveTowardsPlayer();

            // Attack if in range
            if (distanceToPlayer <= attackRange && canAttack)
            {
                StartCoroutine(Attack());
            }
        }
        else
        {
            // Keep the enemy idle if the player is out of detection range
            anim.SetBool("isWalking", false);
        }
    }

    private void MoveTowardsPlayer()
    {
        // Calculate direction to the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Move towards the player
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Set walking animation based on distance to player
        anim.SetBool("isWalking", true);
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        anim.SetTrigger("attack1"); // Play attack animation (assumes "attack1" exists in the Animator)
        // Add any logic for dealing damage to the player here

        yield return new WaitForSeconds(attackCooldown); // Wait for the cooldown before allowing another attack
        canAttack = true;
    }

    public void damageHealth(int h)
    {
        health -= h;

        if (health <= 0)
        {
            anim.SetTrigger("death"); // Play death animation when health is zero
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        // Wait for the death animation to finish
        yield return new WaitForSeconds(2f); // Adjust time according to your animation length
        gameObject.SetActive(false); // Hide the enemy after the death animation
    }
}