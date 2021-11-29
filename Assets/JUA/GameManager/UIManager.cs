using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject UI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }

    IEnumerator UIFade(float time, bool In)
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

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
