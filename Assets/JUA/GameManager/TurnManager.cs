using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    public bool myTurn;
    private Player player;

    private void Awake()
    {
        if (Instance = null) Instance = this;
        else Destroy(this);
    }

    void Start()
    {
        
    }

    public void reSet()
    {
        
    }

    public void StartBattle(Enemy enmey, Player player)
    {
        //if ()
    }

    void EndBattle()
    {

    }

    void Update()
    {
        
    }
}
