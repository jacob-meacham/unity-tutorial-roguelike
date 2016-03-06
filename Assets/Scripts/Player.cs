using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Player : Actor
{
    public float restartLevelDelay = 1f;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public int wallDamage = 1;
    public int enemyDamage = 1;
    public Text healthText;
    
    private Animator animator;
    private int health;

    // Use this for initialization
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        health = GameManager.instance.playerHealth;
        base.Start();
        
        // TODO: This should be handled by a UI layer
        healthText.text = "Health " + health;
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
            AttemptMove(horizontal, vertical);
        }
    }

    protected override bool AttemptMove(int xDir, int yDir)
    {
        health--;
        healthText.text = "Health " + health;
        CheckIfGameOver(true);

        bool canMove = base.AttemptMove(xDir, yDir);
        GameManager.instance.playersTurn = false;

        return canMove;
    }

    protected override void OnCollide(GameObject hitObject)
    {
        if (hitObject.tag == "Wall") {
            Wall hitWall = hitObject.GetComponent<Wall>();
            hitWall.DamageWall(wallDamage);
            animator.SetTrigger("PlayerChop");       
        } else if (hitObject.tag == "Enemy") {
            Enemy hitEnemy = hitObject.GetComponent<Enemy>();
            hitEnemy.LoseHealth(enemyDamage);
            animator.SetTrigger("PlayerChop");
        }   
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
            healthText.text = "+" + pointsPerFood + "! Health " + health;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            health += pointsPerSoda;
            healthText.text = "+" + pointsPerSoda + "! Health " + health;
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
        healthText.text = "-" + loss + "! Health " + health;
        CheckIfGameOver(false);
    }

    private void CheckIfGameOver(bool starved)
    {
        if (health <= 0)
        {
            GameManager.instance.GameOver(starved);
        }
    }
}
