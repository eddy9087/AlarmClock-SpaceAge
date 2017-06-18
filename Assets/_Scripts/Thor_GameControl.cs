using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thor_GameControl : MonoBehaviour {

	public GameObject IceGolem;
	public GameObject Goblin;
	public bool gameOver;
	public float volTh;
    public float waveWait;
    public float gameOverWait;
    public Text gameText;
    public Text scoreText;
    public Text healthText;
    public AudioClip thundersound;
    
    private bool Ragnarok;//private
	private bool UltAvailable;
	private int GoblinNum;
	private int GolemNum;
    private int score;
    private int health = 10;
    private int passScore = 300;
    private string today = System.DateTime.Now.Date.ToString();
    private float recordTime = 0f;
    private bool timeOut = false;
    private bool death = false;
    private MicInput[] mic;
    private AudioSource source;


    // Use this for initialization
    void Start () {
		gameOver = false;
		Ragnarok = false;
		UltAvailable = true;
		mic = Camera.main.GetComponents <MicInput> ();

		StartCoroutine (SpawnWaves ());
        source = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		if (mic[0].getLoudness() >= volTh && UltAvailable){
			UltAvailable = false;
			Ragnarok = true;
            source.Play();
            StartCoroutine (WaitForThunder ());
		}
        if (health <= 0){
            death = true;
            GameOver();
        }            
        if (score >= passScore && !gameOver)
            GameOver();
        else if (recordTime >= 120 && !gameOver){
            timeOut = true;
            GameOver();
        }
        else if (!gameOver)
            recordTime += Time.deltaTime;
    }

	IEnumerator WaitForThunder (){
		yield return new WaitForSeconds (5);
		Ragnarok = false;
        source.Stop();
        yield return new WaitForSeconds (25);
		UltAvailable = true;
	}

	public bool ThorUltimate (){
	   return Ragnarok;
	}

	IEnumerator SpawnWaves (){
		GoblinNum = Random.Range (10, 15);
		GolemNum = Random.Range (3, 5);
		for (int times = 0; times <= 100; times++) {
            for (int i = 0; i < GoblinNum; i++) {
				Vector3 spawnPosition = new Vector3 (Random.Range (-80, -25) * (Random.Range(0,2)*2 -1), 8, Random.Range (-80, -25) * (Random.Range(0, 2) * 2 - 1));
				Quaternion spawnRotation = Quaternion.identity;
				GameObject spawnedHazard = Instantiate (Goblin, spawnPosition, spawnRotation);
                if (gameOver)
                {
                    yield return new WaitForSeconds(gameOverWait);
                    break;
                }
            }
            for (int i = 0; i < GolemNum; i++) {
				Vector3 spawnPosition = new Vector3 (Random.Range (-80, -25) * (Random.Range(0,2)*2 -1), 8, Random.Range (-80, -25) * (Random.Range(0, 2) * 2 - 1));
				Quaternion spawnRotation = Quaternion.identity;
				GameObject spawnedHazard = Instantiate (IceGolem, spawnPosition, spawnRotation);
                if (gameOver)
                {
                    yield return new WaitForSeconds(gameOverWait);
                    break;
                }
            }
            Debug.Log("score = " + score);
            Debug.Log("health = "+ health);
            if (gameOver)
                break;
            else
                yield return new WaitForSeconds(3);
        }
        MenuController.control.AddCoins(score);

        Application.LoadLevel("Home");
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    public void MinusHealth(int newHealthValue)
    {
        health -= newHealthValue;
        UpdateHealth();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    void UpdateHealth()
    {
        healthText.text = "Health: " + health;
    }

    public void GameOver()
    {
        if (!timeOut && !death)
        {
            gameText.text = "Game Over";

            MenuController.control.shootRecords.Add(new ShootRecord(today, score));
            MenuController.control.shootRecords.Sort();
            //Debug.Log ("Date1: " + MenuController.control.shootRecords [1].date +"Time1: " + MenuController.control.shootRecords [1].time +"Count: " + MenuController.control.shootRecords.Count.ToString ());
        }
        else
        {
            gameText.text = "Game Failure";
        }
        gameOver = true;
    }
}
