using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 15;
    private float xBound = 18;
    private float zBoundBottom = -27;
    private float zBoundTop = -10;
    private float horizontalInput;
    private float verticalInput;

    private float bounceOffForce = 3;  //enemy bounced off's force
    private GameManager gameManager;
    private int score;
    public bool hasPowerup;  //check if the player gets power
    public GameObject powerUpIndicator;
    public ParticleSystem explosionParticle;
    public ParticleSystem energyParticle;
    public AudioSource playerAudio;
    public AudioClip getEnergySound;
    public AudioClip carCrashSound;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        //playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        ConstrainPlayerPosition();

        //set powerup indicator's position (follow the player)
        powerUpIndicator.transform.position = transform.position + new Vector3(0, 1.66f, -0.47f);
    }

    //move the player by using arrow key input
    void MovePlayer()
    {
        //allow user to move the player by using keyboards
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        //change the player's movement
        transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);
    }

    //keep the player within the scene
    void ConstrainPlayerPosition()
    {
        if (transform.position.x > xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
        }

        if (transform.position.x < -xBound)
        {
            transform.position = new Vector3(-xBound, transform.position.y, transform.position.z);
        }

        if (transform.position.z < zBoundBottom)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBoundBottom);
        }

        if (transform.position.z > zBoundTop)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBoundTop);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //if the player gets power
        if (other.CompareTag("Powerup"))
        {
            //gas station will disappear
            playerAudio.PlayOneShot(getEnergySound, 1.0f);
            Destroy(other.gameObject);
            Instantiate(energyParticle, other.transform.position, explosionParticle.transform.rotation);
            hasPowerup = true;
            powerUpIndicator.SetActive(true);
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        //if the player has power and collides with the enemy
        if (hasPowerup == true && collision.gameObject.CompareTag("Enemy"))
        {
            //enemy will be destroyed
            playerAudio.PlayOneShot(carCrashSound, 1.0f);
            Destroy(collision.gameObject);
            Instantiate(explosionParticle, collision.transform.position, explosionParticle.transform.rotation);
            score = 5;
            gameManager.UpdateScore(score);


            //after certain seconds, power will be deactivated
            StartCoroutine(PowerupCountdown());
        }
        else if (collision.gameObject.CompareTag("Enemy") && hasPowerup == false)
        {
            //player will be destroyed
            Destroy(gameObject);
            gameManager.GameOver();
        }
    }

    //wait for certain seconds, power will be deactivated
    IEnumerator PowerupCountdown()
    {
        yield return new WaitForSeconds(4);
        hasPowerup = false;
        powerUpIndicator.SetActive(false);

    }
}
