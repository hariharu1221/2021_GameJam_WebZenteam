using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map Data", menuName = "Scriptable Object/Map Data", order = int.MaxValue)]
public class Map : ScriptableObject
{
    [SerializeField]
    private BlockLine[] arrayBlocks;

    public int[,] a;

    public BlockLine[] ArrayBlocks
    {
        get {  return arrayBlocks; }
    }
}

[System.Serializable]
public class BlockLine
{
    public Block[] LineBlocks;
}

[System.Serializable]
public class Block
{
    public GameObject enemy;
    public bool isBattle = false;
    public MapEvent mapEvent;
}

public enum MapEvent
{
    A,
    B,
    C
}