using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    [HideInInspector]
    public bool myTurn;
    private Player player;
    private Enemy enemy;
    private bool isBattle;

    public enum Team
    {
        player,
        enemy
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
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
    }

    IEnumerator DropCard(float time)
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
        for (int i = 0; i < 2; i++) StartCoroutine(DropCard(i * 0.2f));
    }

    void TurnEnd()
    {
        if (enemy.status.Hp <= 0) EndBattle(Team.enemy);
        else if (player.status.Hp <= 0) EndBattle(Team.player);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) CardManager.Instance.ResetCard();
    }

    void OnUI()
    {

    }

    void OffUI()
    {

    }
}
