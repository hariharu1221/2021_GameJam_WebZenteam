using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class InCard : MonoBehaviour
{
    [SerializeField] SpriteRenderer cardSprite;
    [SerializeField] TMP_Text nameT;
    [SerializeField] TMP_Text textT;
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
            this.gameObject.GetComponent<SpriteRenderer>().sprite = this.card.sprite;
            nameT.text = this.card.name;
            textT.text = this.card.skillText;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = this.card.sprite;
            nameT.text = this.card.name;
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(86f / 255, 63f / 255, 204f / 255, 1);
            textT.text = this.card.skillText;
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
