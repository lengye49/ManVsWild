using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class SerendipityActions : MonoBehaviour {

    public Transform SDetail;
    public Transform NewMessage;

    private Text headText;
    private Text detailText;
    private Text noticeText;
    private Text studyText;
    private Button studyButton;
    private Vector3 restPoint;

    private Dictionary<int,string> nameList;
    private Dictionary<int,string> messageList;
    private GameData _gameData;

    void Start(){
        Vungle.init("59b4965c7e75cb8114000385", "Test_iOS", "vungleTest");
        Vungle.onAdFinishedEvent += (args) =>{
            AdFinished(args);
        } ;

        _gameData = this.gameObject.GetComponent<GameData>();
        ResetNameList();
        ResetMessageList();
        restPoint = new Vector3(5000f, 0f, 0f);
        SDetail.localPosition = restPoint;
        NewMessage.localPosition = restPoint;
        studyButton = SDetail.GetComponentInChildren<Button>();
        Text[] ts = SDetail.GetComponentsInChildren<Text>();
        headText = ts[0];
        detailText = ts[1];
        noticeText = ts[2];
        studyText = ts[3];
    }

    void Update(){
//        Debug.Log(Vungle.isAdvertAvailable());
        studyButton.interactable = Vungle.isAdvertAvailable();
        studyText.text = Vungle.isAdvertAvailable() ? "观看" : "加载中";
    }


    /// <summary>
    /// 检查是否触发了意外事件。如果在室外，触发概率高一些。
    /// </summary>
    /// <returns><c>true</c>, if serendipity was checked, <c>false</c> otherwise.</returns>
    /// <param name="outdoor">If set to <c>true</c> outdoor.</param>
    public void CheckSerendipity(){
        
        if (GameData._playerData.minutesPassed < 1440)
            return;

        int r = Random.Range(0, 9999);

        //不触发意外事件
        if (r > 500)
            return;

        //0信息 1广告
        if (r > 400)
            ShowSerendipity(0);
        else
            ShowSerendipity(1);
        
    }
        
    /// <summary>
    /// 根据类型显示意外事件，0是叙事，1是广告
    /// </summary>
    /// <param name="sType">S type.</param>
    void ShowSerendipity(int sType){
        SDetail.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        SDetail.localPosition = Vector3.zero;
        SDetail.DOBlendableScaleBy(Vector3.one, 0.5f);

        if (sType == 0)
        {
            int index = Algorithms.GetIndexByRange(0, nameList.Count);
            headText.text = nameList[index];
            detailText.text = "你发现了" + nameList[index] + "，上面仿佛写着什么。";
            noticeText.text = "(手记，记录着失落之地的一些信息。)";
            studyButton.interactable = true;
        }
        else
        {
            headText.text = "留影石";
            detailText.text = "你发现了一个留影魔法石，不妨看看里面记录着什么，或许会有意想不到的收获。";
            noticeText.text = "(这是一条广告，观看完毕将获得一些灵魂石，下载相关内容将获得额外奖励。)";
            studyButton.interactable = Vungle.isAdvertAvailable();
        }
        studyText.name = sType.ToString();
    }


    /// <summary>
    /// 根据类型确定是显示信息还是播放广告
    /// </summary>
    public void OnStudySerendipity(){
        
        OnCancelSerendipity();

        int sType = int.Parse(studyText.name);
        if (sType == 0)
        {
            NewMessage.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            NewMessage.localPosition = Vector3.zero;
            NewMessage.DOBlendableScaleBy(Vector3.one, 0.5f);

            Text t = NewMessage.GetComponentInChildren<Text>();
            int index = Algorithms.GetIndexByRange(0, messageList.Count);
            t.text = messageList[index];
        }
        else
        {
            PlayRewardedAd();
        }

    }


    void PlayRewardedAd(){
        Debug.Log("播放广告");
        Vungle.playAd(true, "ThisUser");
    }

    /// <summary>
    /// 根据播放完成的事件来发送奖励，跳过无奖励，完成10灵魂石，下载20灵魂石。
    /// </summary>
    /// <param name="args">Arguments.</param>
    void AdFinished(AdFinishedEventArgs args){
        if (args.WasCallToActionClicked)
        {
            //点击了下载按钮，奖励20灵魂石
            _gameData.AddItem(22020000,20);
            GetComponentInChildren<LogManager>().AddLog("你获得了 灵魂石+20。");
        }
        else if (args.IsCompletedView)
        {
            //完成了播放，奖励10灵魂石
            _gameData.AddItem(22020000,10);
            GetComponentInChildren<LogManager>().AddLog("你获得了 灵魂石+10。");
        }
        else
        {
            //未完成播放，没有奖励
            _gameData.AddItem(22020000,1);
            GetComponentInChildren<LogManager>().AddLog("你获得了 灵魂石+1。");
        }

    }


    public void OnCancelSerendipity(){
        SDetail.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        SDetail.localPosition = restPoint;
    }

    public void OnCloseMessage(){
        NewMessage.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        NewMessage.localPosition = restPoint;
    }

    void ResetNameList(){
        nameList = new Dictionary<int, string>();
        nameList.Add(0, "破旧的纸片");
        nameList.Add(1, "一块破布");
        nameList.Add(2, "一个龟壳");
        nameList.Add(3, "残缺的日记");
        nameList.Add(4, "一页航海日志");
        nameList.Add(5, "刻字的盾牌"); 
    }

    void ResetMessageList(){
        messageList = new Dictionary<int, string>();
        messageList.Add(0,"...这片陆地原住民种族复杂，还有一些魔力强大的生灵...");
        messageList.Add(1,"...“他”从..大陆通过空间旋涡逃到此处，藏在...");
        messageList.Add(2,"...地牢中藏着强大的生灵，不要轻易招惹...");
        messageList.Add(3,"...计划在千日之后对“他”进行剿灭...“他”实力强大，需准备大量人手...");
        messageList.Add(4,"...搜寻“他”的...部分死于空间风暴...地图碎片散落各地...");
        messageList.Add(5,"...控制此地精神力薄弱的怪物对“他”进行监控和骚扰...幽灵...夜间...");
        messageList.Add(6,"...回不去了，除非...地牢...");
        messageList.Add(7,"...神秘的地牢埋藏着大量的空间气息和无尽的宝藏...");
        messageList.Add(8,"...地牢中深处有一个巨大的空间裂缝...");
        messageList.Add(9,"...派一些肮脏的盗贼潜入，或许能得到有用的消息...");
        messageList.Add(10,"...那些可恶的原住人类，总是根据好感度给“他”按时送上补给...");
        messageList.Add(11,"...杀光所有的人类，或许是一个好的选择...");
        messageList.Add(12,"...虽然春夏秋冬降雨量不同，但是小溪的水量却没有什么变化...");
        messageList.Add(13,"...鲤鱼除了美味，据说还有其他功效...");
        messageList.Add(14,"...木头可以盖房子，也可以制作武器...");
        messageList.Add(15,"...浆果一定要及早采摘，被野兽叼走可就浪费了...");
        messageList.Add(16,"...不管什么时候，矿产都是珍贵的资源...");
        messageList.Add(17,"...矿物挖起来非常耗费体力...");
        messageList.Add(18,"...小镇上的人经常外出，一定知道去其他地方的路。");
        messageList.Add(19,"...没有“力量”虽然不会危及生命，但总是有心里阴影...");
        messageList.Add(20,"...击败土匪或者有魔力的怪物，会增加镇上居民的好感度...");
        messageList.Add(21,"...狼人计划...密切监视那个外来者...");
        messageList.Add(22,"...蛇人和熊人的领地...那个外来者...");
        messageList.Add(23,"...狼人和蛇人...外来者也杀了他们不少...格杀勿论...");
        messageList.Add(24,"...本地居民有三个势力幕后统治...亡灵，矮人，精灵...人类的后裔...“他”的祖先...");
        messageList.Add(25,"...那帮倒霉的矮人...把自己埋在了矿坑中...");
        messageList.Add(26,"...亡灵的研究方向跟我族不同...永生...可恶的精灵...");
        messageList.Add(27,"...古堡中的势力已经被抹除，只留下几个苟延残喘的魔物...");
        messageList.Add(28,"...龙，真是强大而贪婪的种族...想办法控制住它们...");
        messageList.Add(29,"...镇长的实力太强，不宜正面攻击，骚扰抢劫其他居民...");
        messageList.Add(30,"...向镇长汇报盗匪和魔物的行踪...");
        messageList.Add(31,"...最近有一批神秘的生灵在到处活跃，跟夜间的幽灵往来密切...");
        messageList.Add(32,"...针对外来者的盗贼来势汹汹...小镇也无能为力，只能靠“他”自己了...");
        messageList.Add(33,"...盗贼和幽灵串通一气...");
    }

}
