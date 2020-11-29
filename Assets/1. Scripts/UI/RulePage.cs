using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulePage : MonoBehaviour
{

    public List<GameObject> Pages;
    public int CurrentPage;

    private void Start()
    {
        Pages[0].SetActive(true);
    }
}
