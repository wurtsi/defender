using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public PlayerController player;
    public GameObject enemyPrefab;

    private int score = 0;
    public Text scoreText;

    public List<Image> lifeIndicators = new List<Image>();
    private int lifeCount;

    [HideInInspector] public List<Enemy> enemies = new List<Enemy>();

    public Vector2 spawnRate = new Vector2();
    private float currentSpawnCooldown;

    [HideInInspector] public Vector3 playerPos;

    public static GameManager instance;

    void Start ()
    {
        instance = this;
        lifeCount = lifeIndicators.Count;

        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemyObjects)
        {
            enemies.Add(enemy.GetComponent<Enemy>());
        }
	}

    public void TakeLife()
    {
        lifeCount--;
        lifeIndicators[lifeIndicators.Count - 1].gameObject.SetActive(false);

        if (lifeCount < 1)
            GameOver();
    }

    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(Random.Range(-30, 30), -10f, 0f), Quaternion.identity);
        enemies.Add(enemy.GetComponent<Enemy>());
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = score.ToString();
    }
	
    public void GameOver()
    {
        SceneManager.LoadScene(0);
    }
    
    void Update ()
    {
        playerPos = player.transform.position;

        if (currentSpawnCooldown > 0)
        {
            currentSpawnCooldown -= Time.deltaTime;
            return;
        }
        else
        {
            SpawnEnemy();
            currentSpawnCooldown = Random.Range(spawnRate.x, spawnRate.y);
        }
    }
}
