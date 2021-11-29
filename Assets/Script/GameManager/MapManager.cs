using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    public GameObject blockPrefab;
    public RectTransform MapBoard;

    public Sprite past_location;
    public Sprite player_location;

    [SerializeField] private Map mapData;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject enemyTransform;
    private GameObject enemyObject;
    private Player player;
    private Point playerPos;
    [SerializeField] private GameObject playerUI;

    Node[,] Board;
    [HideInInspector] public int height;
    [HideInInspector] public int width;

    [HideInInspector] public bool active = false;
    bool isMove = false;

    enum Dir
    {
        up,
        down,
        right,
        left
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        Set();
    }

    void Start()
    {
        InitialzieBoard();

        Reset();
    }

    private void Reset()
    {
        player = playerObject.GetComponent<Player>();
        isMove = false;
        setPos(new Point(0, 0));
        active = MapBoard.gameObject.activeSelf;
    }

    public void Set()
    {
        //SetButton(GameObject.Find("Up"), () => Move(Dir.up));
        //SetButton(GameObject.Find("Left"), () => Move(Dir.left));
        SetButton(GameObject.Find("Down"), () => Move(Dir.down));
        SetButton(GameObject.Find("Right"), () => Move(Dir.right));
        SetButton(GameObject.Find("ShowMapButton"), MapButton);
        GameObject.Find("MapBoard").SetActive(false);
    }

    void InitialzieBoard()
    {
        width = 8;
        height = mapData.ArrayBlocks.Length;
        Board = new Node[width, height];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject n = Instantiate(blockPrefab, MapBoard);
                n.name = "[" + j + ", " + i + "]";
                Board[j, i] = new Node(mapData.ArrayBlocks[i].LineBlocks[j], new Point(j, i), n);
            }
        }
    }
    private void Update()
    {

    }

    public void MapReloading()
    {

    }

    void setPos(Point p)
    {
        playerPos = p;
        Vector2 pos = new Vector2(90.625f + (1050 * p.x / 8), -25 -
             (350 * p.y / 5));
        playerUI.GetComponent<RectTransform>().anchoredPosition = pos;
        Board[p.x, p.y].ob.GetComponent<Image>().sprite = player_location;
    }

    void Move(Dir dir)
    {
        if (isMove) return;
        isMove = true;

        Point p = playerPos;
        Board[p.x, p.y].ob.GetComponent<Image>().sprite = past_location;
        switch (dir)
        {
            case Dir.up:
                if (p.y == 0) { isMove = false; return; }
                p.y -= 1;
                break;
            case Dir.down:
                if (p.y == height - 1) { isMove = false; return; }
                p.y += 1;
                break;
            case Dir.left:
                if (p.x == 0) { isMove = false; return; }
                p.x -= 1;
                break;
            case Dir.right:
                if (p.x == width - 1) { isMove = false; return; }
                p.x += 1;
                break;
        }

        setPos(p);
        if (Board[p.x, p.y].getBlock().isBattle == true)
        {
            StopCoroutine(coroutine);
            StartCoroutine(UIManager.Instance.FadeInObject(UIManager.Instance.Fade, 0.5f, 0f));
            coroutine = StartCoroutine(UIManager.Instance.MoveUI(MapBoard.gameObject, new Vector2(0, -500), 3f, 0.3f));
            active = !active;

            enemyObject = GameObject.Instantiate(Board[p.x, p.y].getBlock().enemy, enemyTransform.transform);
            StartCoroutine(UIManager.Instance.FadeOutObject(enemyObject.GetComponentsInChildren<SpriteRenderer>(), 1.0f, 2f));
            Enemy enemy = enemyObject.GetComponent<Enemy>();

            SkillManager.Instance.SetEntity(enemy, player);
            StartCoroutine(Delay(0.5f, () => TurnManager.Instance.StartBattle(enemy, player)));
        }
        else
        {
            isMove = false;
        }
    }

    public void EndBattle()
    {
        for (int i = 0; i < playerUI.transform.childCount; i++)
            playerUI.transform.GetChild(i).gameObject.SetActive(true);
        StartCoroutine(UIManager.Instance.FadeOutObject(UIManager.Instance.Fade, 0.6f, 0f));
        coroutine = StartCoroutine(UIManager.Instance.MoveUI(MapBoard.gameObject, new Vector2(0, 300), 3f, 0.1f, false));
        active = !active;
        StartCoroutine(UIManager.Instance.FadeInObject(enemyObject.GetComponentsInChildren<SpriteRenderer>(), 1.0f, 0f));

        isMove = false;
    }

    IEnumerator Delay(float time, UnityEngine.Events.UnityAction act)
    {
        yield return new WaitForSeconds(time);
        act();
    }

    void SetButton(GameObject ob, UnityEngine.Events.UnityAction act)
    {
        Button BT = ob.GetComponent<Button>();
        BT.onClick.AddListener(act);
    }

    Coroutine coroutine;
    void MapButton()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);

        if (active)
        {
            StartCoroutine(UIManager.Instance.FadeInObject(UIManager.Instance.Fade, 0.5f, 0f));
            coroutine = StartCoroutine(UIManager.Instance.MoveUI(MapBoard.gameObject, new Vector2(0, -350), 3f, 0.3f));
            
        }
        else
        {
            StartCoroutine(UIManager.Instance.FadeOutObject(UIManager.Instance.Fade, 0.5f, 0f));
            coroutine = StartCoroutine(UIManager.Instance.MoveUI(MapBoard.gameObject, new Vector2(0, 350), 3f, 0.1f, false));
        }

        active = !active;
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
        Vector2 pos = new Vector2(90.625f + (1050 * index.x / 8), -55 -
             (350 * index.y / 5));
        ob.GetComponent<RectTransform>().anchoredPosition = pos;
    }
}