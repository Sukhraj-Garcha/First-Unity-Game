using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantVelocity : MonoBehaviour {

    public Vector2 velocity = Vector2.zero;
    private Rigidbody2D rb;

    //awake method
    void Awake() {

        rb = GetComponent<Rigidbody2D>();
    }
    
    void FixedUpdate() {

        rb.velocity = velocity;
    }
}
