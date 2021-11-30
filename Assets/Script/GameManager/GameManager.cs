using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    private void Awake()
    {
        if (instance = null) instance = this;
    }

    //void Start()
    //{

    //}

    void Update()
    {

    }
}
