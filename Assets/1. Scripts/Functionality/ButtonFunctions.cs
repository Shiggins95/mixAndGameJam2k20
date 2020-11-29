using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    public GameObject StartButton;
    public GameObject RestartButton;
    public void StartGame()
    {
        if (CanProceed())
        {
            StartButton.SetActive(false);
            WaveManager wm = FindObjectOfType<WaveManager>();
            GameStateManager.CanMoveTurrets = false;
            wm.StartWave();
        }
        else
        {
            TriggerMessage();
        }
    }

    public void Restart()
    {
        FindObjectOfType<WaveManager>().RestartGame();
    }

    public void Adjust()
    {
        StartButton.SetActive(true);
        RestartButton.SetActive(false);
        GameObject.FindGameObjectWithTag("Backdrop").GetComponent<Animator>().Play("BGIdle");
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("ContinueButtons");
        GameObject[] textObjects = GameObject.FindGameObjectsWithTag("DisableableText");
        GameStateManager.CanMoveTurrets = true;
        foreach (GameObject _button in buttons)
        {
            _button.SetActive(false);
        }

        foreach (GameObject textObject in textObjects)
        {
            textObject.SetActive(false);
        }
    }

    public void PlayOn()
    {
        if (CanProceed())
        {
            WaveManager wm = FindObjectOfType<WaveManager>();
            GameStateManager.CanMoveTurrets = false;
            wm.StartWave();
            GameObject.FindGameObjectWithTag("Backdrop").GetComponent<Animator>().Play("BGIdle");
        }
        else
        {
            TriggerMessage();
        }
    }

    public void StartAgain()
    {
        GameStateManager.CanMoveTurrets = true;
        GameStateManager.IsPlaying = false;
        GameObject.FindGameObjectWithTag("Backdrop").GetComponent<Animator>().Play("BGIdle");
        SceneManager.LoadScene("Game");
    }

    public void GameOver()
    {
        GameStateManager.CanMoveTurrets = true;
        GameStateManager.IsPlaying = false;
        GameObject.FindGameObjectWithTag("Backdrop").GetComponent<Animator>().Play("BGIdle");
        SceneManager.LoadScene("StartScreen");
    }

    private bool CanProceed()
    {
        bool canProceed = true;
        GameObject baseThreshold = GameObject.FindGameObjectWithTag("Base");
        Turret[] turrets = FindObjectsOfType<Turret>();
        foreach (Turret turret in turrets)
        {
            if (turret.transform.position.x < baseThreshold.transform.position.x)
            {
                canProceed = false;
            }
        }

        if (canProceed)
        {
            SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
            foreach (SpawnPoint spawnPoint in spawnPoints)
            {
                spawnPoint.OccupiedGun = null;
            }
        }

        return canProceed;
    }

    private void TriggerMessage()
    {
        Debug.Log($"Cant proceed");
        GameObject turretWarning = GameObject.FindGameObjectWithTag("TurretWarning");
        turretWarning.GetComponent<Animator>().SetBool("IsOpen", true);
    }
}