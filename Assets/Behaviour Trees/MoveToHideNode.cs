using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MoveToHideNode : ActionNode
{
    private BossScript bossScript;
    protected override void OnStart() {
        Debug.Log("Start Move");
        bossScript = context.gameObject.GetComponent<BossScript>();
        bossScript.StartFlee();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(bossScript.GetRemainingDistance() < 3f){
            return State.Success;
        }
        return State.Running;
    }
}
