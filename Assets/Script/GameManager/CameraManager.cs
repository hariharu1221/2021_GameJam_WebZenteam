using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    public SpriteRenderer viewEntity;
    Camera camera;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }

    void Start()
    {
        camera = gameObject.GetComponent<Camera>();
    }


    void Update()
    {
        
    }
    public void SetPosWithSize(Vector3 vec, float size, float time)
    {
        gameObject.transform.DOMove(vec, time);
        StartCoroutine(CameraSize(size, time));
    }

    IEnumerator CameraSize(float size, float time)
    {
        float cool = 0;
        float or = size - camera.orthographicSize;
        float origin = camera.orthographicSize;
        while (time > cool)
        {
            float i = origin + or * (cool / time);
            camera.orthographicSize = i;
            cool += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        camera.orthographicSize = size;
    }

    public void ViewEntity(bool view)
    {
        if (view) StartCoroutine(UIManager.Instance.FadeOutObject(viewEntity, 0.2f, 0f, 2f));
        else StartCoroutine(UIManager.Instance.FadeInObject(viewEntity, 0.2f, 0f, 2f));
    }
}
 