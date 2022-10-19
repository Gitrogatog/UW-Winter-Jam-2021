using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public float bulletDamage = 1f;
    public float moveSpeed;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        if(rb == null){
            rb = GetComponent<Rigidbody>();
        }
        rb.velocity = moveSpeed * transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            PlayerTDController.instance.TakeDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}
