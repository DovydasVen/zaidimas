using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public GameObject Area;

    // Start is called before the first frame update
    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.CompareTag("Player")){
            GameObject player = collider.gameObject;
            Area.GetComponent<AreaController>().SpawnEnemies(player);
        }
    }
}
