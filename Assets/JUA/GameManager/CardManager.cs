using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    [SerializeField] CardData cardData;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] List<InCard> myCards;
    [SerializeField] List<InCard> otherCards;
    [SerializeField] Transform cardSpawnPoint;
    [SerializeField] Transform myCardRight;
    [SerializeField] Transform myCardLeft;
    [SerializeField] GameObject cards;
    [SerializeField] List<SpriteRenderer> sp = new List<SpriteRenderer>();

    List<Card> CardBuffer;

    void Awake() => Instance = this;

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

    public void ResetCard()
    {
        myCards.Clear();
        otherCards.Clear();

    }

    public void AddCard(bool isMine)
    {
        var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
        cardObject.transform.localScale = Vector3.one * 0.7f;
        cardObject.transform.SetParent(cards.transform);
        sp.Add(cardObject.GetComponent<SpriteRenderer>());
        var card = cardObject.GetComponent<InCard>();
        card.Setup(PopCard(), isMine);
        (isMine ? myCards : otherCards).Add(card);

        SetOriginOrder(isMine);
        if (isMine) CardAlignment(isMine, Vector3.one * 0.6f);
    }

    void SetOriginOrder(bool isMine)
    {
        int count = isMine ? myCards.Count : otherCards.Count;
        for (int i = 0; i < count; i++)
        {
            var targetCard = isMine ? myCards[i] : otherCards[i];
            targetCard?.GetComponent<Order>().SetOriginOrder(i);
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
    }
}
