using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDialogueActivator : LivingEntity
{
    [SerializeField] private DialogueObject dialogueObject;
    public GameObject bossObject;
    private Collider theCollider;
    void Start()
    {
        theCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Die()
    {
        if(theCollider != null){
            theCollider.enabled = false;
        }
        PlayerTDController.instance.DialogueUI.ShowDialogue(dialogueObject);
    }

    public void SpawnBoss(){
        Instantiate(bossObject, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
