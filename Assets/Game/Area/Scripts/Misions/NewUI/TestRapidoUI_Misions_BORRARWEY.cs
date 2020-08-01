using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRapidoUI_Misions_BORRARWEY : MonoBehaviour
{
    public List<Mision> misionescurrent;
    public List<Mision> misionesfinished;
    public UI_Mission_GeneralManager generalmanagerM;

    private void Start()
    {
        generalmanagerM.RefreshCurrentMissions(misionescurrent);
        generalmanagerM.RefreshFinishedMissions(misionesfinished);
    }
}
