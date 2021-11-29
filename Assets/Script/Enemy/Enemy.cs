using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]

public class Enemy : MonoBehaviour
{
    public Sprites sprites;
    public Status status;
}

[System.Serializable]
public class Sprites
{
    public Sprite idle;
    public Sprite damaged;
    public Sprite gunAttack;
    public Sprite swordAttack;
    public Sprite gunReady;
    public Sprite swordReady;
}