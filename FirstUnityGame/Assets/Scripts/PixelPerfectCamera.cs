using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfectCamera : MonoBehaviour
{
    //required vars
    public static float pixelsToUnits = 1f;
    public static float scale = 1f;
    public Vector2 nativeResolution = new Vector2(240, 160);
   

    void Awake() {
        Camera cam = gameObject.GetComponent<Camera>();
        if (cam.orthographic) {
            scale = Screen.height / nativeResolution.y;
            pixelsToUnits += scale;
            cam.orthographicSize = (Screen.height / 2.0f) / pixelsToUnits;
        }
    }

}
