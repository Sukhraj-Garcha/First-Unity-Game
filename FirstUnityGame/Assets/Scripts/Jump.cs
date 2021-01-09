using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {

    public float jumpSpeed = 240f;
    public float forwardSpeed = 20f;

    private Rigidbody2D rb;
    private InputState inputState;

    void Awake() {

        rb = GetComponent<Rigidbody2D>();
        inputState = GetComponent<InputState>();
    }

    // Update is called once per frame
    void Update() {
        
        //player needs to be standing and the action button must be pressed to make the player jump
        if (inputState.standing) {

            if (inputState.actionButton) {

                //player always jumps up, but will jump slightly forward if past center of screen (otherwise only vertically)
                rb.velocity = new Vector2(transform.position.x < 0 ? forwardSpeed : 0, jumpSpeed);
            }
        }
    }
}
