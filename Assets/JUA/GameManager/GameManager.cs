using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    public TurnManager turnManager;
    public MapManager mapManager;

    private void Awake()
    {
        if (instance = null) instance = this;
    }

    void Start()
    {
        if (turnManager == null) turnManager = gameObject.GetComponent<TurnManager>();
        if (mapManager == null) mapManager = gameObject.GetComponent<MapManager>();

        SetManager();
    }

    void SetManager()
    {
        mapManager.Set();
        turnManager.Set();
    }

    void Update()
    {
        mapManager.MapUpdate();

    }
}
