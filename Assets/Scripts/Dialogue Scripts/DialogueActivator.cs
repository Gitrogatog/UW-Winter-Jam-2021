using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivator : MonoBehaviour, IInteractable
{
    //[SerializeField] private DialogueObject[] dialogues;
    [SerializeField] private DialogueObject dialogueObject;
    int currentDialogue = 0;

    public void UpdateDialogueObject(DialogueObject newDialogue){
        dialogueObject = newDialogue;
    }
    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            PlayerTDController.instance.interactable = this;
        }
    }

    void OnTriggerExit(Collider other){
        if(other.tag == "Player"){
            if(PlayerTDController.instance.interactable is DialogueActivator dialogueActivator && dialogueActivator == this){
                PlayerTDController.instance.interactable = null;
            }
        }
    }
    public void Interact(PlayerTDController player){
        

        foreach(DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>()){
            if(responseEvents.DialogueObject == dialogueObject){
                player.DialogueUI.AddResponseEvents(responseEvents.Events);
                break;
            }
        }

        /*
        if(dialogues.Length > 0){
            player.DialogueUI.ShowDialogue(dialogues[currentDialogue]);
        }
        currentDialogue++;
        currentDialogue = Mathf.Clamp(currentDialogue, 0, dialogues.Length - 1);
        */
        player.DialogueUI.ShowDialogue(dialogueObject);
    }
}
