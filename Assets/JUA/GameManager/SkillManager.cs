using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    private Enemy enemy;
    private Player player;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }

    private void Start()
    {

    }

    public void SetEntity(Enemy enemy, Player player)
    {
        this.enemy = enemy;
        this.player = player;
    }

    public void PlusMoveDotWeen(GameObject ob, Vector3 plus, float time)
    {
        Vector3 pos = ob.transform.position;
        pos += plus;
        ob.transform.DOMove(pos, time);
    }

    public void SetEntitySprite(GameObject ob, Sprite sprite)
    {
        ob.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sprite;
        ob.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = sprite;
    }

    IEnumerator PlayerGunAttack()
    {
        yield return new WaitForFixedUpdate();
    }

    IEnumerator PlayerDaggerAttack(float percent)
    {
        player.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 10;
        SetEntitySprite(player.gameObject, player.sprites.swordReady);
        PlusMoveDotWeen(player.gameObject, new Vector3(6.4f, 0, 0), 0.4f);
        yield return new WaitForSeconds(0.4f);

        SetEntitySprite(player.gameObject, player.sprites.swordAttack);
        PlusMoveDotWeen(player.gameObject, new Vector3(0.5f, 0, 0), 0.1f);
        yield return new WaitForSeconds(0.15f);

        SetEntitySprite(enemy.gameObject, enemy.sprites.damaged);
        PlusMoveDotWeen(enemy.gameObject, new Vector3(1.3f, 0, 0), 0.2f);
        enemy.status.Hp -= (int)(player.status.Attack * percent);
        //주변 효과
        yield return new WaitForSeconds(0.5f);

        SetEntitySprite(enemy.gameObject, enemy.sprites.idle);
        PlusMoveDotWeen(enemy.gameObject, new Vector3(-1.3f, 0, 0), 0.5f);

        PlusMoveDotWeen(player.gameObject, new Vector3(-6.9f, 0, 0), 0.4f);
        SetEntitySprite(player.gameObject, player.sprites.idle);
        yield return new WaitForSeconds(0.5f);
        player.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
    }

    //IEnumerator PlayerSpearAttack


    public IEnumerator returnSkill(CardSkill skill, bool isMySkill)
    {
        if (skill == CardSkill.BasicAttack)
            return PlayerDaggerAttack(1f);
        return PlayerDaggerAttack(1f);
    }
}