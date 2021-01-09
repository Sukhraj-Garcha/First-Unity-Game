using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectUtil {

    private static Dictionary<RecycleGameObject, ObjectPool> pools = new Dictionary<RecycleGameObject, ObjectPool>();
   
    //custom instantiate method
    public static GameObject Instantiate(GameObject prefab, Vector3 pos) {

        GameObject instance = null;

        var recycledScript = prefab.GetComponent<RecycleGameObject>();

        //get next object from pool if it exists 
        if (recycledScript != null) {

            var pool = GetObjectPool(recycledScript);
            instance = pool.NextObject(pos).gameObject;
        
        //otherwise create a new one
        }else {

            instance = GameObject.Instantiate(prefab);
            instance.transform.position = pos;
        }

        return instance;
    }

    public static void Destroy(GameObject gameObject) {

        var recycleGameObject = gameObject.GetComponent<RecycleGameObject>();

        //if object exists, make it in active instead of completely destroying (allows for recycling objects)
        if (recycleGameObject != null) {

            recycleGameObject.Shutdown();
        
        //otherwise, fully destroy the object
        }else {

            GameObject.Destroy(gameObject);
        }
    }

    private static ObjectPool GetObjectPool(RecycleGameObject reference) {

        ObjectPool pool = null;

        //pool exists already
        if (pools.ContainsKey(reference)) {

            pool = pools[reference];
        
        //otherwise a new pool needs to be created
        }else {

            var poolContainer = new GameObject(reference.gameObject.name + "ObjectPool");
            pool = poolContainer.AddComponent<ObjectPool>();
            pool.prefab = reference; //allows pool to know what type of object to create
            pools.Add(reference, pool);
        }

        return pool;
    }
}
