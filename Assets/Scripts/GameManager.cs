using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public int playerHealth = 100;
    public float turnDelay = 0.1f;
    public float levelStartDelay = 2f;
    
    private int level = 1;
    private int initialPlayerHealth;
    public int Level 
    { 
        get { return level; } 
    }

    [HideInInspector]
    public bool playersTurn = true;
    
    private List<Enemy> enemies;
    private bool enemiesMoving = false;
    private bool inSetup = true;
    
    private Text levelText;
    private GameObject levelImage;
    private BoardManager boardManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Move to the root
        this.transform.parent = null;
        DontDestroyOnLoad(gameObject);

        enemies = new List<Enemy>();
        boardManager = GetComponent<BoardManager>();
        initialPlayerHealth = playerHealth;
        print(initialPlayerHealth);
        
        InitGame();
    }
    
    void OnLevelWasLoaded(int index)
    {
        // For reload after death
        if (level == 1) {
            playerHealth = initialPlayerHealth;
        }
        
        InitGame();
    }

    void InitGame()
    {
        inSetup = true;
        
        SetupGui();
        
        enemies.Clear();
        boardManager.SetupScene(level);
        
        Invoke("StartLevel", levelStartDelay);
    }
    
    void SetupGui() {
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();

        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        
    }
    
    void StartLevel() {
        levelImage.SetActive(false);
        inSetup = false;
        
    }

    void Update()
    {
        if (playersTurn || enemiesMoving)
        {
            return;
        }

        enemiesMoving = true;
        StartCoroutine(MoveEnemies());
    }

    private IEnumerator MoveEnemies()
    {
        yield return new WaitForSeconds(turnDelay);
        
        if (enemies.Count == 0) {
            // Add extra delay when there are no more enemies
            yield return new WaitForSeconds(turnDelay);    
        }
        
        foreach (Enemy enemy in enemies)
        {
            enemy.Move();
            yield return new WaitForSeconds(turnDelay);
        }
        
        // Now, the players turn!
        enemiesMoving = false;
        playersTurn = true;
    }

    public void NextLevel()
    {
        level += 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver(bool starved)
    {
        if (starved) {
            levelText.text = "After " + level + " days, you starved.";
        } else {
            levelText.text = "After " + level + " days, you were slain.";
        }
        levelImage.SetActive(true);
        
        enabled = false;
        Invoke("Restart", levelStartDelay);
    }
    
    public void Restart() {
        level = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddEnemy(Enemy e)
    {
        enemies.Add(e);
    }
    
    public void RemoveEnemy(Enemy e) {
        enemies.Remove(e);
    }
}
