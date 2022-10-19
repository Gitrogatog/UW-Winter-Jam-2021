using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class WaitAtHideNode : ActionNode
{
    public float waitTime;
    float endWaitTime;
    private BossScript bossScript;
    private float startHealth;
    protected override void OnStart() {
        Debug.Log("Start Wait");
        bossScript = context.gameObject.GetComponent<BossScript>();
        startHealth = bossScript.health;
        endWaitTime = Time.time + waitTime;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(endWaitTime < Time.time || bossScript.health > startHealth){
            return State.Success;
        }
        return State.Running;
    }
}
