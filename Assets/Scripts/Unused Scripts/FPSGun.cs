using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSGun : MonoBehaviour, IWeapon
{
    public float damage = 1;
    public float range = 100;
    private Transform shootPoint;

    private Camera fpsCam;
    public LayerMask stopShotLayers;

    MeshRenderer gunRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        gunRenderer = GetComponent<MeshRenderer>();
        fpsCam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(){
        Shoot();
    }

    void Shoot(){
        //Debug.Log("Gun Initiated");
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, stopShotLayers)){
            //Debug.Log(hit.transform.name);
            LivingEntity entityScript = hit.transform.GetComponent<LivingEntity>();
            if(entityScript != null){
                entityScript.TakeDamage(damage);
                //Debug.Log("Hit Enemy");
            }
            else{
                //Debug.Log("Not Enemy");
            }
        }
    }

    public void SetShootPoint(Transform newShootPoint){
        shootPoint = newShootPoint;
    }

    public void Reload(){

    }

    public void Aim(Vector3 aimPoint) {
		if (true) { //!isReloading
			transform.LookAt (aimPoint);
		}
	}

    public void Disable(){
        gunRenderer.enabled = false;
    }

    public void Enable(){
        gunRenderer.enabled = true;
    }
}
