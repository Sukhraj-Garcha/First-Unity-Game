using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    
    //reference all game objects
    public RecycleGameObject prefab; 

    //keep track of created instances
    private List<RecycleGameObject> poolInstances = new List<RecycleGameObject>();

    private RecycleGameObject createInstance(Vector3 pos) {

        var clone = GameObject.Instantiate(prefab);
        clone.transform.position = pos;
        clone.transform.parent = transform;

        poolInstances.Add(clone);

        return clone;
    }

    //return next object in pool
    public RecycleGameObject NextObject(Vector3 pos) {

        RecycleGameObject instance = null;

        //recycle game objects that already exist but are disabled
        foreach(var go in poolInstances) {

            if (go.gameObject.activeSelf != true) {

                instance = go;
                instance.transform.position = pos;
            }
        }

        //create a new instance if the game object does not exist
        if (instance == null) {

            instance = createInstance(pos);
        }

        instance.Restart();

        return instance;
    }
    
}
