using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

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

    GameObject playerStatus;
    GameObject enemyStatus;
    public StatusText playerText;
    public StatusText enemyText;

    public GameObject Victory;
    public GameObject Lose;

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
        playerStatus = GameObject.Find("player_indicator");
        enemyStatus = GameObject.Find("enemy_indicator");
    }

    void Start()
    {
        for (int i = 0; i < 5; i++) StartCoroutine(DropCard(1f + i * 0.2f));
    }

    void reSet()
    {
        
    }

    public void StartBattle(Enemy enemy, Player player)
    {
        StartCoroutine(UIManager.Instance.ShowBattleStart());
        isBattle = true;
        this.enemy = enemy;
        this.player = player;

        OnUI();

        TurnStart();
    }

    IEnumerator DropCard(float time) //카드를 time초 후에 드롭
    {
        yield return new WaitForSeconds(time);
        CardManager.Instance.AddCard(true);
        yield return new WaitForSeconds(0.15f);
        SoundManager.Instance.PlaySFXSound("DropCard", 0.4f);
    }

    public Team CheckSpeed()
    {
        if (enemy.status.Speed <= player.status.Speed) return Team.player;
        else return Team.enemy;
    }

    void EndBattle(Team loseTeam)
    {
        OffUI();
        if (loseTeam == Team.enemy)
        {
            for (int i = 0; i < 2; i++) StartCoroutine(DropCard(i * 0.2f));
            MapManager.Instance.EndBattle();
        }
        else
        {

        }
    }

    public void IfEndBattle()
    {
        if (enemy.status.Hp <= 0) EndBattle(Team.enemy);
        else if (player.status.Hp <= 0) EndBattle(Team.player);
    }

    public void TurnStart()
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
            CardManager.Instance.AddCard(false);
            CardManager.Instance.CardOnBoard(GameBoard.transform.GetChild(AddRandom[i]).gameObject, CardManager.Instance.ReturnOtherCard(i), false);
        }
    }

    public void TurnEnd()
    {
        if (!isBattle) return;
        if (isTurnLoad) return;
        isTurnLoad = true;

        StartCoroutine(CardManager.Instance.PlayCard());

        if (enemy.status.Hp <= 0) EndBattle(Team.enemy);
        else if (player.status.Hp <= 0) EndBattle(Team.player);
    }

    public bool IsEndBattle()
    {
        if (enemy.status.Hp <= 0 || player.status.Hp <= 0) return true;
        else return false;
    }

    void Update()
    {
        if (player != null) 
        {
            playerText.hpTM.text = player.status.Hp.ToString() + " / " + player.status.MaxHp.ToString();
            playerText.attackTM.text = player.status.Attack.ToString();
            playerText.defenseTM.text = player.status.Defense.ToString();
            playerText.speedTM.text = player.status.Speed.ToString();
        }
        if (enemy != null)
        {
            enemyText.hpTM.text = enemy.status.Hp.ToString() + " / " + enemy.status.MaxHp.ToString();
            enemyText.attackTM.text = enemy.status.Attack.ToString();
            enemyText.defenseTM.text = enemy.status.Defense.ToString();
            enemyText.speedTM.text = enemy.status.Speed.ToString();
        }
    }



    void OnUI()
    {

    }

    void OffUI()
    {

    }
}


[System.Serializable]
public class StatusText
{
    public TextMeshPro hpTM;
    public TextMeshPro speedTM;
    public TextMeshPro attackTM;
    public TextMeshPro defenseTM;
}