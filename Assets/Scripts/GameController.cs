using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public SaveData saveData;
    public GameObject healthBar;
    public GameObject experienceBar;
    public GameObject gameOverScreen;
    public GameObject statsScreen;
    public GameObject victoryScreen;
    public GameObject menuScreen;
    public GameObject pauseScreen;
    public GameObject upgradeScreen;
    public GameObject instructionsScreen;
    public GameObject devMenu;
    public GameObject levelUI;
    public GameObject player;
    public GameObject levelGenerator;
    public GameObject cameraHolder;
    public GameObject BackgroundMusic;
    public int  maxDifficulty = 1;
    public int difficulty = 1;
    public int points = 0;
    public float maxHealth = 100f;
    public float damage = 10f;
    public float cooldown = 1f;
    public float speed = 5f;
    public float score = 0f;
    private bool isStarted= false;

    public void Upgrades(){
        menuScreen.SetActive(false);
        upgradeScreen.SetActive(true);
        UpgradeScreenUpdate();
    }

    public void Instructions(){
        menuScreen.SetActive(false);
        instructionsScreen.SetActive(true);
    }

    public void Plus(){
        difficulty++;
        DifficultyUpdate();
    }

    public void Minus(){
        difficulty--;
        DifficultyUpdate();
    }

    public void Victory(){
        isStarted = false;
        player.GetComponent<PlayerController>().enabled = false;
        victoryScreen.SetActive(true);
        statsScreen.SetActive(false);
        devMenu.SetActive(false);
        score = score * (1 + 0.2f * difficulty);
        points =  points + (int) Mathf.Round(score/2000);
        victoryScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Your score: " + Mathf.Round(score);
        if(difficulty == maxDifficulty){
            maxDifficulty++;
        }
    }

    public void GameOver(){
        isStarted = false;
        gameOverScreen.SetActive(true);
        statsScreen.SetActive(false);
        score = score / 2 * (1 + 0.2f * difficulty);
        points =  points + (int) Mathf.Round(score/2000);
        gameOverScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Your score: " + Mathf.Round(score);
    }

    public void Continue(){
        isStarted = true;
        pauseScreen.SetActive(false);
        player.GetComponent<PlayerController>().enabled = true;
        Time.timeScale = 1;
    }

    public void Play(){
        Time.timeScale = 1;
        isStarted = true;
        healthBar.SetActive(true);
        experienceBar.SetActive(true);
        levelUI.SetActive(true);
        player.SetActive(true);
        menuScreen.SetActive(false);
        levelGenerator.GetComponent<LevelGenerator>().enabled = true;
        player.GetComponent<PlayerController>().enabled = true;
        cameraHolder.GetComponent<CameraController>().enabled = true;
        player.GetComponent<PlayerController>().maxHealth = maxHealth;
        player.GetComponent<PlayerController>().damage = damage;
        player.GetComponent<PlayerController>().cooldown = cooldown;
        player.GetComponent<PlayerController>().speed  = speed;
        BackgroundMusic.SetActive(true);
    }

    public void Restart(){
        saveData.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit(){
        Application.Quit();
    }

    public void DifficultyUpdate(){
        menuScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Difficulty: " + difficulty;
    }

    public void StatsScreenUpdate(){
        statsScreen.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = string.Format("Available points: {0}", player.GetComponent<PlayerController>().points);
        statsScreen.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = string.Format("Max health: {0}", player.GetComponent<PlayerController>().maxHealth);
        statsScreen.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = string.Format("Damage: {0}", player.GetComponent<PlayerController>().damage);
        statsScreen.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = string.Format("Attack cooldown: {0}", player.GetComponent<PlayerController>().cooldown);
        statsScreen.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = string.Format("Speed: {0}", player.GetComponent<PlayerController>().speed);
    }

    public void UpgradeScreenUpdate(){
        upgradeScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = string.Format("Available points: {0}", points);
        upgradeScreen.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format("Max health: {0}", maxHealth);
        upgradeScreen.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = string.Format("Damage: {0}", damage);
        upgradeScreen.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = string.Format("Attack cooldown: {0}", cooldown);
        upgradeScreen.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = string.Format("Speed: {0}", speed);
    }

    public void AddHealth(){
        if(player.GetComponent<PlayerController>().points > 0 ){
            player.GetComponent<PlayerController>().points--;
            player.GetComponent<PlayerController>().maxHealth = (float)Math.Round((double) player.GetComponent<PlayerController>().maxHealth * 1.1f);
            StatsScreenUpdate();
        }
        
    }
    public void AddDamage(){
        if(player.GetComponent<PlayerController>().points > 0 ){
            player.GetComponent<PlayerController>().points--;
            player.GetComponent<PlayerController>().damage = (float)Math.Round((double) player.GetComponent<PlayerController>().damage * 1.1f, 2);
            StatsScreenUpdate();
        }
    }
    public void RemoveCooldown(){
        if(player.GetComponent<PlayerController>().points > 0 ){
            player.GetComponent<PlayerController>().points--;
            player.GetComponent<PlayerController>().cooldown = (float)Math.Round((double) player.GetComponent<PlayerController>().cooldown * 0.95f);
            StatsScreenUpdate();
        }
    }
    public void AddSpeed(){
        if(player.GetComponent<PlayerController>().points > 0 ){
            player.GetComponent<PlayerController>().points--;
            player.GetComponent<PlayerController>().speed = (float)Math.Round((double) player.GetComponent<PlayerController>().speed * 1.1f);
            StatsScreenUpdate();
        }
    }

    public void AddHealthUpgrade(){
        if(points > 0 ){
            points--;
            maxHealth = maxHealth + 10f;
            UpgradeScreenUpdate();
        }
        
    }
    public void AddDamageUpgrade(){
        if(points > 0 ){
            points--;
            damage = damage + 1f;
            UpgradeScreenUpdate();
        }
    }
    public void RemoveCooldownUpgrade(){
        if(points > 0 ){
            points--;
            cooldown = cooldown - 0.02f;
            UpgradeScreenUpdate();
        }
    }
    public void AddSpeedUpgrade(){
        if(points > 0 ){
            points--;
            speed = speed + 0.2f;
            UpgradeScreenUpdate();
        }
    }

    // For testing OnApplicationQuit within editor
    void OnDestroy(){
        OnApplicationQuit();
    }

    void OnApplicationQuit(){
        saveData.Save();
    }

    // Start is called before the first frame update
    void Start()
    {
        SaveData.PlayerData playerData = saveData.Load();

        if(playerData != null){
            maxDifficulty = playerData.maxDifficulty;
            difficulty = playerData.difficulty;
            points =  playerData.points;
            maxHealth = playerData.maxHealth;
            damage = playerData.damage;
            cooldown = playerData.cooldown;
            speed = playerData.speed;
        }

        if(maxDifficulty == 1){
            menuScreen.transform.GetChild(1).gameObject.SetActive(false);
            menuScreen.transform.GetChild(2).gameObject.SetActive(false);
            menuScreen.transform.GetChild(3).gameObject.SetActive(false);
        } else{
            DifficultyUpdate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(difficulty == maxDifficulty){
            menuScreen.transform.GetChild(2).gameObject.SetActive(false);
        } else{
            menuScreen.transform.GetChild(2).gameObject.SetActive(true);
        }

        if(difficulty == 1){
            menuScreen.transform.GetChild(3).gameObject.SetActive(false);
        } else {
            menuScreen.transform.GetChild(3).gameObject.SetActive(true);
        }

        if(isStarted == false) return;
        
        if(Input.GetKeyDown("tab")){
            statsScreen.SetActive(!statsScreen.activeSelf);
            StatsScreenUpdate();
        }

        if(Input.GetKeyDown("escape")){
            isStarted = false;
            pauseScreen.SetActive(true);
            statsScreen.SetActive(false);
            devMenu.SetActive(false);
            player.GetComponent<PlayerController>().enabled = false;
            Time.timeScale = 0;
        }

        if(Input.GetKeyDown("`")){
            statsScreen.SetActive(false);
            devMenu.SetActive(!devMenu.activeSelf);
        }

        if(player.GetComponent<PlayerController>().points > 0 && !levelUI.transform.GetChild(2).gameObject.activeSelf){
            levelUI.transform.GetChild(2).gameObject.SetActive(true);
        } else if(player.GetComponent<PlayerController>().points == 0 && levelUI.transform.GetChild(2).gameObject.activeSelf){
            levelUI.transform.GetChild(2).gameObject.SetActive(false);
        }

        levelUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = string.Format("{0}", player.GetComponent<PlayerController>().playerLevel);
    }
}
