using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class WaveManager : MonoBehaviour
{
    public int NumberOfWaves;

    public int EnemiesPerWave;

    public float TimeBetweenWaves;

    public bool EnemiesStillRemainingIn;

    public List<Enemy> EnemyTypesForEachWave;

    public float EnemyMultiplierPerWave;

    public int CurrentWave;

    private int _internalWaveCounter;

    public float LevelMultiplier;

    // to periodically check if all enemies are dead - avoids checking EVERY frame
    public float PeriodicCheck;

    private float _periodicCheck;

    private float _timeRemainingForNextWave;

    private List<Transform> _spawnPoints;

    public int CurrentSpawnPoint;

    private bool GameStarted;

    private Turret[] _currentTurrets;

    public TextMeshProUGUI Countdown;
    public GameObject ContinueButton;
    public GameObject AdjustButton;
    public GameObject PlayAgainButton;
    public GameObject EndGameButton;
    public TextMeshProUGUI WaveText;
    public TextMeshProUGUI ContinueText;
    public TextMeshProUGUI WaveCompleteText;
    public GameObject Backgdrop;

    private Unlockables _unlockables;
    public Button PlayButton;
    public Button RestartButton;
    public int NumberOfRestarts;
    private int _restartsRemaining;

    private void Start()
    {
        _restartsRemaining = NumberOfRestarts;
        _internalWaveCounter = 0;
        _timeRemainingForNextWave = TimeBetweenWaves;
        _periodicCheck = PeriodicCheck;
        CurrentSpawnPoint = 0;
        _unlockables = GameObject.FindObjectOfType<Unlockables>();
        _currentTurrets = FindObjectsOfType<Turret>();
        GameObject[] _spawnPointsRaw = GameObject.FindGameObjectsWithTag("Spawn");
        _spawnPoints = new List<Transform>();
        foreach (GameObject spawnPoint in _spawnPointsRaw)
        {
            _spawnPoints.Add(spawnPoint.transform);
        }
    }

    private void Update()
    {
        if (GameStarted)
        {
            GameStarted = false;
            StartCoroutine(Go());
        }

        if (!GameStateManager.IsPlaying)
        {
            GameObject[] goArr = GameObject.FindGameObjectsWithTag("Turret");
            if (goArr.Length == 0)
            {
                StartCoroutine(EndGame());
            }

            return;
        }

        if (_timeRemainingForNextWave <= 0 || _periodicCheck <= 0 && !GameStateManager.Restart)
        {
            GameObject[] goArr = GameObject.FindGameObjectsWithTag("Enemy");
            if (goArr.Length == 0)
            {
                Turret[] turrets = FindObjectsOfType<Turret>();
                foreach (Turret turret in _currentTurrets)
                {
                    if (!turret.gameObject.activeSelf)
                    {
                        Destroy(turret.gameObject);
                    }
                }
                Backgdrop.GetComponent<Animator>().Play("Backgdrop");
                EnemiesPerWave = Mathf.CeilToInt(EnemiesPerWave * EnemyMultiplierPerWave);
                RestartButton.gameObject.SetActive(false);
                StartCoroutine(NextWave(true));

                _periodicCheck = PeriodicCheck;
                _timeRemainingForNextWave = TimeBetweenWaves;
            }

            goArr = GameObject.FindGameObjectsWithTag("Turret");
            if (goArr.Length == 0)
            {
                Backgdrop.GetComponent<Animator>().Play("Backgdrop");
                RestartButton.gameObject.SetActive(false);
                StartCoroutine(EndGame());
            }
        }

        if (GameStateManager.IsPlaying && !GameStateManager.Restart)
        {
            _periodicCheck -= Time.deltaTime;
            _timeRemainingForNextWave -= Time.deltaTime;
        }
    }

    public void RestartGame()
    {
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        // RESTART LOGIC //////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        
        foreach (Turret turret in _currentTurrets)
        {
            SpawnPoint spawnPoint = turret.FindSpawnPoint();
            if (spawnPoint)
            {
                turret.transform.position = spawnPoint.transform.position;
                spawnPoint.OccupiedGun = turret.gameObject;
                turret.gameObject.SetActive(true);
                turret.transform.Rotate(new Vector3(0, 0, 0));
            }
        }

        GameStateManager.CanMoveTurrets = true;
        GameStateManager.IsPlaying = false;
        PlayButton.gameObject.SetActive(true);
        RestartButton.gameObject.SetActive(false);

        StartCoroutine(NextWave(false));

        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1f);
        WaveCompleteText.gameObject.SetActive(true);
        WaveCompleteText.text = "Game Over";
        GameStateManager.IsPlaying = false;
        yield return new WaitForSeconds(1f);
        WaveText.gameObject.SetActive(true);
        WaveText.text = $"Would you like to start again?";
        yield return new WaitForSeconds(1f);
        PlayAgainButton.SetActive(true);
        EndGameButton.SetActive(true);
    }

    private IEnumerator NextWave(bool increment)
    {
        GameStateManager.IsPlaying = false;
        yield return new WaitForSeconds(1f);
        if (_internalWaveCounter == EnemyTypesForEachWave.Count - 1)
        {
            _internalWaveCounter = 0;
        }
        else if (increment)
        {
            WaveCompleteText.gameObject.SetActive(true);
            WaveCompleteText.text = "Wave Complete";
            CurrentWave++;
            _internalWaveCounter++;
        }

        yield return new WaitForSeconds(1);

        if (increment)
        {
            int numberOfUnlockables = _unlockables.GetWuantityForWave(CurrentWave + 1);
            if (numberOfUnlockables > 0)
            {
                GameObject unlockType = _unlockables.GetObjectForWave(CurrentWave + 1);
                for (int i = 0; i < numberOfUnlockables; i++)
                {
                    GameObject unlock = Instantiate(unlockType);
                }
                ContinueText.gameObject.SetActive(true);
                ContinueText.text = $"You unlocked some more guns! Please assign them.";
                yield return new WaitForSeconds(1f);
                AdjustButton.gameObject.SetActive(true);
            }
            else
            {
                WaveText.gameObject.SetActive(true);
                WaveText.text = $"Next Wave: Wave {CurrentWave + 1}";
                yield return new WaitForSeconds(1);
                ContinueText.gameObject.SetActive(true);
                ContinueText.text = "Would you like to continue, or make adjustments?";
                ContinueButton.SetActive(true);
                AdjustButton.SetActive(true);
            }
        }
        else
        {
            PlayButton.gameObject.SetActive(true);
        }

        // }
    }

    private IEnumerator Go()
    {
        PlayButton.gameObject.SetActive(false);
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("ContinueButtons");
        GameObject[] textObjects = GameObject.FindGameObjectsWithTag("DisableableText");
        foreach (GameObject _button in buttons)
        {
            _button.SetActive(false);
        }

        foreach (GameObject textObject in textObjects)
        {
            textObject.SetActive(false);
        }

        Countdown.gameObject.SetActive(true);
        Countdown.text = "3";
        yield return new WaitForSeconds(1f);
        Countdown.text = "2";
        yield return new WaitForSeconds(1f);
        Countdown.text = "1";
        yield return new WaitForSeconds(1f);
        Countdown.gameObject.SetActive(false);
        RestartButton.gameObject.SetActive(true);
        GameStateManager.IsPlaying = true;
    }

    public void StartWave()
    {
        Enemy enemy = EnemyTypesForEachWave[_internalWaveCounter];
        Turret[] turrets = FindObjectsOfType<Turret>();
        _currentTurrets = turrets;

        // foreach (Turret turret in turrets)
        // {
        //     turret.ResetHealth();
        // }

        GameStarted = true;
        for (int i = 0; i < EnemiesPerWave; i++)
        {
            Enemy _enemy = Instantiate(enemy, _spawnPoints[CurrentSpawnPoint]);
            // TODO: ADD MORE SPAWN POINTS TO CREATE A BIT LESS UNIFORMITY
            // TODO: MAYBE ALLOW COLLISION BETWEEN ENEMIES
            if (CurrentSpawnPoint == _spawnPoints.Count - 1)
            {
                CurrentSpawnPoint = 0;
            }
            else
            {
                CurrentSpawnPoint++;
            }
        }

        GameStateManager.Restart = false;
    }
}