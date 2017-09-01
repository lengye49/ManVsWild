using System.Collections;
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
    private Vector3 restPoint;

    void Start(){
        restPoint = new Vector3(5000f, 0f, 0f);
        SDetail.localPosition = restPoint;
        NewMessage.localPosition = restPoint;
        Text[] ts = SDetail.GetComponentsInChildren<Text>();
        headText = ts[0];
        detailText = ts[1];
        noticeText = ts[2];
        studyText = ts[4];
    }

    /// <summary>
    /// 检查是否触发了意外事件。如果在室外，触发概率高一些。
    /// </summary>
    /// <returns><c>true</c>, if serendipity was checked, <c>false</c> otherwise.</returns>
    /// <param name="outdoor">If set to <c>true</c> outdoor.</param>
    public bool CheckSerendipity(bool outdoor){
        
        int r;
        if (outdoor)
            r = Random.Range(0, 9999);
        else
            r = Random.Range(0, 39999);

        //不触发意外事件
        if (r > 500)
            return false;

        if (r > 400)
            ShowSerendipity(0);
        else
            ShowSerendipity(1);
        
        return true;
    }

    /// <summary>
    /// 根据类型显示意外事件，0是叙事，1是广告
    /// </summary>
    /// <param name="sType">S type.</param>
    void ShowSerendipity(int sType){
        SDetail.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        SDetail.localPosition = Vector3.zero;
        SDetail.DOBlendableScaleBy(Vector3.one, 0.5f);

        Text[] ts = SDetail.GetComponentsInChildren<Text>();
        if (sType == 0)
        {
            headText.text = "破旧的纸片";
            detailText.text = "你发现了一张破旧的纸片，上面仿佛写着什么。";
            noticeText.text = "(手记，记录游戏里的背景信息。)";
        }
        else
        {
            headText.text = "留影石";
            detailText.text = "你发现了一个留影魔法石，不妨看看里面记录着什么，或许会有意想不到的收获。";
            noticeText.text = "(广告，观看完毕将获得一些灵魂石。)";
        }
        studyText.name = sType.ToString();
    }

    public void OnStudySerendipity(){
        
        OnCancelSerendipity();

        int sType = int.Parse(studyText.name);
        if (sType == 0)
        {
            NewMessage.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            NewMessage.localPosition = Vector3.zero;
            NewMessage.DOBlendableScaleBy(Vector3.one, 0.5f);

            Text t = NewMessage.GetComponentInChildren<Text>();
            t.text = "这就是游戏进度。";
        }
        else
        {
            Debug.Log("播放广告");
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
}
