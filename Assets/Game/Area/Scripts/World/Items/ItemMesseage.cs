using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemMesseage : MonoBehaviour
{
    [SerializeField] UI_Anim_Code anim = null;
    [SerializeField] Image img = null;
    [SerializeField] TextMeshProUGUI txt = null;
    [SerializeField] float timeToReturn = 2;

    bool updating;
    float timer;

    public void OpenMesseage(Sprite sprite, string _txt)
    {
        anim.Open();
        timer = 0;
        updating = true;
        img.sprite = sprite;
        txt.text = _txt;
    }

    public void CloseMesseage()
    {
        anim.Close();
        updating = false;
    }

    private void Update()
    {
        if (!updating) return;

        timer += Time.deltaTime;

        if (timer >= timeToReturn)
            CloseMesseage();
    }
}
