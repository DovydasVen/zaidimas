using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject attackEffect;
    public float maxHealth = 100;
    public float currentHealth;
    public Transform target;  // The player
    public float moveSpeed = 5.5f;
    public float damage = 5f;
    public float xpAwarded = 2;
    public float cooldown = 0.3f;
    public int difficulty;
    private float timeStamp;
    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rigidBody =  GetComponent<Rigidbody>();
        timeStamp = Time.time;
        difficulty = GameObject.Find("GameController").GetComponent<GameController>().difficulty;
        moveSpeed = moveSpeed * (1 + 0.05f * difficulty);
        damage = damage * (1 + 0.1f * difficulty);
        maxHealth = maxHealth * (1 + 0.1f * difficulty);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movePosition = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.fixedDeltaTime);
        rigidBody.MovePosition(movePosition);
        transform.LookAt(target);
        if(currentHealth <= 0){
            target.GetComponent<PlayerController>().xp = target.GetComponent<PlayerController>().xp + xpAwarded;
            Destroy(gameObject);
        }
    }

    void HideEffect(){
        attackEffect.SetActive(false);
    }

    void OnCollisionEnter(Collision collision){
        if(collision.collider.gameObject.CompareTag("Player")){
            if (Time.time >= timeStamp)
            {
                timeStamp = Time.time + cooldown;
                GameObject player = collision.collider.gameObject;
                PlayerController playerController = player.GetComponent<PlayerController>();
                playerController.currentHealth = playerController.currentHealth - damage;
                attackEffect.SetActive(true);
                Invoke(nameof(HideEffect), 0.1f);
            }
        }
    }

    void OnCollisionStay(Collision collision){
        if(collision.collider.gameObject.CompareTag("Player")){
            if (Time.time >= timeStamp)
            {
                timeStamp = Time.time + cooldown;
                GameObject player = collision.collider.gameObject;
                PlayerController playerController = player.GetComponent<PlayerController>();
                playerController.currentHealth = playerController.currentHealth - damage;
                attackEffect.SetActive(true);
                Invoke(nameof(HideEffect), 0.1f);
            }
        }
    }
}
 