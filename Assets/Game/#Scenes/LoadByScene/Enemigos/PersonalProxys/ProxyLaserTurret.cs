using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyLaserTurret : ProxyEnemyBase
{
    [SerializeField] TurretType     turrent_type = TurretType.Static;
    [SerializeField] LayerMask      contactMask = 1 << 0;
    [SerializeField] Damagetype     damageType = Damagetype.Fire;
    [SerializeField] int            damage = 3;
    [SerializeField] float          knockback = 10f;
    [SerializeField] float          rayDuration = 6f;
    [SerializeField] Transform      start = null;
    [SerializeField] Transform      end = null;
    [SerializeField] float          rotSpeed = 0.7f;
    [SerializeField] bool           InitializedTurret = false;

    public override void SpawnEnemy(EnemyBase enemy)
    {
        base.SpawnEnemy(enemy);

        var temp = enemy.GetComponent<TurretEnemy>();
        temp.Configure(
            turrent_type,
            contactMask,
            damageType,
            damage,
            knockback,
            rayDuration,
            start,
            end,
            rotSpeed,
            InitializedTurret);
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(this.transform.position, start.transform.position);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, end.transform.position);
    }
}