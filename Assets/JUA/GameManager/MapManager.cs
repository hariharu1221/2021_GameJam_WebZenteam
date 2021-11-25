using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public GameObject blockPrefab;
    public RectTransform MapBoard;

    [SerializeField]
    private Map mapData;
    [SerializeField]
    private GameObject playerObject;
    private Player player;
    private Point playerPos;
    [SerializeField]
    private GameObject playerUI;

    Node[,] Board;
    [HideInInspector]
    public int height;
    [HideInInspector]
    public int width;

    bool isMove = false;

    enum Dir
    {
        up,
        down,
        right,
        left
    }

    public void Set()
    {
        InitialzieBoard();

        SetButton(playerUI.transform.Find("Up").gameObject, () => Move(Dir.up));
        SetButton(playerUI.transform.Find("Down").gameObject, () => Move(Dir.down));
        SetButton(playerUI.transform.Find("Right").gameObject, () => Move(Dir.right));
        SetButton(playerUI.transform.Find("Left").gameObject, () => Move(Dir.left));
        SetButton(GameObject.Find("ShowMapButton"), MapButton);
        

        player = playerObject.GetComponent<Player>();
        isMove = false;
        setPos(new Point(0,0));
    }

    void InitialzieBoard()
    {
        width = 8;
        height = mapData.ArrayBlocks.Length;
        Board = new Node[height, width];

        Debug.Log(height);

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject n = Instantiate(blockPrefab, MapBoard);
                Board[i, j] = new Node(mapData.ArrayBlocks[i].LineBlocks[j], new Point(j, i), n);
            }
        }
    }

    public void MapUpdate()
    {

    }

    public void MapReloading()
    {

    }

    public void ViewMap()
    {
        
    }

    void setPos(Point p)
    {
        playerPos = p;
        Vector2 pos = new Vector2(110 + (1600 * p.x / 8), -60 -
             (500 * p.y / 5));
        playerUI.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    void Move(Dir dir)
    {
        if (isMove) return;
        isMove = true;

        Point p = playerPos;
        switch(dir)
        {
            case Dir.up:
                if (p.y == 0) return;
                p.y -= 1;
                break;
            case Dir.down:
                if (p.y == height - 1) return;
                p.y += 1;
                break;
            case Dir.left:
                if (p.x == 0) return;
                p.x -= 1;
                break;
            case Dir.right:
                if (p.x == width - 1) return;
                p.x += 1;
                break;
        }

        setPos(p);
        if (Board[p.x, p.y].getBlock().enemy != null)
        {
            StartCoroutine(MoveUI(MapBoard.gameObject, new Vector2(0, -500), 2f, 1));
        }
        else
        {
            isMove = false;
        }

    }

    void SetButton(GameObject ob, UnityEngine.Events.UnityAction act)
    {
        Button BT = ob.GetComponent<Button>();
        BT.onClick.AddListener(act);
    }

    Coroutine coroutine;
    void MapButton()
    {
        if (MapBoard.gameObject.activeSelf)
            coroutine = StartCoroutine(MoveUI(MapBoard.gameObject, new Vector2(0, -350), 3f, 0.1f));
        else
            coroutine = StartCoroutine(MoveUI(MapBoard.gameObject, new Vector2(0, 300), 3f, 0.1f, false));
    }

    IEnumerator MoveUI(GameObject ob, Vector2 vec, float maxcool, float waittime, bool nowactive = true)
    {
        yield return new WaitForSeconds(waittime);
        ob.SetActive(true);
        float dis = Vector3.Distance(ob.GetComponent<RectTransform>().anchoredPosition, vec);

        coroutine = null;
        while (Vector3.Distance(ob.GetComponent<RectTransform>().anchoredPosition, vec) > 5)
        {
            if (coroutine != null) yield break;
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

public class Node
{
    public Block block;
    public Point index;
    public GameObject ob;

    public Node(Block b, Point p, GameObject n)
    {
        block = b;
        index = p;
        ob = n;

        setBlockPos();
    }

    public Block getBlock()
    {
        return block;
    }

    void setBlockPos()
    {
        Vector2 pos = new Vector2(110 + (1600 * index.x / 8), -60 - 
             (500 * index.y / 5));
        ob.GetComponent<RectTransform>().anchoredPosition = pos;
    }
}