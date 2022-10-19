using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueUIScript : MonoBehaviour
{
    public GameObject dialogueBox;
    public TMP_Text textLabel;

    public bool isOpen {get; private set;}
    //public DialogueObject testDialogue;
    TypewriterEffectScript typewriterEffect;
    private ResponseHandler responseHandler;
    bool interactPressed = false;
    // Start is called before the first frame update
    void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffectScript>();
        responseHandler = GetComponent<ResponseHandler>();
        CloseDialogueBox();
        //ShowDialogue(testDialogue);
        PlayerInputManagerScript.instance.OnUpdateInputs += InteractInput;
    }

    public void ShowDialogue(DialogueObject dialogueObject){
        OpenDialogueBox();
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents){
        responseHandler.AddResponseEvents(responseEvents);
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject){

        for(int i = 0; i < dialogueObject.Dialogue.Length; i++){
            string dialogue = dialogueObject.Dialogue[i];

            //yield return typewriterEffect.Run(dialogue, textLabel);
            yield return RunTypingEffect(dialogue);

            textLabel.text = dialogue;

            yield return null;

            if(i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses){
                break;
            }
            else{
                interactPressed = false;
                yield return new WaitUntil(InteractPressed);
                interactPressed = false;
            }
            
        }

        if(dialogueObject.HasResponses){
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else{
            CloseDialogueBox();
        }
    }

    private IEnumerator RunTypingEffect(string dialogueToType){
        typewriterEffect.Run(dialogueToType, textLabel);

        while(typewriterEffect.isRunning){
            yield return null;
            if(InteractPressed()){
                typewriterEffect.Stop();
            }
        }
    }
    public void CloseDialogueBox(){
        isOpen = false;
        PlayerTDController.instance.EndCutscene();
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }

    void OpenDialogueBox(){
        isOpen = true;
        PlayerTDController.instance.StartCutscene();
        dialogueBox.SetActive(true);
        textLabel.text = string.Empty;
    }

    public void InteractInput(GameInputs newInputs){
        interactPressed = newInputs.interact;
    }

    bool InteractPressed(){
        return interactPressed;
    }
}
