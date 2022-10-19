using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManagerScript : MonoBehaviour
{
    public static PlayerInputManagerScript instance;
    public event System.Action<GameInputs> OnUpdateInputs;
    GameInputs currentInputs;

    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool shoot;
    public bool interact;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    //public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    void Awake()
    {
        instance = this;
        currentInputs = new GameInputs();
    }

    void FixedUpdate(){
        if(OnUpdateInputs != null){
            OnUpdateInputs(currentInputs);
        }
        currentInputs.jump = false;
        currentInputs.shoot = false;
        currentInputs.interact = false;
    }
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        if(cursorInputForLook)
        {
            LookInput(value.Get<Vector2>());
        }
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }

    public void OnShoot(InputValue value){
        ShootInput(value.isPressed);
    }

    public void OnInteract(InputValue value){
        InteractInput(value.isPressed);
    }


    public void MoveInput(Vector2 newMoveDirection)
    {
        currentInputs.move = newMoveDirection;
    } 

    public void LookInput(Vector2 newLookDirection)
    {
        currentInputs.look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        currentInputs.jump = newJumpState;
    }

    public void SprintInput(bool newSprintState)
    {
        currentInputs.sprint = newSprintState;
    }

    public void ShootInput(bool newShootState){
        currentInputs.shoot = newShootState;
    }

    public void InteractInput(bool newInteractState){
        currentInputs.interact = newInteractState;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        //SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}

public struct GameInputs{
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool shoot;
    public bool interact;
}