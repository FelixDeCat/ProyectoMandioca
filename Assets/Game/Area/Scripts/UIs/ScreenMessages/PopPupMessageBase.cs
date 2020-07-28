
using UnityEngine;
using UnityEngine.UI;

public class PopPupMessageBase: MonoBehaviour
{
    public Text txt_message;
    public Image Img_photo;

    public void SetInfo(string _name)
    {
        txt_message.text = _name;
    }
    public void SetInfo(string _name, Sprite _photo)
    {
        txt_message.text = _name;
        Img_photo.sprite = _photo;
    }
}
