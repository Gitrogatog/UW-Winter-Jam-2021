using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLineOfSightChecker : MonoBehaviour
{
    [HideInInspector]
    public SphereCollider checkCollider;
    public float FieldOfView = 90f;
    public LayerMask LineOfSightLayers;

    public delegate void GainSightEvent(Transform Target);
    public GainSightEvent OnGainSight;
    public delegate void LoseSightEvent(Transform Target);
    public LoseSightEvent OnLoseSight;
    private bool canSeePlayer = false;

    private Coroutine CheckForLineOfSightCoroutine;

    private void Awake()
    {
        checkCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CheckLineOfSight(other.transform))
        {
            CheckForLineOfSightCoroutine = StartCoroutine(CheckForLineOfSight(other.transform));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canSeePlayer = false;
        OnLoseSight?.Invoke(other.transform);
        if (CheckForLineOfSightCoroutine != null)
        {
            StopCoroutine(CheckForLineOfSightCoroutine);
        }
    }

    private bool CheckLineOfSight(Transform Target)
    {
        Vector3 direction = (Target.transform.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, direction);
        if (true) //dotProduct >= Mathf.Cos(FieldOfView)
        {
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, checkCollider.radius, LineOfSightLayers))
            {
                if(hit.collider.tag == "Player"){
                    //Debug.Log(Target.gameObject.name);
                    canSeePlayer = true;
                    OnGainSight?.Invoke(Target);
                    return true;
                }
                
            }
        }
        canSeePlayer = false;
        return false;
    }

    private IEnumerator CheckForLineOfSight(Transform Target)
    {
        WaitForSeconds Wait = new WaitForSeconds(0.5f);

        while(!CheckLineOfSight(Target))
        {
            yield return Wait;
        }
    }

    void Update(){
        if(canSeePlayer){
            Debug.Log("See Player!");
        }
    }
}
