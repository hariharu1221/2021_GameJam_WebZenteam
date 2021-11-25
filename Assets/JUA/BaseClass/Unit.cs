using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{

}

[System.Serializable]
public class Status
{
    [SerializeField]
    int maxhp;
    int hp;
    int speed;
    int attack;
    int defense;
    int action;

    public int MaxHp
    {
        set
        {
            maxhp = value;
        }
        get
        {
            return maxhp;
        }
    }
    public int Hp
    {
        set
        {
            hp = value;
        }
        get
        {
            return hp;
        }
    }
    public int Speed
    {
        set
        {
            speed = value;
        }
        get
        {
            return speed;
        }
    }
    public int Attack
    {
        set
        {
            attack = value;
        }
        get
        {
            return attack;
        }
    }
    public int Defense
    {
        set
        {
            defense = value;
        }
        get
        {
            return defense;
        }
    }
    public int Action
    {
        set
        {
            action = value;
        }
        get
        {
            return action;
        }
    }
}
