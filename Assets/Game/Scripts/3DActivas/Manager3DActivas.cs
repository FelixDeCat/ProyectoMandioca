using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager3DActivas : MonoBehaviour
{
    public UI3D_Element_SkillsActivas[] sides;

    public UI3D_CursorActiva cursor;

    [SerializeField] GameObject blockedModel = null;
    [SerializeField] GameObject emptyModel = null;

    //luego implementar un dictionary<key, queue<gameobject>> onda pool con los modelos ya instanciados
    public void ChangeModel(int i, GameObject model)
    {
        sides[i].SetModel(Instantiate(model));
    }
    public void InitializeAllBlocked()
    {
        for (int i = 0; i < 4; i++)
        {
            sides[i].SetModel(Instantiate(blockedModel));
        }
    }

    public void RefreshCooldownAuxiliar(int _index, float _time) => sides[_index].SetCooldow(_time);
    public void CooldownEndReadyAuxiliar(int _index) => sides[_index].SkillLoaded();

    public void ReAssignUIInfo(SkillActivas[] col)
    {
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i] != null)
            {
                sides[i].SetSkillInfo(col[i].skillinfo);
                sides[i].Ocupy();
                ChangeModel(i, (col[i].skillinfo.model));
                col[i].SetUI(sides[i]);
            }
            else
            {
                sides[i].RemoveSkillInfo();
                sides[i].Vacate();
                SetEmpty(i);
            }

        }
    }
    public void RefreshButtons(bool[] actives)
    {
        for (int i = 0; i < actives.Length; i++)
        {
            if (actives[i])
            {
                sides[i].SetIsUsable();

                //esta disponible
                if (!sides[i].IsOcupied())
                {
                    SetEmpty(i);
                }
                else
                {
                    sides[i].SetUnlocked();
                }
            }
            else
            {
                //Debug.Log("INDEX: " + i +" BLOQUEADO");
                //esta bloqueado
                sides[i].Vacate();
                sides[i].SetBlocked();
                sides[i].SetIsNotUsable();
                DeSelect(i);
                SetBlock(i);
            }
        }
    }

    public void SetBlock(int i) => sides[i].SetModel(Instantiate(blockedModel));
    public void SetEmpty(int i) => sides[i].SetModel(Instantiate(emptyModel));
    public void Select(int i)
    {
        var basevenetdata = new UnityEngine.EventSystems.BaseEventData(Main.instance.GetMyEventSystem().GetMyEventSystem());
        foreach (var e in sides) e.OnDeselect(basevenetdata);
        sides[i].OnSelect(basevenetdata);
        cursor.GoToPosition(sides[i].transform.position);
    }
    public void DeSelect(int i)
    {
        var basevenetdata = new UnityEngine.EventSystems.BaseEventData(Main.instance.GetMyEventSystem().GetMyEventSystem());
        sides[i].OnDeselect(basevenetdata);
    }
}
