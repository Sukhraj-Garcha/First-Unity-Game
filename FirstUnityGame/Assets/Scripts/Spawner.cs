using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject[] prefabs; //array of all obstacles
    public float delay = 2.0f;
    public Vector2 delayRange = new Vector2(1, 2); 
    public bool active = true;

    // Start is called before the first frame update
    void Start() {

        ResetDelay();
        StartCoroutine(EnemyGenerator());
    }

    //generate enemies after the given delay
    IEnumerator EnemyGenerator() {

        yield return new WaitForSeconds(delay);

        if (active) {

            //pos of where we want to spawn
            var newTransform = transform; 

            //pick random obstacle to spawn
            GameObjectUtil.Instantiate(prefabs[Random.Range(0, prefabs.Length)], newTransform.position);
            ResetDelay();
        }

        StartCoroutine(EnemyGenerator());
    }

    void ResetDelay() {

        delay = Random.Range(delayRange.x, delayRange.y);
    }
}
