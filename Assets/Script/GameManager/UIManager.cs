using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject UI;
    public SpriteRenderer Fade;

    public GameObject BattleStart;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
        BattleStart.SetActive(false);
    }

    public IEnumerator ShowBattleStart()
    {
        BattleStart.SetActive(true);
        RectTransform rect = BattleStart.GetComponent<RectTransform>();
        CanvasGroup canvas = BattleStart.GetComponent<CanvasGroup>();
        Vector3 vec = Vector3.one;
        float cool = 0;
        while (0.3f > cool)
        {
            vec = Vector3.one * (1 - (cool / 1.5f));
            rect.localScale = vec;
            canvas.alpha = cool / 0.5f;
            cool += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        rect.localScale = Vector3.one * 0.8f;
        canvas.alpha = 1;
        cool = 0;
        yield return new WaitForSeconds(0.1f);
        while (0.4f > cool)
        {
            vec = Vector3.one * (0.8f + (cool / 1.5f));
            rect.localScale = vec;
            canvas.alpha = 1f - cool / 0.5f;
            cool += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        canvas.alpha = 1;
        BattleStart.SetActive(false);
    }

    public IEnumerator UIFade(float time, bool In)
    {
        float cool = 0;
        UI.SetActive(true);
        while (cool < time)
        {
            if (In) UI.GetComponent<CanvasGroup>().alpha = 1f - (cool / time);
            else UI.GetComponent<CanvasGroup>().alpha = cool / time;
            cool += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        if (In) UI.SetActive(false);
        else UI.GetComponent<CanvasGroup>().alpha = 1.0f;
    }

    public IEnumerable UIFade(GameObject ob ,float time, bool In)
    {
        float cool = 0;
        ob.SetActive(true);
        while (cool < time)
        {
            if (In) ob.GetComponent<CanvasGroup>().alpha = 1f - (cool / time);
            else ob.GetComponent<CanvasGroup>().alpha = cool / time;
            cool += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        if (In) ob.SetActive(false);
        else ob.GetComponent<CanvasGroup>().alpha = 1.0f;
    }

    public IEnumerator FadeOutObject(SpriteRenderer[] sr, float time, float delay = 0f)
    {
        foreach (SpriteRenderer sr2 in sr)
        {
            Color color = sr2.color;
            color.a = 0;
            sr2.color = color;
        }
        yield return new WaitForSeconds(delay);
        float cool = 0f;
        while (time > cool)
        {
            foreach (SpriteRenderer sr3 in sr)
            {
                Color color = sr3.color;
                color.a = cool / time;
                sr3.color = color;
            }
            cool += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        foreach (SpriteRenderer sr2 in sr)
        {
            Color color = sr2.color;
            color.a = 1;
            sr2.color = color;
        }
    }

    public IEnumerator FadeOutObject(SpriteRenderer sr, float time, float delay = 0f, float n = 1)
    {
        Color color = sr.color;
        color.a = 0;
        sr.color = color;
        yield return new WaitForSeconds(delay);
        float cool = 0f;
        while (time > cool)
        {
            color.a = cool / time / n;
            sr.color = color;
            cool += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        color.a = 1 / n;
        sr.color = color;
    }

    public IEnumerator FadeInObject(SpriteRenderer[] sr, float time, float delay = 0f)
    {
        foreach (SpriteRenderer sr2 in sr)
        {
            Color color = sr2.color;
            color.a = 1;
            sr2.color = color;
        }
        yield return new WaitForSeconds(delay);
        float cool = 0f;
        while (time > cool)
        {
            foreach (SpriteRenderer sr3 in sr)
            {
                Color color = sr3.color;
                color.a = 1f - cool / time;
                sr3.color = color;
            }
            cool += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        foreach (SpriteRenderer sr2 in sr)
        {
            Color color = sr2.color;
            color.a = 0;
            sr2.color = color;
        }
    }

    public IEnumerator FadeInObject(SpriteRenderer sr, float time, float delay = 0f, float n = 1)
    {
        Color color = sr.color;
        color.a = 1 / n;
        sr.color = color;
        yield return new WaitForSeconds(delay);
        float cool = 0f;
        while (time > cool)
        {
            color.a = (1f / n) - cool / time / n;
            sr.color = color;
            cool += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        color.a = 0;
        sr.color = color;
    }

    public IEnumerator MoveUI(GameObject ob, Vector2 vec, float maxcool, float waittime, bool nowactive = true)
    {
        yield return new WaitForSeconds(waittime);
        ob.SetActive(true);
        float dis = Vector3.Distance(ob.GetComponent<RectTransform>().anchoredPosition, vec);

        while (Vector3.Distance(ob.GetComponent<RectTransform>().anchoredPosition, vec) > 5)
        {
            ob.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(ob.GetComponent<RectTransform>().anchoredPosition,
                vec, Time.deltaTime * maxcool);
            if (nowactive) ob.GetComponent<CanvasGroup>().alpha = Vector3.Distance(ob.GetComponent<RectTransform>().anchoredPosition, vec) / dis;
            else ob.GetComponent<CanvasGroup>().alpha = (dis - Vector3.Distance(ob.GetComponent<RectTransform>().anchoredPosition, vec)) / dis;
            yield return new WaitForFixedUpdate();
        }
        ob.GetComponent<RectTransform>().anchoredPosition = vec;
        if (nowactive)
        {
            ob.GetComponent<CanvasGroup>().alpha = 0;
            ob.SetActive(false);
        }
        else ob.GetComponent<CanvasGroup>().alpha = 1;
    }
}
