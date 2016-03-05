using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Enemy : Actor {
    public int playerDamage;
    public int health;
    
    private Animator animator;
    private Transform target;

	// Use this for initialization
	protected override void Start () {
	   GameManager.instance.AddEnemy(this);
       animator = GetComponent<Animator>();
       target = GameObject.FindGameObjectWithTag("Player").transform;
       health += GameManager.instance.Level / 2;
       base.Start();
	}
    
    public void Move() {
        int xDir = 0;
        int yDir = 0;
        
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon) {
            yDir = target.position.y > transform.position.y ? 1 : -1;
        } else {
            xDir = target.position.x  > transform.position.x ? 1 : -1;
        }
        
        AttemptMove(xDir, yDir);
    }
    
    public void LoseHealth(int loss)
    {
        animator.SetTrigger("EnemyHit");
        health -= loss;
        if (health < 0) {
            GameManager.instance.RemoveEnemy(this);
            gameObject.SetActive(false);
        }
    }
    
    protected override void OnCollide (GameObject hitObject) {
        Player player = hitObject.GetComponent<Player>();
        if (player != null) {
            player.LoseHealth(playerDamage);
            animator.SetTrigger("EnemyAttack");    
        }
        
    }
}
