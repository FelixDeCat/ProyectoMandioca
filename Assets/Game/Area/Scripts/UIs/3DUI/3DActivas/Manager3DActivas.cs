using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager3DActivas : MonoBehaviour
{

    [SerializeField] GameObject blockedModel = null;
    [SerializeField] GameObject emptyModel = null;

    public UI3D_Element_SkillsActivas[] ui_actives;
    

    //luego implementar un dictionary<key, queue<gameobject>> onda pool con los modelos ya instanciados
    public void ChangeModel(int i, GameObject model)
    {
        ui_actives[i].SetModel(Instantiate(model));
    }
    public void InitializeAllBlocked()
    {
        //foreach (var skillUI in sides)
        //{
        //    skillUI.RemoveSkillInfo();
        //}
    }

    public void RefreshCooldownAuxiliar(int _index, float _time) => ui_actives[_index].SetCooldown(_time);
    public void CooldownEndReadyAuxiliar(int _index) => ui_actives[_index].SkillLoaded();

    public void Execute()
    {
        var basevenetdata = new UnityEngine.EventSystems.BaseEventData(Main.instance.GetMyEventSystem().GetMyEventSystem());
        ui_actives[0].OnSubmit(basevenetdata);

    }

    public void Refresh(int index) { }
    public void Switch(int index)
    {
        GameObject model0 = ui_actives[0].GetModel();
        GameObject model1 = ui_actives[1].GetModel();
        if (index == 0)
        {
            ui_actives[0].SetModel(model0);
            ui_actives[1].SetModel(model1);
        }
        else
        {
            ui_actives[0].SetModel(model1);
            ui_actives[1].SetModel(model0);
        }
    }

    public void ReAssignUIInfo(SkillActivas[] col)
    {
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i] != null)
            {
                ui_actives[i].SetSkillInfo(col[i].skillinfo);
                ui_actives[i].Ocupy();
                ChangeModel(i, (col[i].skillinfo.model));
                col[i].SetUI(ui_actives[i]);
            }
            else
            {
                ui_actives[i].RemoveSkillInfo();
                ui_actives[i].Vacate();
                SetEmpty(i);
            }
        }
    }
    public void RefreshButtons(bool[] actives)
    {
        //for (int i = 0; i < actives.Length; i++)
        //{
        //    if (actives[i])
        //    {
        //        sides[i].SetIsUsable();

        //        //esta disponible
        //        if (!sides[i].IsOcupied())
        //        {
        //            SetEmpty(i);
        //        }
        //        else
        //        {
        //            sides[i].SetUnlocked();
        //        }
        //    }
        //    else
        //    {
        //        //Debug.Log("INDEX: " + i +" BLOQUEADO");
        //        //esta bloqueado
        //        sides[i].Vacate();
        //        sides[i].SetBlocked();
        //        sides[i].SetIsNotUsable();
        //        DeSelect(i);
        //        SetBlock(i);
        //    }
        //}
    }

    public void SetBlock(int i) => ui_actives[i].SetModel(Instantiate(blockedModel));
    public void SetEmpty(int i) => ui_actives[i].SetModel(Instantiate(emptyModel));
    //public void Select(int i)
    //{
    //    var basevenetdata = new UnityEngine.EventSystems.BaseEventData(Main.instance.GetMyEventSystem().GetMyEventSystem());
    //    foreach (var e in sides) e.OnDeselect(basevenetdata);
    //    sides[i].OnSelect(basevenetdata);
    //}
    //public void DeSelect(int i)
    //{
    //    var basevenetdata = new UnityEngine.EventSystems.BaseEventData(Main.instance.GetMyEventSystem().GetMyEventSystem());
    //    sides[i].OnDeselect(basevenetdata);
    //}
}
