using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    [HideInInspector]
    public bool myTurn;
    public bool isBattle;
    public bool isTurnLoad = false;
    private Player player;
    private Enemy enemy;
    [SerializeField] private GameObject GameBoard;
    private GameObject[] BoardCardArray;
    [SerializeField] GameObject TurnEndOb;

    public enum Team
    {
        player,
        enemy
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        TurnEndOb = GameObject.Find("TurnEnd");
        TurnEndOb.GetComponent<Button>().onClick.AddListener(() => TurnEnd());
    }

    void Start()
    {

    }

    void reSet()
    {
        
    }

    public void StartBattle(Enemy enemy, Player player)
    {
        isBattle = true;
        this.enemy = enemy;
        this.player = player;

        OnUI();

        for (int i = 0; i < 5; i++) StartCoroutine(DropCard(i * 0.2f));
        TurnStart();
    }

    IEnumerator DropCard(float time) //카드를 time초 후에 드롭
    {
        yield return new WaitForSeconds(time);
        CardManager.Instance.AddCard(true);
    }

    public Team CheckSpeed()
    {
        if (enemy.status.Speed <= player.status.Speed) return Team.player;
        else return Team.enemy;
    }

    void EndBattle(Team loseTeam)
    {
        OffUI();
        MapManager.Instance.EndBattle();
    }

    void TurnStart()
    {
        isTurnLoad = false;
        for (int i = 0; i < 2; i++) StartCoroutine(DropCard(i * 0.2f));

        int count;
        if (CheckSpeed() == Team.enemy) count = 3;
        else count = 2;

        int[] AddRandom = new int[count];
        for (int i = 0; i < count; i++)
        {
            while (true)
            {
                bool isSame = false;
                AddRandom[i] = Random.Range(0, 5);
                for (int j = 0; j < i; j++)
                {
                    if (AddRandom[j] == AddRandom[i])
                    {
                        isSame = true;
                        break;
                    }
                }
                if (!isSame) break;
            }
            Debug.Log(AddRandom[i]);
            CardManager.Instance.AddCard(false);
            CardManager.Instance.CardOnBoard(GameBoard.transform.GetChild(AddRandom[i]).gameObject, CardManager.Instance.ReturnOtherCard(i), false);
        }
    }

    public void TurnEnd()
    {
        if (isTurnLoad) return;
        isTurnLoad = true;

        StartCoroutine(CardManager.Instance.PlayCard());

        if (enemy.status.Hp <= 0) EndBattle(Team.enemy);
        else if (player.status.Hp <= 0) EndBattle(Team.player);

        StartCoroutine(SkillManager.Instance.PlayerDaggerAttack());
    }

    public bool IsEndBattle()
    {
        if (enemy.status.Hp <= 0 || player.status.Hp <= 0) return true;
        else return false;
    }

    void Update()
    {

    }

    void OnUI()
    {

    }

    void OffUI()
    {

    }
}
