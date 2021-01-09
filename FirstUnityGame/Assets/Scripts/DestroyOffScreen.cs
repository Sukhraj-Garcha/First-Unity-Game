using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffScreen : MonoBehaviour {

    public delegate void onDestroy();
    public event onDestroy destroyCallBack;

    public float offset = 16f;
    private bool offScreen;
    private float offScreenX = 0f;
    private Rigidbody2D rb;

    //awake method
    void Awake() {

        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start() {

        offScreenX = (Screen.width / PixelPerfectCamera.pixelsToUnits) / 2 + offset;

    }

    // Update is called once per frame
    void Update() {

        var posX = transform.position.x;
        var dirX = rb.velocity.x;
           
        //determine if the object is offscreen
        if (Mathf.Abs(posX) > offScreenX) {

            //offscreen left
            if (dirX < 0 && posX < -offScreenX) {

                offScreen = true;
            
            //offscreen right
            }else if (dirX > 0 && posX > offScreenX) {

                offScreen = true;
           
            //not offscreen
            }else {

                offScreen = false;
            }
        }

        if (offScreen) {

            OnOutOfBounds();
        }
    }

    void OnOutOfBounds() {

        offScreen = false;
        GameObjectUtil.Destroy(gameObject);

        if (destroyCallBack != null) {

            destroyCallBack();
        }
    }
}
