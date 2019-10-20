using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackClass : MonoBehaviour
{
    // Class stats variables
    [SerializeField] int attack = 25;
    [SerializeField] int move = 3;
    [SerializeField] int defense = 0;

    // Colliders
    private CapsuleCollider2D hitbox;

    private void Awake()
    {
        hitbox = GetComponent<CapsuleCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
