using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class ChargeLaserNode : ActionNode
{
    public float chargeTime;
    private float fireLaserTime;
    public float laserTime;
    private float endLaserTime;
    private bool firedLaser = false;
    private BossScript bossScript;

    protected override void OnStart() {
        firedLaser = false;
        bossScript = context.gameObject.GetComponent<BossScript>();
        bossScript.StartLaserCharge();
        fireLaserTime = Time.time + chargeTime;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(!firedLaser && fireLaserTime < Time.time){
            bossScript.FireLaser();
            firedLaser = true;
            endLaserTime = Time.time + laserTime;
        }
        else if(firedLaser && endLaserTime < Time.time){
            bossScript.CancelLaser();
            return State.Success;
        }
        return State.Running;
    }
}
