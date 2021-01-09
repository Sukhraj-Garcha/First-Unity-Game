using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {

    public GameObject playerPrefab;

    //vars to display text
    public Text continueText;
    public Text scoreText;
    public Text difficultyText;

    //vars for text blink effect
    private float blinkTime = 0f;
    private bool blink;

    //vars for calculating score
    private float timeElapsed = 0f; 
    private float bestTime = 0f;
    private bool beatBest;

    //vars for stopping/restarting the game
    private bool gameStarted;
    private TimeManager timeManager; 

    //references to player, floor, spawner
    private GameObject player;
    private GameObject floor;
    private Spawner spawner;

    void Awake() {

        floor = GameObject.Find("Foreground");
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        timeManager = GetComponent<TimeManager>();
    }
    
    // Start is called before the first frame update
    void Start() {

        var floorHeight = floor.transform.localScale.y;
        var pos = floor.transform.position;

        pos.x = 0; //make sure floor is always centered
        pos.y = -((Screen.height / PixelPerfectCamera.pixelsToUnits) / 2) + (floorHeight/2); //calc position that will place floor at bottom of screen
        floor.transform.position = pos;

        spawner.active = false; //turn spawner off at start of game

        Time.timeScale = 0; //start time at 0 so game isn't running initially

        continueText.text = "Press Any Button to Start";

        bestTime = PlayerPrefs.GetFloat("BestTime");

        //hide difficulty text 
        difficultyText.enabled = false;
    }

    // Update is called once per frame
    void Update() {
        
        //check if game is over
        if (!gameStarted && Time.timeScale == 0) {

            //reset time to 1 after any key is pressed
            if (Input.anyKeyDown) {

                timeManager.ManipulateTime(1, 1f);
                ResetGame();
            }
        }

        if (!gameStarted) {

            //make starting text blink
            blinkTime++;

            if (blinkTime % 40 == 0) {

                blink = !blink;
            }

            continueText.canvasRenderer.SetAlpha(blink ? 0 : 1);


            var textColor = beatBest ? "#FF0" : "#FFF"; //change color depending on whether or not best score is beaten
            scoreText.text = "Time: " + FormatTime(timeElapsed) + "\n<color="+ textColor + ">Best : " + FormatTime(bestTime) + "</color>";

        }else {

            //show current time elapsed as the game is running
            timeElapsed += Time.deltaTime;
            scoreText.text = "Time: " + FormatTime(timeElapsed);
        }
    }

    //stop spawner when player dies
    void OnPlayerKilled() {

        spawner.active = false;

        //unlink delegate
        var playerDestroyScript = player.GetComponent<DestroyOffScreen>();
        playerDestroyScript.destroyCallBack -= OnPlayerKilled;

        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        timeManager.ManipulateTime(0, 5.5f);

        gameStarted = false;

        //change text to an appropriate message for restarting
        continueText.text = "Press Any Button to Restart";

        //check for new best score and save it
        if (timeElapsed > bestTime) {

            bestTime = timeElapsed;
            PlayerPrefs.SetFloat("BestTime", bestTime);
            beatBest = true;
        }
    }

    //start (or restart) the game
    void ResetGame() {

        spawner.active = true;

        var spawnY = (Screen.height / PixelPerfectCamera.pixelsToUnits) / 2 + 100;
        player = GameObjectUtil.Instantiate(playerPrefab, new Vector3(0, spawnY, 0));

        //link up delegate
        var playerDestroyScript = player.GetComponent<DestroyOffScreen>();
        playerDestroyScript.destroyCallBack += OnPlayerKilled;

        gameStarted = true;

        //hide text once game has started 
        continueText.canvasRenderer.SetAlpha(0);

        //reset vars 
        timeElapsed = 0f; //score
        beatBest = false;

        //reset delay ranges back to original 
        spawner.delayRange.x = 6;
        spawner.delayRange.y = 10;

        //increase rate of obstacles every 20 seconds
        InvokeRepeating("ChangeObstacleSpawnRate", 15f, 20f);

    }

    string FormatTime(float val) {

        TimeSpan t = TimeSpan.FromSeconds(val);
        return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
    }

    void ChangeObstacleSpawnRate() {

        //decrease until cap of x = 1 and y = 2
        if (spawner.delayRange.x > 1) {

            spawner.delayRange.x--;
            Debug.Log("range x = " + spawner.delayRange.x);
        }

        if (spawner.delayRange.y > 2) {

            spawner.delayRange.y--;
            Debug.Log("range y = " + spawner.delayRange.y);
        }

        //show prompt to user
        if (spawner.delayRange.x > 1 || spawner.delayRange.y > 2) {

            StartCoroutine(ShowDifficultyText(2f));
        }
    }

    //pop up difficulty message to notify user that difficulty increased
    IEnumerator ShowDifficultyText (float delay) {

        difficultyText.enabled = true;
        yield return new WaitForSeconds(delay);
        difficultyText.enabled = false;
    }
}
