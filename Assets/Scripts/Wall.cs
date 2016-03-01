using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class Wall : MonoBehaviour
{
    public Sprite dmgSprite;
    public int hp = 3;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int damage)
    {
        spriteRenderer.sprite = dmgSprite;
        hp -= damage;

        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
