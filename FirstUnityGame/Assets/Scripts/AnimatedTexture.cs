using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedTexture : MonoBehaviour {

    public Vector2 speed = Vector2.zero;
    private Vector2 offset = Vector2.zero;
    private Material material;
    
    // Start is called before the first frame update
    void Start() {

        material = GetComponent<Renderer>().material;
        offset = material.GetTextureOffset("_MainTex");
    }

    // Update is called once per frame
    void Update() {

        //change x offset of background so it appears as if the player is moving
        offset += speed * Time.deltaTime;
        material.SetTextureOffset("_MainTex", offset);
    }
}
