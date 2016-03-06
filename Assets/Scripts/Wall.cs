using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class Wall : MonoBehaviour
{
    public Sprite dmgSprite;
    public int hp = 3;
    public AudioClip[] chopSounds;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int damage)
    {
        spriteRenderer.sprite = dmgSprite;
        hp -= damage;
        
        SoundManager.Instance.PlayRandom(chopSounds);

        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
