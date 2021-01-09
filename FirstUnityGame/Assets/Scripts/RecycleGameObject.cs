using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRecycle {

    void Restart();
    void ShutDown();
}

public class RecycleGameObject : MonoBehaviour {

    private List<IRecycle> recycleComponents;

    public void Awake() {

        //get all of the components of the object that extend MonoBehaviour
        var components = GetComponents<MonoBehaviour>();
        recycleComponents = new List<IRecycle>();

        //test which components implement IRecycle
        foreach(var component in components) {

            if (component is IRecycle) {

                recycleComponents.Add(component as IRecycle);

            }

        }
        
    }

    public void Restart() {

        gameObject.SetActive(true);

        //loop through and restart each component
        foreach (var component in recycleComponents) {

            component.Restart();
        }
    }

    public void Shutdown() {

        gameObject.SetActive(false);

        //loop through and shutdown each component
        foreach (var component in recycleComponents) {

            component.ShutDown();
        }
    }
}
