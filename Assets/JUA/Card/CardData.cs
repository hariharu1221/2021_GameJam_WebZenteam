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
    BasicAttack,        //기본 공격
    DoubleUp,           //두번 공격
    HeadShot,           //헤드샷-크리티컬
    LittleSacrifice,    //방어력 낮추고 hp회복
    TheCollector,       //상대의 hp가 10이하일 때 즉시 사살
    Ignite,             //점화
    BattleFury,         //공격력 증가
    Bloodlust,          //공격 후 체력 회복
    BreakTheMold,       //상대 방여력 수치 감소
    LastStand,          //자신의 hp가 60% 이하일시 공격력 증가
    Hemorrhage          //2턴 출혈
}