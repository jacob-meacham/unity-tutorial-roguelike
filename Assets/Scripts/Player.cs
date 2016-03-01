using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Player : Actor
{
    public float restartLevelDelay = 1f;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public int wallDamage = 1;

    private Animator animator;
    private int health;

    // Use this for initialization
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        health = GameManager.instance.playerHealth;
        base.Start();
    }

    private void OnDisable()
    {
        // Store health across level loads
        GameManager.instance.playerHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.playersTurn)
        {
            return;
        }

        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = (int)Input.GetAxisRaw("Vertical");

        // Only allow movement in a cardinal direction
        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            // Attempt to move
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    protected override bool AttemptMove<T>(int xDir, int yDir)
    {
        health--;
        CheckIfGameOver();

        bool canMove = base.AttemptMove<T>(xDir, yDir);
        GameManager.instance.playersTurn = false;

        return canMove;
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("PlayerChop");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("OnExit", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            health += pointsPerFood;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            health += pointsPerSoda;
            other.gameObject.SetActive(false);
        }
    }

    private void OnExit()
    {
        GameManager.instance.NextLevel();
    }

    public void LoseHealth(int loss)
    {
        animator.SetTrigger("PlayerHit");
        health -= loss;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (health <= 0)
        {
            GameManager.instance.GameOver();
        }
    }
}
