using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputState : MonoBehaviour {

    public bool actionButton;
    public float absVelX = 0.0f;
    public float absVelY = 0.0f;
    public bool standing;
    public float standingThreshold = 1.0f;

    private Rigidbody2D rb;

    void Awake() {

        rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update() {

        //determine if action button is pressed or not
        actionButton = Input.anyKeyDown; 
    }

    void FixedUpdate() {

        //determine velocity
        absVelX = System.Math.Abs(rb.velocity.x);
        absVelY = System.Math.Abs(rb.velocity.y);

        //determine if player is standing
        standing = absVelY <= standingThreshold; 
    }
}
