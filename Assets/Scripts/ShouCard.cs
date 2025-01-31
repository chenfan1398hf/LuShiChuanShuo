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

        info = _info;
        ShowCrad();
    }
    private void ShowCrad()
    {
        gjText.text = info.gjNumberNow.ToString();
        hpText.text = info.hpNumberNow.ToString();
        xjText.text = info.xjNumber.ToString();
        GameManager.instance.SpritPropImageByPath("KaPai/"+ info.imageId, cardImage);

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

    public void DragMethod()
    {
        GameManager.instance.DrageCardSetFatherOut(this.gameObject);
        transform.position = Input.mousePosition;
    }
    public void EndDragMethod()
    {
        if (this.transform.position.y >= 300f)
        {
            //成功
            Debug.Log(this.transform.position.y);
        }
        else
        {
            //失败
            GameManager.instance.DrageCardSetFatherIn(this.gameObject);
        }
    }
}
