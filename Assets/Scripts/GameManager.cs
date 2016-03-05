using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public int playerHealth = 100;
    public float turnDelay = 0.1F;
    private BoardManager boardManager;
    
    private int level = 3;
    public int Level 
    { 
        get { return level; } 
    }

    [HideInInspector]
    public bool playersTurn = true;
    private List<Enemy> enemies;
    private bool enemiesMoving = false;

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
        
        InitGame();
    }

    void InitGame()
    {
        boardManager.SetupScene(level);
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

    public void GameOver()
    {
        enabled = false;
    }

    public void AddEnemy(Enemy e)
    {
        enemies.Add(e);
    }
    
    public void RemoveEnemy(Enemy e) {
        enemies.Remove(e);
    }
}
