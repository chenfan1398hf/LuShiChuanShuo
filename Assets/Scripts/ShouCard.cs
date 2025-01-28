using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShouCard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            //³É¹¦
            Debug.Log(this.transform.position.y);
        }
        else
        {
            //Ê§°Ü
            GameManager.instance.DrageCardSetFatherIn(this.gameObject);
        }
    }
}
