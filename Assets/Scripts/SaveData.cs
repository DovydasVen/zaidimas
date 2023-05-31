using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    //The data that needs to be saved between games
    public class PlayerData{
        public int maxDifficulty;
        public int difficulty;
        public int points;
        public float maxHealth;
        public float damage;
        public float cooldown;
        public float speed;
    }

    public GameObject player;
    public GameController gameController;
    private string path;

    public void Save(){
        PlayerData playerData  = new PlayerData();
        playerData.maxDifficulty = gameController.maxDifficulty;
        playerData.difficulty = gameController.difficulty;
        playerData.points = gameController.points;
        playerData.maxHealth = gameController.maxHealth;
        playerData.damage = gameController.damage;
        playerData.cooldown = gameController.cooldown;
        playerData.speed = gameController.speed;

        string jsonData = JsonUtility.ToJson(playerData);
        using StreamWriter writer = new StreamWriter(path);
        writer.Write(jsonData);
    }

    public PlayerData Load(){
        if(File.Exists(path)){
            using StreamReader reader = new StreamReader(path);
            string json = reader.ReadToEnd();
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
            return playerData;
        } else{
            return null;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        path = Application.dataPath + "/Save.json";
    }
}
