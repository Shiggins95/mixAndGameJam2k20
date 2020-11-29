using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    public int Health;
    public int HealthRemaining;
    [SerializeField] private HealthBar _healthBarPrefab;
    private HealthBar _healthBar;
    private Transform _canvas;
    public Vector3 Offset;

    private void Start()
    {
        HealthRemaining = Health;
        SpawnPoint spawnPoint = FindSpawnPoint();
        if (spawnPoint)
        {
            transform.position = spawnPoint.transform.position;
            spawnPoint.OccupiedGun = gameObject;
        }

        _canvas = GameObject.FindGameObjectWithTag("HealthBarCanvas").transform;
        _healthBar = Instantiate(_healthBarPrefab, _canvas.transform);
        _healthBar.transform.SetParent(_canvas.transform);;
        _healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position - new Vector3(1, 1, 1));
        _healthBar.SetMaxHealth(Health);
        _healthBar.SetHealth(Health);
    }

    public SpawnPoint FindSpawnPoint()
    {
        SpawnPoint[] spawnPointsRaw = FindObjectsOfType<SpawnPoint>();
        List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
        foreach (SpawnPoint spawnPoint in spawnPointsRaw)
        {
            if (!spawnPoint.OccupiedGun)
            {
                spawnPoints.Add(spawnPoint);
            }
        }

        if (spawnPoints.Count == 0)
        {
            return null;
        }

        spawnPoints.Sort((a, b) => a.Index.CompareTo(b.Index));

        return spawnPoints[0];
    }

    private void Update()
    {

        _healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position - Offset);
    }

    public void TakeDamage(int damage)
    {
        HealthRemaining -= damage;
        _healthBar.SetHealth(HealthRemaining);
    }

    public void Die()
    {
        Debug.Log($"turret ded");
        _healthBar.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void ResetHealth()
    {
        HealthRemaining = Health;
        _healthBar.SetHealth(Health);
        if (!_healthBar.gameObject.activeSelf)
        {
            _healthBar.gameObject.SetActive(true);
            // TODO GAME FINISHED NOW - JUST DO ART AND PARTICLES DICKHEAD!!!!!!!
        }
    }
}