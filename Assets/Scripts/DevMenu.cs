using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevMenu : MonoBehaviour
{
    public GameController gameController;
    public PlayerController playerController;

    public void Victory(){
        gameController.Victory();
    }

    public void Levels(){
        playerController.playerLevel = playerController.playerLevel + 10;
        playerController.points = playerController.points + 20;
    }

    public void Score(){
        gameController.score = gameController.score + 1000;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
