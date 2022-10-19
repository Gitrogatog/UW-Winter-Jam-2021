using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class DashAttackNode : ActionNode
{
    public float chargeTime;
    private float dashAttackTime;
    public float dashTime;
    private float cancelDashTime;
    private BossScript bossScript;
    bool startedDashing = false;

    protected override void OnStart() {
        startedDashing = false;
        bossScript = context.gameObject.GetComponent<BossScript>();
        bossScript.StartDashCharge();
        dashAttackTime = Time.time + chargeTime;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(!startedDashing && dashAttackTime < Time.time){
            bossScript.StartDash();
            startedDashing = true;
            cancelDashTime = Time.time + dashTime;
        }
        else if(startedDashing && (bossScript.GetCurrentState() == BossScript.BossState.Chase || cancelDashTime < Time.time)){
            if(bossScript.GetCurrentState() == BossScript.BossState.Chase){
                Debug.Log("Door Canceled Dash");
            }
            else{
                Debug.Log("Node Canceled Dash");
                bossScript.CancelDash();
            }
            
            return State.Success;
        }
        return State.Running;
    }
}
