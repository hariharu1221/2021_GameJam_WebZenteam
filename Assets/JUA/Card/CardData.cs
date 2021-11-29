using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public string name;
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
    DoubleAttack,           //�ι� ����
    HeadShot,           //��弦-ũ��Ƽ��
    LittleSacrifice,    //���� ���߰� hpȸ��
    TheCollector,       //����� hp�� 10������ �� ��� ���
    FireBullet,         //��ȭ
    BattlePosture,      //���ݷ� ����
    Bloodlust,          //���� �� ü�� ȸ��
    BreakTheMold,       //��� �濩�� ��ġ ����
    LastStand,          //�ڽ��� hp�� 60% �����Ͻ� ���ݷ� ����
    VitalSpotLunge,
    StealthAttack,
    Hemorrhage          //2�� ����
}