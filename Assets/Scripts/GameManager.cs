using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public int playerHealth = 100;
    private BoardManager boardManager;
    private int level = 3;
    [HideInInspector] public bool playersTurn = true;

    void Awake()
    {
        print("Awake");
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

        boardManager = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        boardManager.SetupScene(level);
    }

    public void NextLevel()
    {
        level += 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void GameOver() {
        enabled = false;
    }
}
