using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class SpawnRocketNode : ActionNode
{
    private BossScript bossScript;
    public float chargeShotTime = 1f;
    float nextShotTime;
    protected override void OnStart() {
        Debug.Log("Started Firing");
        nextShotTime = Time.time + chargeShotTime;
        bossScript = context.gameObject.GetComponent<BossScript>();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(nextShotTime < Time.time){
            bossScript.SpawnMissile();
            return State.Success;
        }
        return State.Running;
    }
}
