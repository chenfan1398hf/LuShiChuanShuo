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
        //�����BOSS�������û�
        if (info.state == 4)
        {
            huiObj.SetActive(true);
        }
        else
        {
            huiObj.SetActive(false);
        }
    }
    //����϶���
    public void DragMethod()
    {
        //��������϶�����
        if (info.state == 3 && GameManager.instance.GetPlayerOperand())
        {
            GameManager.instance.DrageCardSetFatherOut(this.gameObject);
            transform.position = Input.mousePosition;
        }
        
    }
    //�����϶�
    public void EndDragMethod()
    {
        if (info.state == 3)
        {
            if (this.transform.position.y >= 400f)
            {
                //�ɹ�
                bool isBool = GameManager.instance.ChuCard(this.gameObject, info);
                if (isBool == false)
                {
                    //ʧ��
                    GameManager.instance.DrageCardSetFatherIn(this.gameObject);
                }
            }
            else
            {
                //ʧ��
                GameManager.instance.DrageCardSetFatherIn(this.gameObject);
            }
        }
       
    }
    //��ҵ����
    public void OnClickCard()
    { 

    }
    //��ȡ������
    public CardInfo GetCardInfo()
    {
        return info;
    }
    //�ɹ�����ɫ
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
    //���ƹ���
    public void CardAttack()
    {
        GameManager.instance.Attack(this.gameObject);
    }
    //����Ѫ��
    public void addHpNumber(int _number)
    {
        info.hpNumberNow += _number;
        ShowCrad();
    }
    //�۳��ж�����
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
