using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossScript : LivingEntity
{
    private NavMeshAgent navMeshAgent;
    private Transform playerTransform;
    public LayerMask blockLOSLayers;
    public float LOSDistance = 10f;

    int phase = 0;
    public GameObject missile;
    public Transform spawnPoint;
    public float laserDamage = 1f;
    public float collideDamage = 1f;
    public float dashSpeed = 5f;

    public Transform currentHidingSpot;
    private List<Transform> goodHidingSpots;
    public Transform[] hidingSpots;
    public float minHideDistance = 5f;
    private Rigidbody rb;
    private LineRenderer lineRenderer;
    public enum BossState{
        Attack, Chase, Flee, Dash, Laser, Dead
    }

    private BossState currentState = BossState.Flee;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        goodHidingSpots = new List<Transform>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start(){
        GameUIScript.instance.BossSpawned(this);
        playerTransform = PlayerTDController.instance.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(currentState);
        switch(currentState){
            case(BossState.Flee):
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(currentHidingSpot.position);
                break;
            case(BossState.Chase):
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(playerTransform.position);
                break;
            case(BossState.Attack):
                navMeshAgent.isStopped = true;
                transform.LookAt(playerTransform);
                break;
            case(BossState.Dash):
                navMeshAgent.isStopped = true;
                //RaycastHit hit = new RaycastHit();
                rb.velocity = transform.forward * dashSpeed;
                /*
                RaycastHit[] hits = Physics.BoxCastAll(transform.position, transform.localScale, transform.forward, transform.rotation, blockDashLayers);
                if(hits.Length > 0){
                    Debug.Log("Door Canceled Dash");
                    CancelDash();
                    foreach(RaycastHit hit in hits){
                        if(hit.collider.tag == "Player"){
                            PlayerTDController.instance.TakeDamage(dashDamage);
                            break;
                        }
                    }
                }
                */
                break;
            case(BossState.Laser):
                navMeshAgent.isStopped = true;
                transform.LookAt(playerTransform);
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 100, blockLOSLayers))
                {
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, hit.point);
                    if(hit.collider.tag == "Player"){
                        PlayerTDController.instance.TakeDamage(laserDamage);
                    }
                }
                else{
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, transform.position + direction * 20f);
                }
                break;
        }
        
        //navMeshAgent.destination = navMeshTarget.position;
    }

    public void SpawnMissile(){
        currentState = BossState.Attack;
        Destroy(Instantiate(missile, spawnPoint.position, Quaternion.Euler(-90, 0, 0)), 20f);
    }

    public void StartLaserCharge(){
        Debug.Log("Start Laser Charge!");
        currentState = BossState.Attack;
    }

    public void FireLaser(){
        lineRenderer.enabled = true;
        currentState = BossState.Laser;
    }

    public void CancelLaser(){
        lineRenderer.enabled = false;
        currentState = BossState.Chase;
        navMeshAgent.isStopped = false;
        rb.velocity = Vector3.zero;
    }

    public void StartDashCharge(){
        Debug.Log("Start Dash Charge!");
        currentState = BossState.Attack;
    }

    public void StartDash(){
        Debug.Log("Dashing Begun");
        transform.LookAt(playerTransform);
        currentState = BossState.Dash;
    }

    public void CancelDash(){
        currentState = BossState.Chase;
        navMeshAgent.isStopped = false;
        rb.velocity = Vector3.zero;
    }



    public void StartFlee(){
        float squareMinDistance = minHideDistance * minHideDistance;
        goodHidingSpots.Clear();
        foreach(Transform hidingSpot in hidingSpots){
            Vector3 squareDistance = hidingSpot.position - transform.position;
            if(squareDistance.sqrMagnitude > squareMinDistance){
                if(currentHidingSpot == null || currentHidingSpot.position != hidingSpot.position){
                    goodHidingSpots.Add(hidingSpot);
                }
            }
        }
        if(goodHidingSpots.Count > 0){
            currentHidingSpot = goodHidingSpots[Random.Range(0, goodHidingSpots.Count)];
            currentState = BossState.Flee;
        }
        else{
            Debug.Log("Failed to find hiding spot!");
        }
    }

    public void StartChasePlayer(){
        currentState = BossState.Chase;
    }

    public void SetNavDestination(Vector3 destination){
        navMeshAgent.SetDestination(destination);
    }

    public bool CheckSeePlayer(){
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, LOSDistance, blockLOSLayers))
        {
            if(hit.collider.tag == "Player"){
                Debug.Log("See Player");
                return true;
            }
            
        }
        return false;
    }

    public float GetRemainingDistance(){
        
        return navMeshAgent.remainingDistance;
    }

    public BossState GetCurrentState(){
        return currentState;
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            PlayerTDController.instance.TakeDamage(collideDamage);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if(health > 0){
            AudioManagerScript.instance.PlaySound("Enemy Hurt", transform.position);
        }
    }

    public override void Die()
    {
        AudioManagerScript.instance.PlaySound2D("Boss Death");
        base.Die();
    }
}
