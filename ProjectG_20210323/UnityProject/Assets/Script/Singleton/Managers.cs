using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static bool isApplicationsQuitting = false;
    private static Managers _instance;
    public static Managers instance
    {
        get
        {
            if (isApplicationsQuitting)
                return null;

            Init();

            return _instance;
        }
    }

    private DataManager _data = new DataManager();
    private ResourceManager _resource = new ResourceManager();
    private InputManager _input = new InputManager();
    private GameManager _game = new GameManager();
    private ItemManager _item = new ItemManager();

    public static DataManager Data { get { return instance._data; } }
    public static ResourceManager Resource { get { return instance._resource; } }
    public static InputManager Input { get { return instance._input; } }
    public static GameManager Game { get { return instance._game; } }
    public static ItemManager Item { get { return instance._item; } }

    void Start()
    {
        Init();
    }

    [RuntimeInitializeOnLoadMethod()]
    static void RunOnStart()
    {
        Application.quitting += () => isApplicationsQuitting = true;
    }

    void Update()
    {
        _input.OnUpdate();
    }

    private static void Init()
    {
        
        if (_instance == null)
        {
            GameObject obj = GameObject.Find("Managers");
            if(obj ==null)
            {
                obj = new GameObject { name = "Managers" };
                obj.AddComponent<Managers>();
            }
            _instance = obj.GetComponent<Managers>();

            _instance._item.Init();

            DontDestroyOnLoad(obj);
        }
    }
}
