using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class InCard : MonoBehaviour
{
    [SerializeField] SpriteRenderer cardSprite;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] TMP_Text nameT;
    [SerializeField] TMP_Text attack;
    [SerializeField] TMP_Text health;
    [SerializeField] Sprite cardFront;
    [SerializeField] Sprite cardBack;

    public Card card;
    bool isFront;
    public PRS originPRS;
    public bool isMyCard;
    public bool isSetCard = false;

    public void Setup(Card card, bool isFront)
    {
        this.card = card;
        this.isFront = isFront;

        if (this.isFront)
        {
            sprite.sprite = this.card.sprite;
            nameT.text = this.card.name;
            attack.text = this.card.attack.ToString();
            health.text = this.card.health.ToString();
        }
        else
        {
            sprite.sprite = this.card.sprite;
            nameT.text = this.card.name;
            attack.text = this.card.attack.ToString();
            health.text = this.card.health.ToString();
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(86f / 255, 63f / 255, 204f / 255, 1);
        }
    }

    private void OnMouseOver()
    {
        CardManager.Instance.CardMouseOver(this);
    }

    private void OnMouseExit()
    {
        CardManager.Instance.CardMouseExit(this);
    }

    private void OnMouseDown()
    {
        if (!isMyCard) return;
        CardManager.Instance.CardMouseDown(this);
    }

    private void OnMouseUp()
    {
        if (!isMyCard) return;
        CardManager.Instance.CardMouseUp(this);
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if (useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
}
