using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIScript : MonoBehaviour
{
    public static GameUIScript instance;
    public GameObject allUI;
	public Image fadePlane;
	public GameObject gameOverUI;
	public RectTransform playerHealthBar;
    public RectTransform bossHealthBar;
	PlayerTDController player;
    BossScript bossScript;

    void Awake(){
        instance = this;
    }
	void Start () {
		//allUI.SetActive (true);
		player = PlayerTDController.instance;
		player.OnDeath += OnGameOver;
	}
    void Update() {
        float playerHealthPercent = 0;
        if (player != null) {
            playerHealthPercent = player.health / player.maxHealth;
        }
        playerHealthBar.localScale = new Vector3 (playerHealthPercent, 1, 1);
        float bossHealthPercent = 0;
        if (bossScript != null) {
            bossHealthPercent = bossScript.health / bossScript.maxHealth;
        }
        bossHealthBar.localScale = new Vector3 (bossHealthPercent, 1, 1);
    }

    void OnGameOver(){

    }

    public void BossSpawned(BossScript newBoss){
        bossScript = newBoss;
    }
}
