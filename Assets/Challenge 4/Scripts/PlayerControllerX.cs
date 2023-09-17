using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    private float normalSpeed = 700; // Normal speed
    private float boostedSpeed = 1200; // Speed when space key is held down
    private GameObject focalPoint;

    public ParticleSystem smokeParticle;

    public bool hasPowerup = false;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;

    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup

    private bool isBoosting = false; // Track if the space key is currently held

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isBoosting = true; // The space key is being held down
            smokeParticle.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isBoosting = false; // The space key is not being held down
            smokeParticle.Stop();
        }

        if (isBoosting)
        {
            // Add force to player in direction of the focal point (and camera) with boosted speed
            playerRb.AddForce(focalPoint.transform.forward * verticalInput * boostedSpeed * Time.deltaTime);
        }
        else
        {
            // Add force to player in direction of the focal point (and camera) with normal speed
            playerRb.AddForce(focalPoint.transform.forward * verticalInput * normalSpeed * Time.deltaTime);
        }

        // Set powerup indicator position to beneath player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);
    }

    // Rest of your code...

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - playerRb.transform.position;

            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }
        }
    }
}
