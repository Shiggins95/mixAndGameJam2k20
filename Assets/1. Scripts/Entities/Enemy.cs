using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Health;
    public int HealthRemaining;
    public float Speed;
    public bool CanAttack;
    public float AttackInterval;
    public float AttackRadius;
    public int Attack;

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Transform _areaCheckPoint;
    [SerializeField] private float _checkRadius;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private GameObject Base;
    private Transform _endGoal;
    private int _endGoalIndex;

    private float _attackInterval;
    private bool _canMove;
    private Turret _target;

    private void Start()
    {
        // TODO: ADD IN RANDOM END POINTS FOR ENEMY TO GO TOWARDS INSTEAD OF THEM ALL GOING FOR ONE OF THEM
        // TODO: ADD LIKE THE SPAWN POINT AND ASSIGN WHEN INSTANTIATED
        HealthRemaining = Health;
        _canMove = true;
        _attackInterval = AttackInterval;
        _endGoal = FindNextTarget();
    }

    private Transform FindNextTarget()
    {
        GameObject[] _targets = GameObject.FindGameObjectsWithTag("Turret");
        GameObject closestTurret = FindClosesTurret(_targets);
        if (closestTurret)
        {
            return closestTurret.transform;
        }

        return null;
    }

    private GameObject FindClosesTurret(GameObject[] targets)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in targets)
        {
            if (t.gameObject.activeSelf)
            {
                float dist = Vector3.Distance(t.transform.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
        }
        return tMin;
    }

    private void Update()
    {

        if (!GameStateManager.IsPlaying)
        {
            return;
        }

        if (!_endGoal || !_endGoal.gameObject.activeSelf)
        {
            _endGoal = FindNextTarget();
            if (!_endGoal)
            {
                GameStateManager.IsPlaying = false;
                return;
            }
            _canMove = true;
        }


        if (CanAttack)
        {
            if (!_target)
            {
                CanAttack = false;
                return;
            }

            _attackInterval -= Time.deltaTime;
            Debug.Log($"hit");
            if (_attackInterval <= 0)
            {
                // attack turret
                _attackInterval = AttackInterval;
                if (_target && _target.HealthRemaining > 0)
                {
                    _target.TakeDamage(Attack);
                }
                else
                {
                    Debug.Log($"he ded");
                    _target.Die();
                    CanAttack = false;
                    _canMove = true;
                }
            }
        }

        if (_canMove)
        {
            transform.position =
                Vector2.MoveTowards(transform.position, _endGoal.transform.position, Speed * Time.deltaTime);
        }
        else
        {
            // play attack animation
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Turret"))
        {
            CanAttack = true;
            _canMove = false;

            _target = other.gameObject.GetComponent<Turret>();
        }

        // if (other.gameObject.CompareTag("Base"))
        // {
        //     CanAttack = true;
        //     _canMove = false;
        // }
    }


    public void TakeDamage(int damage)
    {
        HealthRemaining -= damage;
    }

    public void Die()
    {
        Debug.Log($"Enemy ded");
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        // Gizmos.color = Color.blue;
        // Gizmos.DrawSphere(transform.position, _checkRadius);
        // Gizmos.color = Color.red;
        // Gizmos.DrawSphere(transform.position, AttackRadius);
    }
}