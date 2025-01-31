using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using Spine.Unity;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager :MonoSingleton<GameManager>
{
    #region 构造函数及其变量
    public GameManager()
    {
        configMag = new ConfigManager();
    }
    public static bool isDbugLog = true;
    public PlayerData playerData = null;                            //玩家数据（本地持久化）
    public ConfigManager configMag;
    private System.Random Random;                                   //随机种子
    private int TimeNumber = 0;
    private List<UnityAction> unityActionList = new List<UnityAction>();
    public bool isBattle = true;


    public static int TI_LI_MAX_NUMBER = 100;
    public static int TI_LI_CD_NUMBER = 600;

    #endregion

    private void Update()
    {
        foreach (var item in unityActionList)
        {
            item.Invoke();
        }
    }
    #region Awake()
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;//设置帧率为60帧
        GetLocalPlayerData();
        Random = new System.Random(Guid.NewGuid().GetHashCode());
    }
    #endregion



    private void Start()
    {
        InitGame();
        this.InvokeRepeating("CheckTime", 0, 0.1f);
    }

    void CheckTime()
    {
        TimeNumber++;

        if (TimeNumber % 10 == 0)
        {
       
        }
        if (TimeNumber % 20 == 0)
        {

        }


    }


    #region OnApplicationPause(bool pause)切屏感知
    public void OnApplicationPause(bool pause)
    {
        if (pause == true)
        {
            if (isDbugLog)
                Debug.Log("切屏感知");
            SaveGame();
        }
    }
    #endregion

    #region OnApplicationQuit() 退出游戏感知
    public void OnApplicationQuit()
    {
        if (isDbugLog)
            Debug.Log("退出感知");
        SaveGame();

    }
    #endregion

    #region 获取本地数据
    public void GetLocalPlayerData()
    {
        playerData = PlayerData.GetLocalData();//读取本地持久化玩家数据(包括本土化设置)
        configMag.InitGameCfg();//读取配置表
        playerData.InitData();//根据配置表和本地数据初始化游戏数据
    }
    #endregion

    #region SaveGame() 保存玩家数据
    public void SaveGame()
    {
        //if(SocketManager.instance.socket!=null)
        //SocketManager.instance.socket.Disconnect();
        playerData.Save();
    }
    #endregion

    #region OnDestroy()
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    #endregion

    /// <summary>
    /// 注册一个update在这里跑
    /// </summary>
    /// <param name="_action"></param>
    public void AddUpdateListener(UnityAction _action)
    {
        unityActionList.Add(_action);
    }

    /// <summary>
    /// 加载图片
    /// </summary>
    public void SpritPropImage(string id, Image image)
    {
        string path = "Icon/" + id;
        Sprite Tab3Img = ResourcesLoad.instance.Load<Sprite>(path);
        image.sprite = Tab3Img;
    }

    /// <summary>
    /// 加载图片--装备图标
    /// </summary>
    public void SpritPropEquipIcon(string id, Image image)
    {
        string path = "EquipIcon/" + id;
        Sprite Tab3Img = ResourcesLoad.instance.Load<Sprite>(path);
        image.sprite = Tab3Img;
    }


    /// <summary>
    /// 加载图片
    /// </summary>
    public void SpritPropImageByPath(string path, Image image)
    {
        Sprite Tab3Img = Resources.Load(path, typeof(Sprite)) as Sprite;
        image.sprite = Tab3Img;
    }

    /// <summary>
    /// 加载图片
    /// </summary>
    public void SpritPropImageByPath(string path, SpriteRenderer image)
    {
        Sprite Tab3Img = Resources.Load(path, typeof(Sprite)) as Sprite;
        image.sprite = Tab3Img;
    }

    /// <summary>
    /// 添加预制体
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fatherTransform"></param>
    /// <returns></returns>
    public GameObject AddPrefab(string name, Transform fatherTransform)
    {
        string newpath = "Prefab/" + name;
        GameObject obj = ObjPool.instance.GetObj(newpath, fatherTransform);
        obj.AddComponent<DesObj>();
        obj.GetComponent<DesObj>().InitDes(newpath);
        return obj;
    }
    /// <summary>
    /// 销毁预制体
    /// </summary>
    /// <returns></returns>
    public void DestroyPrefab(string name, GameObject gameObject)
    {
        string[] list = name.Split(new char[] { '(' });
        if (list.Length != 2)
        {
            string newpath = "Prefab/" + name;
            ObjPool.instance.Recycle(newpath, gameObject);
        }
        else
        {
            string newpath = "Prefab/" + list[0];
            ObjPool.instance.Recycle(newpath, gameObject);
        }
        return;
    }
    /// <summary>
    /// 销毁预制体
    /// </summary>
    /// <returns></returns>
    public void DestroyPrefab(GameObject prefabObj, GameObject gameObject, string _path = null)
    {
        ObjPool.instance.Recycle(prefabObj, gameObject, "Prefab/" + _path);
        return;
    }
    /// <summary>
    /// 销毁预制体
    /// </summary>
    /// <returns></returns>
    public void DestroyPrefab(GameObject gameObject)
    {
        string name = gameObject.GetComponent<DesObj>().name;
        ObjPool.instance.Recycle(name, gameObject);
        return;
    }
    /// <summary>
    /// 播放动画并重置动画到第0帧
    /// </summary>
    public void PlaySpine(SkeletonGraphic _skeletonGraphic, bool isLoop, string _spineName, bool isRest)
    {
        if (isRest)
        {
            _skeletonGraphic.AnimationState.ClearTracks();
            _skeletonGraphic.AnimationState.Update(0);
        }
        _skeletonGraphic.AnimationState.SetAnimation(0, _spineName, isLoop);

        return;
    }
    /// <summary>
    /// 播放动画并重置动画到第0帧
    /// </summary>
    public void PlaySpine(Animator _animator, string _spineName, bool isRest)
    {
        //_animator.Play(_spineName, 0 ,0f);
        if (isRest)
        {
            //_animator.Update(0);
            _animator.Play(_spineName, 0, 0f);
        }
        else
        {
            _animator.Play(_spineName);
        }
        return;
    }
    /// <summary>
    /// 获取对象池内对象数据
    /// </summary>
    /// <returns></returns>
    public ObjPool.PoolItem GetPoolItem(string name)
    {
        string newpath = "Prefab/" + name;
        return ObjPool.instance.GetPoolItem(newpath); ;
    }
    /// <summary>
    /// 网络拉取图片
    /// </summary>
    /// <param name="_url"></param>
    /// <param name="_image"></param>
    /// <returns></returns>
    public IEnumerator GetHead(string _url, Image _image)
    {
        if (_url == string.Empty || _url == "")
        {
            _url = "https://p11.douyinpic.com/aweme/100x100/aweme-avatar/mosaic-legacy_3797_2889309425.jpeg?from=3067671334";
        }

        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(_url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(1f, 1f));
                _image.sprite = sprite;
                //Renderer renderer = plane.GetComponent<Renderer>();
                //renderer.material.mainTexture = texture;
            }
        }
    }

    /// <summary>
    /// 清空数据
    /// </summary>
    public void CleraPlayerData()
    {
        PlayerData.ClearLocalData();
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Editor/Tools/Clear")]
    static void CleraPlayerData1()
    {
        PlayerData.ClearLocalData();
    }
#endif
    private GameObject[] GetDontDestroyOnLoadGameObjects()
    {
        var allGameObjects = new List<GameObject>();
        allGameObjects.AddRange(FindObjectsOfType<GameObject>());
        //移除所有场景包含的对象
        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            var objs = scene.GetRootGameObjects();
            for (var j = 0; j < objs.Length; j++)
            {
                allGameObjects.Remove(objs[j]);
            }
        }
        //移除父级不为null的对象
        int k = allGameObjects.Count;
        while (--k >= 0)
        {
            if (allGameObjects[k].transform.parent != null)
            {
                allGameObjects.RemoveAt(k);
            }
        }
        return allGameObjects.ToArray();
    }

    private List<int> paiku = new List<int> {1,1,2,2,3,3,4,4,5,5,6,6,7,7,8,8,9,9,10,10,11,11,12,12,13,13,14,14,15,15 };
    private List<CardInfo> bossCardList = new List<CardInfo>();             //boss牌组
    private List<CardInfo> playerCardList = new List<CardInfo>();           //玩家牌组

    private List<GameObject> bossShouCardList = new List<GameObject>();         //boss手牌
    private List<GameObject> playerShouCardList = new List<GameObject>();       //玩家手牌

    private GameObject content1;                //玩家手牌
    private GameObject content2;                //玩家场牌
    private GameObject content3;                //BOSS手牌
    public GameObject beginPanel;
    public void InitGame()
    {
        content1 = GameObject.Find("Canvas/Panel/List1/Viewport/Content").gameObject;
        content2 = GameObject.Find("Canvas/Panel/List2/Viewport/Content").gameObject;
        content3 = GameObject.Find("Canvas/Panel/List3/Viewport/Content").gameObject;
    }
    //结束游戏
    public void EndGame()
    {
        bossCardList.Clear();
        playerCardList.Clear();
        foreach (var item in bossShouCardList)
        {
            Destroy(item);
        }
        foreach (var item in playerShouCardList)
        {
            Destroy(item);
        }
        bossShouCardList.Clear();
        playerShouCardList.Clear();
    }
    //开始游戏
    public void BeginGame()
    {
        beginPanel.SetActive(false);
        InitCard();
    }
    //初始化牌组
    public void InitCard()
    {
        //读取牌组配置
        for (int i = 0; i < paiku.Count; i++)
        {
            CardInfoCfg cfg = configMag.GetCardInfoCfgByKey(paiku[i]);
            CardInfo cardInfo = new CardInfo();
            cardInfo.addId = i;
            cardInfo.id = cfg.ID;
            cardInfo.xjNumber = cfg.xjNumber;
            cardInfo.hpNumber = cfg.hpNumber;
            cardInfo.gjNumber = cfg.gjNumber;
            cardInfo.hpNumberNow = cfg.hpNumber;
            cardInfo.gjNumberNow = cfg.gjNumber;
            cardInfo.name = cfg.name;
            cardInfo.type = cfg.type;
            cardInfo.imageId = cfg.imageId;
            cardInfo.state = 2;
            bossCardList.Add(cardInfo);

            CardInfo cardInfo2 = new CardInfo();
            cardInfo2.addId = 100+i;
            cardInfo2.id = cfg.ID;
            cardInfo2.xjNumber = cfg.xjNumber;
            cardInfo2.hpNumber = cfg.hpNumber;
            cardInfo2.gjNumber = cfg.gjNumber;
            cardInfo2.hpNumberNow = cfg.hpNumber;
            cardInfo2.gjNumberNow = cfg.gjNumber;
            cardInfo2.name = cfg.name;
            cardInfo2.type = cfg.type;
            cardInfo2.imageId = cfg.imageId;
            cardInfo2.state = 1;
            playerCardList.Add(cardInfo2);
        }
        //打乱排序
        Util.shuffle<CardInfo>(bossCardList);
        Util.shuffle<CardInfo>(playerCardList);

        //随机牌组的心境类型
        RandXjType();
    }
    //随机牌组心境类型
    public void RandXjType()
    {
        //确定卡牌心境类型
        List<int> bossRand = new List<int>();
        List<int> playerRand = new List<int>();
        if (playerData.playerLevel == 1)
        {
            for (int i = 0; i < 8; i++)
            {
                bossRand.Add(1);
                playerRand.Add(1);
            }
        }
        else if (playerData.playerLevel == 2)
        {
            for (int i = 0; i < 8; i++)
            {
                bossRand.Add(2);
            }
            playerRand.Add(1);
            playerRand.Add(1);
            playerRand.Add(1);
            playerRand.Add(1);
            playerRand.Add(2);
            playerRand.Add(2);
            playerRand.Add(2);
            playerRand.Add(2);
        }
        else if (playerData.playerLevel == 3)
        {
            for (int i = 0; i < 8; i++)
            {
                bossRand.Add(3);
            }
            playerRand.Add(1);
            playerRand.Add(1);
            playerRand.Add(1);
            playerRand.Add(3);
            playerRand.Add(2);
            playerRand.Add(2);
            playerRand.Add(3);
            playerRand.Add(3);
        }
        else if (playerData.playerLevel == 4)
        {
            for (int i = 0; i < 8; i++)
            {
                bossRand.Add(4);
            }
            playerRand.Add(1);
            playerRand.Add(1);
            playerRand.Add(2);
            playerRand.Add(2);
            playerRand.Add(3);
            playerRand.Add(3);
            playerRand.Add(4);
            playerRand.Add(4);
        }
        else if (playerData.playerLevel == 5)
        {
            bossRand.Add(1);
            bossRand.Add(1);
            bossRand.Add(2);
            bossRand.Add(2);
            bossRand.Add(3);
            bossRand.Add(3);
            bossRand.Add(4);
            bossRand.Add(4);

            playerRand.Add(1);
            playerRand.Add(1);
            playerRand.Add(2);
            playerRand.Add(2);
            playerRand.Add(3);
            playerRand.Add(3);
            playerRand.Add(4);
            playerRand.Add(4);
        }

        //随机赋值牌组的心境类型
        for (int i = 0; i < 8; i++)
        {
            int randNumber = Util.randomInt(1, 29);
            bossCardList[randNumber].xjType = bossRand[i];
        }
        for (int i = 0; i < 8; i++)
        {
            int randNumber = Util.randomInt(1, 29);
            playerCardList[randNumber].xjType = playerRand[i];
        }
    }
    //添加玩家手牌
    public void AddPlayerShouCard(int _number)
    {
        FaCard(playerCardList, playerShouCardList, _number, content1.transform, 3);
    }
    //添加Boss手牌
    public void AddBossShouCard(int _number)
    {
        FaCard(bossCardList, bossShouCardList, _number, content3.transform, 4);
    }
    //发牌
    public void FaCard(List<CardInfo> _list1, List<GameObject> _list2, int _number, Transform _fatherTransform, int _cardState)
    {
        for (int i = 0; i < _number; i++)
        {
            if (_list1.Count <= 0)
            {
                return;
            }
            CardInfo info = _list1.First();
            info.state = _cardState;
            _list1.RemoveAt(0);
            var obj = AddPrefab("ShouCard", _fatherTransform);
            obj.GetComponent<ShouCard>().InitCardInfo(info);
            _list2.Add(obj);
        }
    }
    //出牌
    public void ChuCard()
    {
        
    }
    //拖拽牌设置父节点
    public void DrageCardSetFatherOut(GameObject _obj)
    {
        _obj.transform.SetParent(GameObject.Find("Canvas/Panel").transform);
    }
    //拖拽牌设置父节点
    public void DrageCardSetFatherIn(GameObject _obj)
    {
        _obj.transform.parent = GameObject.Find("Canvas/Panel/List1/Viewport/Content").transform;
    }
}
