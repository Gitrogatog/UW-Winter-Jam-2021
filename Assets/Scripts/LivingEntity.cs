using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float health {get; protected set;}
    public float maxHealth = 3;
    public GameObject deathObject;
    protected bool isDead = false;
    public event System.Action OnDeath;
    private CinemachineImpulseSource damageImpulse;
    protected virtual void Awake(){
        health = maxHealth;
        damageImpulse = GetComponent<CinemachineImpulseSource>();
    }

    public virtual void TakeHit(float damage, Vector3 hitpoint, Vector3 hitDirection){
        TakeDamage(damage);
    }

    public virtual void TakeDamage(float damage){
        health -= damage;
        if(damageImpulse != null){
            damageImpulse.GenerateImpulse();
        }
        if(health <= 0 && !isDead){
            Die();
        }
        else{
            //CameraScript.instance.Shake(hurtShakeDuration, hurtShakeMagnitude);
        }
    }

    [ContextMenu("Self Destruct")]
    public virtual void Die(){
        //CameraScript.instance.Shake(deathShakeDuration, deathShakeMagnitude);
        
        isDead = true;
        if(deathObject != null){
            Destroy(Instantiate(deathObject, transform.position, Quaternion.identity), 1f);
        }
        if(OnDeath != null){
            OnDeath();
        }
        Destroy(gameObject);
    }

    protected virtual bool CheckOnDeathNull(){
        return OnDeath == null;
    }

    protected virtual void CallOnDeath(){
        if(OnDeath != null){
            OnDeath();
        }
        
    }
}
