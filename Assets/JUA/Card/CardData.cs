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
    public string skillText;
    public CardSkill skill;
    public float[] skillParameter;
}

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Object/CardData")]
public class CardData : ScriptableObject
{
    public Card[] cards;
}

public enum CardSkill
{
    BasicAttack,        //�⺻ ����
    DoubleUp,           //�ι� ����
    HeadShot,           //��弦-ũ��Ƽ��
    LittleSacrifice,    //���� ���߰� hpȸ��
    TheCollector,       //����� hp�� 10������ �� ��� ���
    Ignite,             //��ȭ
    BattleFury,         //���ݷ� ����
    Bloodlust,          //���� �� ü�� ȸ��
    BreakTheMold,       //��� �濩�� ��ġ ����
    LastStand,          //�ڽ��� hp�� 60% �����Ͻ� ���ݷ� ����
    Hemorrhage          //2�� ����
}