using DevelopTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMessageLogger : SingleObjectPool<UI_Comp_Message>
{
    public static UIMessageLogger instance;
    void Awake() => instance = this;

    [SerializeField] RectTransform parent = null;
    [SerializeField] UI_Comp_Message model = null;
    public void LogMessage(MsgLogData data)
    {
        var comp = Get();
        comp.SetInfo(data, EndAnim);
    }

    protected override void AddObject(int amount = 5)
    {
        for (int i = 0; i < amount; i++)
        {
            var comp = GameObject.Instantiate(model, parent);

            //aca le paso los actions

            comp.gameObject.SetActive(false);
            objects.Enqueue(comp);
        }


        base.AddObject(amount);
    }

    void EndAnim(UI_Comp_Message comp)
    {
        ReturnToPool(comp);
    }

    public void ClearPanel()
    {
        //acá mando a todos los componentes al return to pool
    }

    #region TEST

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    var msg1 = new MsgLogData("Test basico");
        //    var msg2 = new MsgLogData("Test dura 5 seg", 5f);
        //    var msg3 = new MsgLogData("Test bloqueado",true);
        //   var msg4 = new MsgLogData("Test dura 7 el negro, con img", InputImageDatabase.instance.GetSprite(InputImageDatabase.InputImageType.interact), Color.black, Color.white, 7f);

        //    LogMessage(msg1);
        //    LogMessage(msg2);
        //    LogMessage(msg3);
        //    LogMessage(msg4);
        //}
    }

    #endregion
}

public static class GameMessage
{
    public static void Log(MsgLogData data)
    {
        UIMessageLogger.instance.LogMessage(data);
    }
}
