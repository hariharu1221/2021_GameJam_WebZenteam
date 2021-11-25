using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public string name;
    public int attack;
    public int health;
    public Sprite sprite;
    public float percent;
}

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Object/CardData")]
public class CardData : ScriptableObject
{
    public Card[] cards;
}