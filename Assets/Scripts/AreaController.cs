using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour
{
    // 0 - UP 1 - DOWN 2 - RIGHT 3 - LEFT
    public GameObject[] entrances;
    public GameObject[] blocked;
    public GameObject[] enemies;
    public GameObject[] triggers;
    public GameObject[] battleWalls;
    public GameObject levelGenerator;
    public GameObject gameController;
    private bool enemiesSpawned = false;
    private bool completed = false;
    private int previousDead = 0;

    void Update(){
        if(enemiesSpawned == true && completed == false){
            int dead = 0;
            for(int i = 0; i < enemies.Length; i++){
                if(enemies[i] == null){
                    dead++;
                } else {
                    break;
                }
            }
            if(previousDead != dead){
                gameController.GetComponent<GameController>().score = gameController.GetComponent<GameController>().score + 150 * (dead-previousDead);
            }
            previousDead = dead; 
            if(dead == enemies.Length){
                completed = true;
                levelGenerator.GetComponent<LevelGenerator>().roomsCompleted++;
                DisableBattleWalls();
                gameController.GetComponent<GameController>().score = gameController.GetComponent<GameController>().score + 50;
            }
        }
    }
    
    public void InitRoom(bool[] status){
        for(int i = 0; i < status.Length; i++){
            entrances[i].SetActive(status[i]);
            blocked[i].SetActive(!status[i]);
        }

        levelGenerator = GameObject.Find("LevelGenerator");
        gameController = GameObject.Find("GameController");
    }

    public void DisableTriggers(){
        for(int i=0; i<triggers.Length; i++){
            triggers[i].SetActive(false);
        }
    }

    public void DisableBattleWalls(){
        for(int i=0; i<battleWalls.Length; i++){
            battleWalls[i].GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void SpawnEnemies(GameObject player){
        if(enemiesSpawned == false){
            DisableTriggers();
            for(int i = 0; i < enemies.Length; i++){
                enemies[i].GetComponent<EnemyController>().target = player.transform;
                enemies[i].SetActive(true);
                battleWalls[i].GetComponent<BoxCollider>().enabled = true;
            }
            enemiesSpawned = true;
        }
    }
}
