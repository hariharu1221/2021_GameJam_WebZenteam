using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    [SerializeField] CardData cardData;
    [SerializeField] GameObject cardPrefab;
    List<InCard> myCards = new List<InCard>();
    List<InCard> otherCards = new List<InCard>();
    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] Transform myCardRight;
    [SerializeField] Transform myCardLeft;
    [SerializeField] GameObject cards;
    List<SpriteRenderer> sp = new List<SpriteRenderer>();

    InCard[] BoardCardArray = new InCard[5];
    List<Card> CardBuffer;
    InCard selectCard;
    bool isMyCardDrag;
    bool onMyCardArea;
    enum ECardState { Nothing, CanMouseOver, CanMouseDrag }
    ECardState eCardState;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }

    void SetupCardBuffer()
    {
        CardBuffer = new List<Card>();
        for (int i = 0; i < cardData.cards.Length; i++)
        {
            Card card = cardData.cards[i];
            for (int j = 0; j < card.percent; j++)
                CardBuffer.Add(card);
        }

        for (int i = 0; i < cardData.cards.Length; i++)
        {
            int rand = Random.Range(i, CardBuffer.Count);
            Card temp = CardBuffer[i];
            CardBuffer[i] = CardBuffer[rand];
            CardBuffer[rand] = temp;
        }
    }

    void Start()
    {
        SetupCardBuffer();
    }

    public Card PopCard()
    {
        if (CardBuffer.Count == 0)
            SetupCardBuffer();

        Card card = CardBuffer[0];
        CardBuffer.RemoveAt(0);
        return card;
    }

    public void ResetCard(int type)
    {
        if (type == 1)
        {
            for (int i = 0; i < myCards.Count; i++)
                Destroy(myCards[i].gameObject);
            myCards.Clear();
        }
        else if (type == 2)
        {
            for (int i = 0; i < otherCards.Count; i++)
                Destroy(otherCards[i].gameObject);
            otherCards.Clear();
        }
        else if (type == 3)
        {
            for (int i = 0; i < myCards.Count; i++)
                Destroy(myCards[i].gameObject);
            myCards.Clear();
            for (int i = 0; i < otherCards.Count; i++)
                Destroy(otherCards[i].gameObject);
            otherCards.Clear();
        }
    }

    public void ClearBoardCardArray()
    {
        for (int i = 0; i < BoardCardArray.Length; i++)
        {
            if (BoardCardArray[i] == null) continue;
            Destroy(BoardCardArray[i].gameObject);
            BoardCardArray[i] = null;
        }
    }

    public IEnumerator PlayCard()
    {
        yield return StartCoroutine(UIManager.Instance.UIFade(0.5f, true));
        yield return new WaitForSeconds(0.3f);
        bool isEndbattle = false;
        for (int i = 0; i < BoardCardArray.Length; i++)
        {
            if (BoardCardArray[i] == null) continue;
            yield return StartCoroutine(SkillManager.Instance.returnSkill(BoardCardArray[i].card.skill, BoardCardArray[i].isMyCard));
            yield return new WaitForSeconds(0.2f);
            isEndbattle = TurnManager.Instance.IsEndBattle();
            if (isEndbattle) break;
        }
        ClearBoardCardArray();
        ResetCard(2);
        if (isEndbattle) TurnManager.Instance.IfEndBattle();
        else TurnManager.Instance.TurnStart();
        StartCoroutine(UIManager.Instance.UIFade(1f, false));
    }

    public void AddCard(bool isMine)
    {
        var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
        cardObject.transform.localScale = Vector3.one * 0.7f;
        cardObject.transform.SetParent(cards.transform);
        sp.Add(cardObject.GetComponent<SpriteRenderer>());
        var card = cardObject.GetComponent<InCard>();
        card.Setup(PopCard(), isMine);
        card.isMyCard = isMine ? true : false;
        if (!isMine) card.originPRS = new PRS(Vector3.zero, Quaternion.identity, Vector3.one * 0.6f);
        (isMine ? myCards : otherCards).Add(card);


        SetOriginOrder(isMine);
        if (isMine) CardAlignment(isMine, Vector3.one * 0.6f);
    }

    public void RightCard()
    {
        if (myCards.Count == 0) return;
        myCards.Insert(0, myCards[myCards.Count - 1]);
        myCards.RemoveAt(myCards.Count - 1);
        SetOriginOrder(true);
        CardAlignment(true, Vector3.one * 0.6f);
    }

    void SetOriginOrder(bool isMine)
    {
        int count = isMine ? myCards.Count : otherCards.Count;
        for (int i = 0; i < count; i++)
        {
            var targetCard = isMine ? myCards[i] : otherCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(i + 5);
        }
    }

    void CardAlignment(bool isMine, Vector3 scale)
    {
        List<PRS> originCardPRSs = new List<PRS>();
        if (isMine)
            originCardPRSs = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 1f, scale);

        var targetCards = isMine ? myCards : otherCards;
        for (int i = 0; i < targetCards.Count; i++)
        {
            var targetCard = targetCards[i];

            targetCard.originPRS = originCardPRSs[i];
            targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
        }
    }

    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);

        switch(objCount)
        {
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;
            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                    objLerps[i] = interval * i;
                break;
        }

        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Quaternion.identity;
            if (objCount >= 4)
            {
                if (i == 0 || i == objCount - 1) targetPos.y -= 0.2f;
                float curve = Mathf.Sqrt(Mathf.Pow(height, 10) + Mathf.Pow(objLerps[i] - 0.5f, 10));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                //curve = Mathf.Abs(((objCount / 2) - i - 1) / (objCount / 2)) * 0.3f;
                //targetPos.y -= curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }
            results.Add(new PRS(targetPos, targetRot, scale));
        }
        return results;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            AddCard(true);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            AddCard(false);

        if (isMyCardDrag)
        {
            CardDrag(selectCard);
        }

        DetectCardArea();
        SetEcardState();
    }

    void CardDrag(InCard card)
    {
        if (!onMyCardArea)
        {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);
        }
    }

    void DetectCardArea()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("MyCardArea");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }

    #region CARD

    public void CardMouseOver(InCard card)
    {
        if (eCardState == ECardState.Nothing) return;

        EnlargeCard(true, card);
    }

    public void CardMouseExit(InCard card)
    {
        EnlargeCard(false, card);
    }

    public void CardMouseDown(InCard card)
    {
        selectCard = card;
        if (eCardState != ECardState.CanMouseDrag) return;
        isMyCardDrag = true;
    }

    public void CardMouseUp(InCard card)
    {
        isMyCardDrag = false;
        if (eCardState != ECardState.CanMouseDrag) return;

        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("GameBoardArea");
        if (Array.Exists(hits, x => x.collider.gameObject.layer == layer)) //카드 레이어에 카드를 놓았을 때
        {
            RaycastHit2D hit = Array.Find(hits, x=>x.collider.gameObject.layer == layer);
            GameObject ob = hit.collider.gameObject;

            int i = int.TryParse(ob.name, out i) ? i : -1;
            if (BoardCardArray[i] != null)
                if (!BoardCardArray[i].isMyCard) //아래 카드가 적 카드인 경우 카드 아웃 후 리턴
                {
                    return;
                }
            for (int k = 0; k < 5; k++)
            {
                if (BoardCardArray[k] == card) CardOutBoard(selectCard , false); //선택된 카드가 이미 그 자리에 있는 경우 카드 아웃
            }
            if (BoardCardArray[i] != null && BoardCardArray[i] != card) CardOutBoard(BoardCardArray[i], false); // 자리가 비어있지 않고 선택된 카드가 아닌 경우 그 자리에 있었던 카드 아웃
            CardOnBoard(ob, selectCard);
        }
        else
        {
            if (selectCard.isSetCard == true) CardOutBoard(selectCard); //세팅되어 있는 카드일 경우 카드 아웃
        }
    }

    public void CardOnBoard(GameObject Board, InCard card, bool mycard = true)
    {
        card.isSetCard = true;
        if (mycard) myCards.Remove(card);
        //else otherCards.Remove(card);

        int i = int.TryParse(Board.name, out i) ? i : -1;
        BoardCardArray[i] = card;
        card.MoveTransform(new PRS(new Vector3(Board.transform.position.x, Board.transform.position.y, 0)
            , Utils.QI, card.originPRS.scale * 0.8f), true);
        card.GetComponent<Order>().SetOriginOrder(3);
        CardAlignment(true, Vector3.one * 0.6f);
    }

    public void CardOutBoard(InCard card, bool move = true, bool mycard = true)
    {
        if (!mycard)
        {
            Destroy(card.gameObject);
        }

        card.isSetCard = false;
        myCards.Add(card);
        int i = Array.IndexOf(BoardCardArray, card);
        if (i >= 0) BoardCardArray[i] = null;
        if (move == true)
        {
            SetOriginOrder(true);
            CardAlignment(true, Vector3.one * 0.6f);
        }
    }

    void EnlargeCard(bool isEnlarge, InCard card)
    {
        if (card.isSetCard) return;
        if (isEnlarge)
        {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -3.15f, -10f);
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 0.8f), false);
        }
        else
            card.MoveTransform(card.originPRS, false);

        card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
    }

    void SetEcardState()
    {
        if (!TurnManager.Instance.isBattle)
            eCardState = ECardState.Nothing;

        else if (MapManager.Instance.active)
            eCardState = ECardState.Nothing;

        else if (TurnManager.Instance.isTurnLoad)
            eCardState = ECardState.Nothing;

        else
            eCardState = ECardState.CanMouseDrag;
    }

    public InCard ReturnOtherCard(int index)
    {
        return otherCards[index];
    }

    #endregion
}