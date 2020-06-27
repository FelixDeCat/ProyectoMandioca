using UnityEngine;

public class Manager2DActivas : MonoBehaviour
{
    [SerializeField] Sprite emptyModel = null;

    public UI_Active[] ui_actives;
    
    public void ChangeModel(int i, Sprite model, float cd = 1)
    {
        ui_actives[i].SetSprite(model);
        ui_actives[i].SetCooldown(cd);
    }

    public void Execute(int index)
    {
        var basevenetdata = new UnityEngine.EventSystems.BaseEventData(Main.instance.GetMyEventSystem().GetMyEventSystem());
        ui_actives[index].OnSubmit(basevenetdata);

    }

    public void InitializeAllBlocked()
    {
        ui_actives[0].SetSprite(emptyModel);
        ui_actives[1].SetSprite(emptyModel);
    }

    public void RefreshCooldown(int index, float time)
    {
        ui_actives[index].SetCooldown(time);
    }

    public void CooldownEndReadyAuxiliar(int index)
    {
        ui_actives[index].FeedbackEndCooldown();
    }

    public void Refresh(SkillActivas[] col) 
    {
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i] != null)
            {
                ui_actives[i].SetCooldown(col[i].Cooldown);
                ui_actives[i].SetSprite(col[i].skillinfo.img_actived);
                ui_actives[i].SetColor(Color.white);
            }
            else
            {
                ui_actives[i].SetCooldown(1);
                ui_actives[i].SetSprite(emptyModel);
                ui_actives[i].SetColor(Color.black);
            }
        }
    }
}
