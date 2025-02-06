using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShouCard : MonoBehaviour
{
    private CardInfo info = new CardInfo();
    private Text gjText;
    private Text hpText;
    private Image cardImage;
    private Text xjText;
    private GameObject huiObj;
    private Image bg;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitCardInfo(CardInfo _info)
    {
        gjText = this.transform.Find("Image_gj/Text (Legacy)").GetComponent<Text>();
        hpText = this.transform.Find("Image_hp/Text (Legacy)").GetComponent<Text>();
        xjText = this.transform.Find("Image_xj/Text (Legacy)").GetComponent<Text>();
        cardImage = this.transform.Find("Image_pai").GetComponent<Image>();
        huiObj = this.transform.Find("ImageB").gameObject;
        bg = this.gameObject.GetComponent<Image>();
        info = _info;
        ShowCrad();
    }
    private void ShowCrad()
    {
        gjText.text = info.gjNumberNow.ToString();
        hpText.text = info.hpNumberNow.ToString();
        xjText.text = info.xjNumber.ToString();
        GameManager.instance.SpritPropImageByPath("KaPai/"+ info.imageId, cardImage);
        UpdateCardBack();

    }
    public void UpdateCardBack()
    {
        //如果是BOSS手牌则置灰
        if (info.state == 4)
        {
            huiObj.SetActive(true);
        }
        else
        {
            huiObj.SetActive(false);
        }
    }
    //玩家拖动牌
    public void DragMethod()
    {
        //玩家手牌拖动出牌
        if (info.state == 3 && GameManager.instance.GetPlayerOperand())
        {
            GameManager.instance.DrageCardSetFatherOut(this.gameObject);
            transform.position = Input.mousePosition;
        }
        
    }
    //结束拖动
    public void EndDragMethod()
    {
        if (info.state == 3)
        {
            if (this.transform.position.y >= 400f)
            {
                //成功
                bool isBool = GameManager.instance.ChuCard(this.gameObject, info);
                if (isBool == false)
                {
                    //失败
                    GameManager.instance.DrageCardSetFatherIn(this.gameObject);
                }
            }
            else
            {
                //失败
                GameManager.instance.DrageCardSetFatherIn(this.gameObject);
            }
        }
       
    }
    //玩家点击牌
    public void OnClickCard()
    { 

    }
    //获取牌数据
    public CardInfo GetCardInfo()
    {
        return info;
    }
    //可攻击变色
    public void ShowAttack()
    {
        if (info.attackNumber > 0)
        {
            bg.color = Color.green;
        }
        else
        {
            bg.color = Color.white;
        }
    }
    //卡牌攻击
    public void CardAttack()
    {
        GameManager.instance.Attack(this.gameObject);
    }
    //增加血量
    public void addHpNumber(int _number)
    {
        info.hpNumberNow += _number;
        ShowCrad();
    }
    //扣除行动次数
    public void addAttackNumber(int _number)
    {
        info.attackNumber += _number;
        ShowAttack();
    }
    public void CheckHp()
    {
        if (info.hpNumberNow <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
