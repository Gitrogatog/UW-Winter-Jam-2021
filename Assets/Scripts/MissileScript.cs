using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MissileScript : LivingEntity
{
    private NavMeshAgent navMeshAgent;
    public float moveSpeed;
    public float turnSpeed;
    Transform playerTransform;
    public float updateNavMeshTime = 0.25f;
    void Start()
    {
        playerTransform = PlayerTDController.instance.transform;
        //playerTransform = PlayerFPSMovement.instance.transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(ChasePlayer());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        Vector3 targetDirection = playerTransform.position - transform.position + new Vector3(0, 1.4f, 0);
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, turnSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
        rb.velocity = transform.forward * moveSpeed;
        */
    }

    private IEnumerator ChasePlayer(){
        WaitForSeconds Wait = new WaitForSeconds(updateNavMeshTime);
        while(true){
            navMeshAgent.SetDestination(playerTransform.position);
            yield return Wait;
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            //PlayerFPSMovement playerMovement = other.gameObject.GetComponent<PlayerFPSMovement>();
            PlayerTDController playerMovement = other.gameObject.GetComponent<PlayerTDController>();
            if(playerMovement != null){
                playerMovement.TakeDamage(0f);
            }
        }
        Destroy(gameObject);
    }
}
