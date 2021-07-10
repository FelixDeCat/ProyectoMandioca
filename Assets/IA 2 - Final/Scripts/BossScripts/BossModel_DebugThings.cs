using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossModel_DebugThings : MonoBehaviour
{
    BossModel bossmodel;
    public static BossModel_DebugThings instance;
    void Awake() => instance = this;
    private void Start() => bossmodel = GetComponent<BossModel>();
    public static void InstaKill_Caronte() => instance.CaronteInstaKill();
    public void CaronteInstaKill() { bossmodel.DEBUG_InstaKill(); }
}
