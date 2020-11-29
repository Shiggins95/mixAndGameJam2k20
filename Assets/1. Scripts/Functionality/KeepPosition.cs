using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepPosition : MonoBehaviour
{
    private Vector2 _startingPos;
    // Start is called before the first frame update
    void Start()
    {
        _startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _startingPos;
    }
}
