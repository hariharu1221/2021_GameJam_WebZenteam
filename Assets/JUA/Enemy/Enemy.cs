using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]

public class Enemy : MonoBehaviour
{
    public Sprites charater;
    public Status status;
}

[System.Serializable]
public class Sprites
{
    public Sprite idle;
    public Sprite hit;
    public Sprite attack;
}