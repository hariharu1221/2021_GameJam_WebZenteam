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
        CameraManager.Instance.SetPosWithSize(new Vector3(-3f, 2.7f, -100), 3, 0.2f);
        SetEntitySprite(player.gameObject, player.sprites.gunReady);
        CameraManager.Instance.ViewEntity(true);
        yield return new WaitForSeconds(0.75f);

        StartCoroutine(ShowEffect(true, "GunEffect", 0.2f, 0.3f, 0.2f));
        SetEntitySprite(player.gameObject, player.sprites.gunAttack);
        yield return new WaitForSeconds(0.2f);


        CameraManager.Instance.SetPosWithSize(new Vector3(3f, 2.7f, -100), 3, 0.2f);
        yield return new WaitForSeconds(0.1f);
        SetEntitySprite(enemy.gameObject, enemy.sprites.damaged);
        PlusMoveDotWeen(enemy.gameObject, new Vector3(1.3f, 0, 0), 0.2f);

        //주변 효과
        yield return new WaitForSeconds(0.5f);

        CameraManager.Instance.ViewEntity(false);
        SetEntitySprite(enemy.gameObject, enemy.sprites.idle);
        PlusMoveDotWeen(enemy.gameObject, new Vector3(-1.3f, 0, 0), 0.5f);
        SetEntitySprite(player.gameObject, player.sprites.idle);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator PlayerHeadShot(float percent)
    {
        CameraManager.Instance.SetPosWithSize(new Vector3(-3f, 2.7f, -100), 3, 0.2f);
        SetEntitySprite(player.gameObject, player.sprites.gunReady);
        CameraManager.Instance.ViewEntity(true);
        yield return new WaitForSeconds(0.75f);

        StartCoroutine(ShowEffect(true, "GunEffect", 0.2f, 0.3f, 0.2f));
        SetEntitySprite(player.gameObject, player.sprites.gunAttack);
        yield return new WaitForSeconds(0.2f);

        CameraManager.Instance.SetPosWithSize(new Vector3(3f, 2.7f, -100), 3, 0.2f);
        yield return new WaitForSeconds(0.1f);

        enemy.status.Hp -= CheckDamage(percent, player.status.Attack, enemy.status.Defense);
        SetEntitySprite(enemy.gameObject, enemy.sprites.damaged);
        PlusMoveDotWeen(enemy.gameObject, new Vector3(1.3f, 0, 0), 0.2f);
        yield return new WaitForSeconds(0.5f);

        CameraManager.Instance.ViewEntity(false);
        SetEntitySprite(enemy.gameObject, enemy.sprites.idle);
        PlusMoveDotWeen(enemy.gameObject, new Vector3(-1.3f, 0, 0), 0.5f);
        SetEntitySprite(player.gameObject, player.sprites.idle);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator PlayerDaggerAttack(float percent)
    {
        yield return StartCoroutine(Dash(true));

        CameraManager.Instance.SetPosWithSize(new Vector3(3f, 2.7f, -100), 3, 0.2f);
        yield return new WaitForSeconds(0.2f);
        CameraManager.Instance.ViewEntity(false);

        SetEntitySprite(player.gameObject, player.sprites.swordAttack);
        PlusMoveDotWeen(player.gameObject, new Vector3(0.5f, 0, 0), 0.1f);
        StartCoroutine(ShowEffect(true, "DaggerEffect", 0.2f, 0.3f, 0.2f));
        SoundManager.Instance.PlaySFXSound("DaggerAttack", 0.6f);
        CameraManager.Instance.SetPosWithSize(new Vector3(3f, 2.6f, -100), 3, 0.2f);
        yield return new WaitForSeconds(0.25f);

        SetEntitySprite(enemy.gameObject, enemy.sprites.damaged);
        PlusMoveDotWeen(enemy.gameObject, new Vector3(1.3f, 0, 0), 0.2f);
        enemy.status.Hp -= CheckDamage(percent, player.status.Attack, enemy.status.Defense);
        //주변 효과
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(DashBack(true));
    }

    IEnumerator Dash(bool isMySkill)
    {
        if (isMySkill)
        {
            player.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 10;
            SetEntitySprite(player.gameObject, player.sprites.swordReady);
            PlusMoveDotWeen(player.gameObject, new Vector3(6.0f, 0, 0), 0.3f);
            CameraManager.Instance.SetPosWithSize(new Vector3(2.6f, 2.6f, -100), 3, 0.4f);
            CameraManager.Instance.ViewEntity(true);
            yield return new WaitForSeconds(0.4f);
        }
        else
        {
            enemy.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 10;
            SetEntitySprite(enemy.gameObject, enemy.sprites.swordReady);
            PlusMoveDotWeen(enemy.gameObject, new Vector3(-6.0f, 0, 0), 0.3f);
            CameraManager.Instance.SetPosWithSize(new Vector3(-2.6f, 2.6f, -100), 3, 0.4f);
            CameraManager.Instance.ViewEntity(true);
            yield return new WaitForSeconds(0.4f);
        }
    }

    IEnumerator DashBack(bool isMySkill)
    {
        if (isMySkill)
        {
            SetEntitySprite(enemy.gameObject, enemy.sprites.idle);
            PlusMoveDotWeen(enemy.gameObject, new Vector3(-1.3f, 0, 0), 0.3f);

            PlusMoveDotWeen(player.gameObject, new Vector3(-6.5f, 0, 0), 0.3f);
            SetEntitySprite(player.gameObject, player.sprites.idle);
            yield return new WaitForSeconds(0.3f);
            player.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
        }
        else
        {
            SetEntitySprite(player.gameObject, player.sprites.idle);
            PlusMoveDotWeen(player.gameObject, new Vector3(1.3f, 0, 0), 0.3f);

            PlusMoveDotWeen(enemy.gameObject, new Vector3(6.5f, 0, 0), 0.3f);
            SetEntitySprite(enemy.gameObject, enemy.sprites.idle);
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator EnemyDaggerAttack(float percent)
    {
        yield return StartCoroutine(Dash(false));

        CameraManager.Instance.SetPosWithSize(new Vector3(-3f, 2.7f, -100), 3, 0.2f);
        yield return new WaitForSeconds(0.2f);
        CameraManager.Instance.ViewEntity(false);

        SetEntitySprite(enemy.gameObject, enemy.sprites.swordAttack);
        PlusMoveDotWeen(enemy.gameObject, new Vector3(-0.5f, 0, 0), 0.1f);

        SoundManager.Instance.PlaySFXSound("DaggerAttack", 0.6f);
        CameraManager.Instance.SetPosWithSize(new Vector3(-3f, 2.6f, -100), 3, 0.2f);
        yield return new WaitForSeconds(0.15f);

        SetEntitySprite(player.gameObject, player.sprites.damaged);
        PlusMoveDotWeen(player.gameObject, new Vector3(-1.3f, 0, 0), 0.2f);
        player.status.Hp -= CheckDamage(percent, enemy.status.Attack, player.status.Defense);
        //주변 효과
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(DashBack(false));
    }

    IEnumerator PlayerSacrifice(int defense, int hp, bool isMySkill)
    {
        if (isMySkill)
        {
            CameraManager.Instance.SetPosWithSize(new Vector3(-3f, 2.7f, -100), 3, 0.2f);
            yield return new WaitForSeconds(0.2f);
            player.status.Defense -= defense;
            player.status.Hp += hp;
            yield return StartCoroutine(ShowEffect(true, "DecreaseDefense", 0.2f, 0.5f, 0.2f));
            yield return StartCoroutine(ShowEffect(true, "Heal", 0.2f, 0.5f, 0.2f));
        }
        else
        {
            CameraManager.Instance.SetPosWithSize(new Vector3(3f, 2.7f, -100), 3, 0.2f);
            yield return new WaitForSeconds(0.2f);
            enemy.status.Defense -= defense;
            enemy.status.Hp += hp;
            yield return StartCoroutine(ShowEffect(false, "DecreaseDefense", 0.2f, 0.5f, 0.2f));
            yield return StartCoroutine(ShowEffect(false, "Heal", 0.2f, 0.5f, 0.2f));
        }
    }


    public int CheckDamage(float percent, int attack, int defense)
    {
        if (attack * percent <= defense * 2) return (int)(attack * percent / 2);
        return (int)(attack * percent) - defense;
    }
    //IEnumerator PlayerSpearAttack


    public IEnumerator returnSkill(CardSkill skill, bool isMySkill)
    {
        if (skill == CardSkill.BasicAttack && isMySkill)
            return PlayerDaggerAttack(1f);
        else if (skill == CardSkill.BasicAttack && !isMySkill)
            return EnemyDaggerAttack(1f);
        else if (skill == CardSkill.HeadShot && isMySkill)
            return PlayerHeadShot(2f);
        else if (skill == CardSkill.HeadShot && !isMySkill)
            return EnemyDaggerAttack(2f);
        else if (skill == CardSkill.LittleSacrifice)
            return PlayerSacrifice(2, 10, isMySkill);



        else if (isMySkill) return PlayerDaggerAttack(1f);
        else if (!isMySkill) return EnemyDaggerAttack(1f);

        return EnemyDaggerAttack(1f);
    }

    IEnumerator ShowEffect(bool isMySkill, string name, float show, float ing, float hide)
    {
        SpriteRenderer sr;
        if (isMySkill) sr = player.transform.Find(name).gameObject.GetComponent<SpriteRenderer>();
        else sr = enemy.transform.Find(name).gameObject.GetComponent<SpriteRenderer>();

        sr.gameObject.SetActive(true);

        yield return StartCoroutine(UIManager.Instance.FadeOutObject(sr, show, 0f));
        yield return StartCoroutine(UIManager.Instance.FadeInObject(sr, hide, ing));

        sr.gameObject.SetActive(false);
    }
}