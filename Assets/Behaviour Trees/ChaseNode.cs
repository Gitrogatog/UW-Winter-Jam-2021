using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ChaseNode : ActionNode
{
    float nextCheckTime;
    public float timeBetweenChecks;
    private Transform playerTransform;
    private BossScript bossScript;
    protected override void OnStart() {
        nextCheckTime = Time.time + timeBetweenChecks;
        bossScript = context.gameObject.GetComponent<BossScript>();
        bossScript.StartChasePlayer();
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(nextCheckTime < Time.time){
            if(bossScript.CheckSeePlayer()){
                Debug.Log("Chase Node End");
                return State.Success;
            }
            else{
                nextCheckTime = Time.time + timeBetweenChecks;
            }
        }
        return State.Running;
    }
}
