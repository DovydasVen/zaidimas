using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public SaveData saveData;
    public GameController gameController;
    private bool isDead = false;
    public GameObject attackEffect;
    public float maxHealth = 100;
    public float currentHealth;
    public float playerLevel = 1;
    public float xp = 0;
    public Image health;
    public TextMeshProUGUI healthText;
    public Image experienceBar;
    public TextMeshProUGUI experienceText;
    public TextMeshProUGUI levelText;
    public AudioSource attackSound;
    public float speed;
    public float healthRegen = 1f;
    private float healthTimeStamp;
    public float damage = 10f;
    public float cooldown = 1f;
    public int points = 0;

    public GameObject attackZone;
    private float timeStamp;
    private Vector2 move, mouse;
    private Vector3 rotationTarget; // Point to look towards

    public void HideEffect(){
        attackEffect.SetActive(false);
    }

    public void OnMove(InputAction.CallbackContext context){
        move = context.ReadValue<Vector2>();
    }

    public void OnMouse(InputAction.CallbackContext context){
        mouse = context.ReadValue<Vector2>();
    }

    public void MovePlayer(){
        var lookPosition = rotationTarget - transform.position;
        lookPosition.y = 0;
        var rotation = Quaternion.LookRotation(lookPosition);
        Vector3 direction = new Vector3(rotationTarget.x,0,rotationTarget.y);
        if(direction != Vector3.zero){
           transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
        }

        Vector3 movementAxis = new Vector3(move.x, 0f, move.y);
        transform.Translate(movementAxis * speed * Time.deltaTime, Space.World);
    }

    // Start is called before the first frame update
    void Start()
    {
        attackEffect.SetActive(false);
        currentHealth = maxHealth;
        timeStamp = Time.time;
        healthTimeStamp = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        healthRegen = maxHealth*0.05f;
        if(isDead == true ) return;

        if(currentHealth <= 0){
            isDead = true;
            gameController.GameOver();
            saveData.Save();
        }

        if(Time.time >= healthTimeStamp && currentHealth < maxHealth){
            currentHealth = currentHealth + healthRegen/10;
            healthTimeStamp = Time.time + 0.1f;
        }
        

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(mouse);

        if(Physics.Raycast(ray, out hit)){
            rotationTarget = hit.point;
        }

        MovePlayer();

        if (Input.GetMouseButton(0))
        {
            if (Time.time >= timeStamp)
            {
                timeStamp = Time.time + cooldown;
                attackEffect.SetActive(true);
                OnAttack();
                Invoke(nameof(HideEffect), 0.1f);
            }
        }

        health.rectTransform.localScale = new Vector3(currentHealth/maxHealth, 1, 1);
        healthText.text = string.Format("Health: {0}/{1}", Math.Round(currentHealth,1),maxHealth);

        experienceText.text = string.Format("XP: {0}/{1}", xp, playerLevel*10);
        experienceBar.rectTransform.localScale = new Vector3(xp/(playerLevel*10), 1, 1);

        if(xp >= playerLevel*10){
            xp = xp-playerLevel*10;
            playerLevel++;
            levelText.text = string.Format("{0}", playerLevel);
            points = points + 2;
        }
    }

    void OnAttack()
    {
        attackSound.Play();
        Collider[] attackColliders = Physics.OverlapSphere(attackZone.transform.position, 1);
        foreach(var attackCollider in attackColliders){
            if (attackCollider.gameObject.CompareTag("Enemy"))
            {
                GameObject enemy = attackCollider.gameObject;
                if (enemy != null)
                {
                    EnemyController enemyController = enemy.GetComponent<EnemyController>();
                    enemyController.currentHealth = enemyController.currentHealth - damage;
                }
            }
        }
    }
}
