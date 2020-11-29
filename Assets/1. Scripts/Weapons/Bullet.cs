using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage;
    public float Speed;
    public float Timer;
    private float _timer;
    public EnemyType EnemyType;
    public Transform ForwardPoint;
    private bool _negativeSpeed;
    private void Start()
    {
        _timer = Timer;
    }

    private void Update()
    {
        if (!GameStateManager.IsPlaying)
        {
            Destroy(gameObject);
            return;
        }
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 currentPos = transform.position;
        float moveSpeed = currentPos.x + 10 * Time.deltaTime;
        if (!ForwardPoint)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = Vector2.MoveTowards(currentPos, ForwardPoint.position, Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string tagToCompare = EnemyType == EnemyType.ENEMY ? "Enemy" : "Player";
        if (other.gameObject.CompareTag(tagToCompare))
        {
            // give damage
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy)
            {
                if (enemy.HealthRemaining <= 0)
                {
                    enemy.Die();
                }
                else
                {
                    enemy.TakeDamage(Damage);
                }

                Damage = Mathf.CeilToInt(Damage * 0.75f);
            }
            // play particles
        }
    }
}