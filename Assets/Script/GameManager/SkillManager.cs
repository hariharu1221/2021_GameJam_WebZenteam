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

    IEnumerator PlayerGunAttack(float percent)
    {
        CameraManager.Instance.SetPosWithSize(new Vector3(-3f, 2.7f, -100), 3, 0.2f);
        SetEntitySprite(player.gameObject, player.sprites.gunReady);
        CameraManager.Instance.ViewEntity(true);
        yield return new WaitForSeconds(0.75f);

        StartCoroutine(ShowEffect(true, "GunEffect", 0.2f, 0.3f, 0.2f));
        SoundManager.Instance.PlaySFXSound("GunAttack", 0.6f);
        SetEntitySprite(player.gameObject, player.sprites.gunAttack);
        yield return new WaitForSeconds(0.2f);


        CameraManager.Instance.SetPosWithSize(new Vector3(3f, 2.7f, -100), 3, 0.2f);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(ShowEffect(false, "DamagedByGun", 0.1f, 0.3f, 0.2f));
        SetEntitySprite(enemy.gameObject, enemy.sprites.damaged);
        PlusMoveDotWeen(enemy.gameObject, new Vector3(1.3f, 0, 0), 0.2f);
        enemy.status.Hp -= CheckDamage(percent, player.status.Attack, enemy.status.Defense);

        //주변 효과
        yield return new WaitForSeconds(0.5f);

        CameraManager.Instance.ViewEntity(false);
        SetEntitySprite(enemy.gameObject, enemy.sprites.idle);
        PlusMoveDotWeen(enemy.gameObject, new Vector3(-1.3f, 0, 0), 0.5f);
        SetEntitySprite(player.gameObject, player.sprites.idle);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator PlayerDaggerAttack(float percent, bool blood = false)
    {
        yield return StartCoroutine(Dash(true));

        CameraManager.Instance.SetPosWithSize(new Vector3(3f, 2.7f, -100), 3, 0.2f);
        yield return new WaitForSeconds(0.2f);
        CameraManager.Instance.ViewEntity(false);

        SetEntitySprite(player.gameObject, player.sprites.swordAttack);
        PlusMoveDotWeen(player.gameObject, new Vector3(0.5f, 0, 0), 0.1f);
        StartCoroutine(ShowEffect(true, "DaggerEffect", 0.2f, 0.3f, 0.2f));
        if (blood) StartCoroutine(ShowEffect(false, "Damaged", 0.2f, 0.3f, 0.2f));
        StartCoroutine(ShowEffect(false, "DamagedByDagger", 0.2f, 0.3f, 0.2f));
        SoundManager.Instance.PlaySFXSound("DaggerAttack", 0.6f);
        CameraManager.Instance.SetPosWithSize(new Vector3(3f, 2.6f, -100), 3, 0.2f);
        yield return new WaitForSeconds(0.25f);

        SetEntitySprite(enemy.gameObject, enemy.sprites.damaged);
        PlusMoveDotWeen(enemy.gameObject, new Vector3(1.3f, 0, 0), 0.2f);
        enemy.status.Hp -= CheckDamage(percent, player.status.Attack, enemy.status.Defense);
        if (blood) enemy.status.Hp += CheckDamage(percent, enemy.status.Attack, player.status.Defense);
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(DashBack(true));
    }

    IEnumerator PlayerDoubleAttack(float percent)
    {
        yield return StartCoroutine(PlayerDaggerAttack(percent));
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(PlayerGunAttack(percent));
    }

    IEnumerator PlayerHeadShot(float percent)
    {
        CameraManager.Instance.SetPosWithSize(new Vector3(-3f, 2.7f, -100), 3, 0.2f);
        SetEntitySprite(player.gameObject, player.sprites.gunReady);
        CameraManager.Instance.ViewEntity(true);
        yield return new WaitForSeconds(0.75f);

        CameraManager.Instance.SetPosWithSize(new Vector3(3f, 2.7f, -100), 3, 0.4f);
        StartCoroutine(ShowEffect(true, "HeadShotReady", 0.1f, 2f, 0.3f));
        yield return new WaitForSeconds(0.8f);

        CameraManager.Instance.SetPosWithSize(new Vector3(-3f, 2.7f, -100), 3, 0.4f);
        yield return new WaitForSeconds(0.6f);

        CameraManager.Instance.SetPosWithSize(new Vector3(0f, 2.7f, -100), 4, 0.4f);
        StartCoroutine(ShowEffect(true, "HeadShotAttack", 0.1f, 0.3f, 0.2f));
        StartCoroutine(ShowEffect(true, "GunEffect", 0.2f, 0.3f, 0.2f));
        SoundManager.Instance.PlaySFXSound("HeadShot", 0.6f);
        SetEntitySprite(player.gameObject, player.sprites.gunAttack);
        yield return new WaitForSeconds(0.4f);

        StartCoroutine(ShowEffect(false, "DamagedByGun", 0.2f, 0.3f, 0.2f));

        enemy.status.Hp -= CheckDamage(percent, player.status.Attack, enemy.status.Defense);
        SetEntitySprite(enemy.gameObject, enemy.sprites.damaged);
        PlusMoveDotWeen(enemy.gameObject, new Vector3(1.3f, 0, 0), 0.5f);
        yield return new WaitForSeconds(0.9f);

        CameraManager.Instance.ViewEntity(false);
        SetEntitySprite(enemy.gameObject, enemy.sprites.idle);
        PlusMoveDotWeen(enemy.gameObject, new Vector3(-1.3f, 0, 0), 0.5f);
        SetEntitySprite(player.gameObject, player.sprites.idle);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator EX(int hp)
    {
        CameraManager.Instance.SetPosWithSize(new Vector3(-3f, 2.7f, -100), 3, 0.2f);
        SetEntitySprite(player.gameObject, player.sprites.gunReady);
        CameraManager.Instance.ViewEntity(true);
        yield return new WaitForSeconds(0.75f);

        CameraManager.Instance.SetPosWithSize(new Vector3(3f, 2.7f, -100), 3, 0.4f);
        StartCoroutine(ShowEffect(true, "HeadShotReady", 0.1f, 0.3f, 0.2f));
        yield return new WaitForSeconds(0.8f);

        if (enemy.status.Hp <= hp)
        {
            enemy.status.Hp = -999;

            CameraManager.Instance.SetPosWithSize(new Vector3(-3f, 2.7f, -100), 3, 0.4f);
            yield return new WaitForSeconds(0.6f);

            CameraManager.Instance.SetPosWithSize(new Vector3(0f, 2.7f, -100), 4, 0.4f);
            StartCoroutine(ShowEffect(true, "ExOne", 0.1f, 0.3f, 0.1f));
            StartCoroutine(ShowEffect(true, "GunEffect", 0.1f, 0.3f, 0.1f));
            SoundManager.Instance.PlaySFXSound("GunAttack", 0.6f);
            SetEntitySprite(enemy.gameObject, enemy.sprites.damaged);
            yield return new WaitForSeconds(0.5f);
            PlusMoveDotWeen(enemy.gameObject, new Vector3(0.5f, 0, 0), 0.15f);
            StartCoroutine(ShowEffect(false, "DamagedByGun", 0.1f, 2f, 0.1f));

            StartCoroutine(ShowEffect(true, "ExTwo", 0.1f, 0.3f, 0.1f));
            StartCoroutine(ShowEffect(true, "GunEffect", 0.1f, 0.3f, 0.1f));
            SoundManager.Instance.PlaySFXSound("GunAttack", 0.6f);
            yield return new WaitForSeconds(0.5f);
            PlusMoveDotWeen(enemy.gameObject, new Vector3(0.5f, 0, 0), 0.15f);

            StartCoroutine(ShowEffect(true, "HeadShotAttack", 0.1f, 0.3f, 0.2f));
            StartCoroutine(ShowEffect(true, "GunEffect", 0.1f, 0.3f, 0.2f));
            SoundManager.Instance.PlaySFXSound("HeadShot", 0.6f);
            SetEntitySprite(player.gameObject, player.sprites.gunAttack);
            yield return new WaitForSeconds(0.4f);
            PlusMoveDotWeen(enemy.gameObject, new Vector3(9f, 0, 0), 0.9f);


            yield return new WaitForSeconds(0.9f);

            CameraManager.Instance.ViewEntity(false);
            SetEntitySprite(player.gameObject, player.sprites.idle);
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return StartCoroutine(EnemyDaggerAttack(1f));
        }
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

    IEnumerator EnemyDaggerAttack(float percent, bool blood = false)
    {
        yield return StartCoroutine(Dash(false));

        CameraManager.Instance.SetPosWithSize(new Vector3(-3f, 2.7f, -100), 3, 0.2f);
        yield return new WaitForSeconds(0.2f);
        CameraManager.Instance.ViewEntity(false);

        SetEntitySprite(enemy.gameObject, enemy.sprites.swordAttack);
        PlusMoveDotWeen(enemy.gameObject, new Vector3(-0.5f, 0, 0), 0.1f);
        StartCoroutine(ShowEffect(false, "DaggerEffect", 0.2f, 0.3f, 0.2f));
        if (blood) StartCoroutine(ShowEffect(true, "Damaged", 0.2f, 0.3f, 0.2f));
        StartCoroutine(ShowEffect(true, "DamagedByDagger", 0.2f, 0.3f, 0.2f));
        SoundManager.Instance.PlaySFXSound("DaggerAttack", 0.6f);
        CameraManager.Instance.SetPosWithSize(new Vector3(-3f, 2.6f, -100), 3, 0.2f);
        yield return new WaitForSeconds(0.15f);

        SetEntitySprite(player.gameObject, player.sprites.damaged);
        PlusMoveDotWeen(player.gameObject, new Vector3(-1.3f, 0, 0), 0.2f);
        player.status.Hp -= CheckDamage(percent, enemy.status.Attack, player.status.Defense);
        if (blood) enemy.status.Hp += CheckDamage(percent, enemy.status.Attack, player.status.Defense);
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(DashBack(false));
    }

    IEnumerator EnemyDoubleAttack(float percent)
    {
        yield return StartCoroutine(EnemyDaggerAttack(percent));
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(EnemyDaggerAttack(percent));
    }

    IEnumerator Sacrifice(int defense, int hp, bool isMySkill)
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

    IEnumerator BreakTheMold(int defense, bool isMySkill)
    {
        if (isMySkill)
        {
            CameraManager.Instance.SetPosWithSize(new Vector3(3f, 2.7f, -100), 3, 0.2f);
            yield return new WaitForSeconds(0.2f);
            enemy.status.Defense -= defense;
            yield return StartCoroutine(ShowEffect(false, "DecreaseDefense", 0.2f, 0.5f, 0.2f));
        }
        else
        {
            CameraManager.Instance.SetPosWithSize(new Vector3(-3f, 2.7f, -100), 3, 0.2f);
            yield return new WaitForSeconds(0.2f);
            player.status.Defense -= defense;
            yield return StartCoroutine(ShowEffect(true, "DecreaseDefense", 0.2f, 0.5f, 0.2f));
        }
    }

    IEnumerator HealthUp(int hp, bool isMySkill)
    {
        if (isMySkill)
        {
            CameraManager.Instance.SetPosWithSize(new Vector3(-3f, 2.7f, -100), 3, 0.2f);
            yield return new WaitForSeconds(0.2f);
            player.status.MaxHp += hp;
            player.status.Hp += hp;
            yield return StartCoroutine(ShowEffect(true, "Heal", 0.2f, 0.5f, 0.2f));
        }
        else
        {
            CameraManager.Instance.SetPosWithSize(new Vector3(3f, 2.7f, -100), 3, 0.2f);
            yield return new WaitForSeconds(0.2f);
            enemy.status.MaxHp += hp;
            enemy.status.Hp += hp;
            yield return StartCoroutine(ShowEffect(false, "Heal", 0.2f, 0.5f, 0.2f));
        }
    }

    IEnumerator StrengthUp(int strength, bool isMySkill)
    {
        if (isMySkill)
        {
            CameraManager.Instance.SetPosWithSize(new Vector3(-3f, 2.7f, -100), 3, 0.2f);
            yield return new WaitForSeconds(0.2f);
            player.status.Attack += strength;
            yield return StartCoroutine(ShowEffect(true, "IncreaseAttack", 0.2f, 0.5f, 0.2f));
        }
        else
        {
            CameraManager.Instance.SetPosWithSize(new Vector3(3f, 2.7f, -100), 3, 0.2f);
            yield return new WaitForSeconds(0.2f);
            enemy.status.Attack += strength;
            yield return StartCoroutine(ShowEffect(false, "IncreaseAttack", 0.2f, 0.5f, 0.2f));
        }
    }

    IEnumerator DefenseUp(int defense, bool isMySkill)
    {
        if (isMySkill)
        {
            CameraManager.Instance.SetPosWithSize(new Vector3(-3f, 2.7f, -100), 3, 0.2f);
            yield return new WaitForSeconds(0.2f);
            player.status.Defense += defense;
            yield return StartCoroutine(ShowEffect(true, "IncreaseDefense", 0.2f, 0.5f, 0.2f));
        }
        else
        {
            CameraManager.Instance.SetPosWithSize(new Vector3(3f, 2.7f, -100), 3, 0.2f);
            yield return new WaitForSeconds(0.2f);
            enemy.status.Defense += defense;
            yield return StartCoroutine(ShowEffect(false, "IncreaseDefense", 0.2f, 0.5f, 0.2f));
        }
    }

    IEnumerator SpeedUp(int speed, bool isMySkill)
    {
        if (isMySkill)
        {
            CameraManager.Instance.SetPosWithSize(new Vector3(-3f, 2.7f, -100), 3, 0.2f);
            yield return new WaitForSeconds(0.2f);
            player.status.Speed += speed;
            yield return StartCoroutine(ShowEffect(true, "IncreaseAttack", 0.2f, 0.5f, 0.2f));
        }
        else
        {
            CameraManager.Instance.SetPosWithSize(new Vector3(3f, 2.7f, -100), 3, 0.2f);
            yield return new WaitForSeconds(0.2f);
            enemy.status.Speed += speed;
            yield return StartCoroutine(ShowEffect(false, "IncreaseAttack", 0.2f, 0.5f, 0.2f));
        }
    }

    IEnumerator LastStand(float min, float max, bool isMySkill)
    {
        if (isMySkill)
        {
            float n = max - min;
            float percent = (1 - player.status.Hp / player.status.MaxHp) * n + min;
            yield return StartCoroutine(PlayerDaggerAttack(percent));
        }
        else
        {
            float n = max - min;
            float percent = (1 - enemy.status.Hp / enemy.status.MaxHp) * n + min;
            yield return StartCoroutine(EnemyDaggerAttack(percent));
        }
    }


    public int CheckDamage(float percent, int attack, int defense)
    {
        if (attack * percent <= defense * 2) return (int)(attack * percent / 2);
        return (int)(attack * percent) - defense;
    }
    //IEnumerator PlayerSpearAttack


    public IEnumerator returnSkill(CardSkill skill, bool isMySkill, float[] parameter)
    {
        if (skill == CardSkill.BasicAttack && isMySkill)
            return PlayerDaggerAttack(1f);
        else if (skill == CardSkill.BasicAttack && !isMySkill)
            return EnemyDaggerAttack(1f);
        else if (skill == CardSkill.DoubleAttack && isMySkill)
            return PlayerDoubleAttack(1f);
        else if (skill == CardSkill.DoubleAttack && !isMySkill)
            return EnemyDoubleAttack(1f);
        else if (skill == CardSkill.HeadShot && isMySkill)
            return PlayerHeadShot(2f);
        else if (skill == CardSkill.HeadShot && !isMySkill)
            return EnemyDaggerAttack(2f);
        else if (skill == CardSkill.LittleSacrifice)
            return Sacrifice(2, 10, isMySkill);
        else if (skill == CardSkill.Bloodlust && isMySkill)
            return PlayerDaggerAttack(1f, true);
        else if (skill == CardSkill.Bloodlust && !isMySkill)
            return EnemyDaggerAttack(1f, true);
        else if (skill == CardSkill.BreakTheMold)
            return BreakTheMold(4, isMySkill);
        else if (skill == CardSkill.HealthUp)
            return HealthUp((int)(parameter[0]), isMySkill);
        else if (skill == CardSkill.StrengthUp)
            return StrengthUp((int)(parameter[0]), isMySkill);
        else if (skill == CardSkill.DefenseUP)
            return DefenseUp((int)(parameter[0]), isMySkill);
        else if (skill == CardSkill.SpeedUp)
            return SpeedUp((int)(parameter[0]), isMySkill);
        else if (skill == CardSkill.LastStand)
            return LastStand(1.5f, 2.5f, isMySkill);
        else if (skill == CardSkill.TheCollector && isMySkill)
            return EX(50);



        else if (isMySkill) return PlayerDoubleAttack(1f);
        else if (!isMySkill) return EnemyDaggerAttack(1f);

        return EnemyDaggerAttack(1f);
    }

    IEnumerator ShowEffect(bool isMySkill, string name, float show, float ing, float hide)
    {
        SpriteRenderer sr;
        if (isMySkill) sr = player.transform.Find(name).gameObject.GetComponent<SpriteRenderer>();
        else sr = enemy.transform.Find(name).gameObject.GetComponent<SpriteRenderer>();

        //if (sr != null)

        sr.gameObject.SetActive(true);

        yield return StartCoroutine(UIManager.Instance.FadeOutObject(sr, show, 0f));
        yield return StartCoroutine(UIManager.Instance.FadeInObject(sr, hide, ing));

        sr.gameObject.SetActive(false);
    }
}