using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerTDController : LivingEntity
{
	public LayerMask testMask;
	public LineRenderer lineRenderer;
    public float moveSpeed = 5;

	public Crosshairs crosshairs;
	public float crosshairHeight;

	Camera viewCamera;
	
    CharacterController charController;
    WeaponsManager weaponsManager;
    Vector3 moveVelocity = Vector3.zero;
    //private PlayerTDInputs _input;
	private GameInputs _input;
	public static PlayerTDController instance;

	public float invincibilityTime;
	private float invincibilityEndTime = 0f;
	public float interactBreakTime = 1f;
	private float reenableInteractTime = 0f;

	//Dialogue stuff
	[SerializeField] private DialogueUIScript dialogueUI;
	public DialogueUIScript DialogueUI => dialogueUI;
	public IInteractable interactable {get; set;}

	public enum PlayerState{
		Idle, Dead, Cutscene
	}

	private PlayerState currentState = PlayerState.Idle;
	//public CinemachineImpulseSource damageImpulse;
	
	protected override void Awake () {
		instance = this;
		base.Awake ();
		weaponsManager = GetComponent<WeaponsManager>();
        charController = GetComponent<CharacterController>();
		viewCamera = Camera.main;
        //_input = GetComponent<PlayerTDInputs>();
		
		_input = new GameInputs();
	}

	void Start(){
		PlayerInputManagerScript.instance.OnUpdateInputs += UpdateInputs;
	}
	void Update () {
		
		// Movement input
        //float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
		if(currentState != PlayerState.Dead){

			//Get velocity input
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
			moveVelocity = inputDirection * moveSpeed;

			// Look input
			Ray ray = viewCamera.ScreenPointToRay (Mouse.current.position.ReadValue());
			//Debug.Log(Mouse.current.position.ReadValue());
			Plane groundPlane = new Plane (Vector3.up, Vector3.up * weaponsManager.GunHeight);
			float rayDistance;

			//Aiming
			if (groundPlane.Raycast(ray,out rayDistance)) {
				Vector3 point = ray.GetPoint(rayDistance);
				Debug.DrawLine(ray.origin,point,Color.red);

				//Aiming
				LookAt(point);
				crosshairs.transform.position = point;//new Vector3(point.x, crosshairHeight, point.z);
				crosshairs.DetectTargets(ray);
				if ((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude > 1) {
					weaponsManager.Aim(point);
					//gunController.Aim(point);
				}

				//Draw visible line
				Vector3 direction = (point - transform.position).normalized;
				if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 100, testMask))
				{
					lineRenderer.SetPosition(0, transform.position);
					lineRenderer.SetPosition(1, hit.point);
				}
				else{
					lineRenderer.SetPosition(0, transform.position);
					lineRenderer.SetPosition(1, transform.position + direction * 20f);
				}
			}
		}
	}

    private void FixedUpdate(){
		
		if(currentState == PlayerState.Idle){
			Interact();
			Move();
        	Shoot();
		}
		else{
			charController.Move(Vector3.zero);
		}
    }

	private void UpdateInputs(GameInputs newInputs){
		_input = newInputs;
		//Debug.Log("Input Received!");
	}

	private void Interact(){
		if(currentState == PlayerState.Idle && _input.interact && reenableInteractTime < Time.time){
			interactable?.Interact(this);
		}
	}

    private void Move(){
        charController.Move(moveVelocity * Time.fixedDeltaTime);
    }

    public void LookAt(Vector3 lookPoint) {
		Vector3 heightCorrectedPoint = new Vector3 (lookPoint.x, transform.position.y, lookPoint.z);
		transform.LookAt (heightCorrectedPoint);
	}

    private void Shoot(){
        if(_input.shoot){
            Debug.Log("PC Fired!");
            weaponsManager.ShootGun();
            _input.shoot = false;
        }
    }

	public void StartCutscene(){
		if(currentState != PlayerState.Dead){
			currentState = PlayerState.Cutscene;
		}
		
	}

	public void EndCutscene(){
		if(currentState != PlayerState.Dead){
			currentState = PlayerState.Idle;
			reenableInteractTime = Time.time + interactBreakTime;
		}
	}

	public override void TakeDamage(float damage){
		if(invincibilityEndTime < Time.time){
			base.TakeDamage(damage);
			if(health > 0){
				AudioManagerScript.instance.PlaySound("Player Hurt", transform.position);
			}
			//damageImpulse.GenerateImpulse();
			invincibilityEndTime = Time.time + invincibilityTime;
		}
	}

	public override void Die ()
	{
		
		//base.Die ();
		if(currentState != PlayerState.Dead){
			currentState = PlayerState.Dead;
			base.CallOnDeath();
			AudioManagerScript.instance.PlaySound ("Player Death", transform.position);
			if(LevelLoaderScript.instance != null){
				LevelLoaderScript.instance.ReloadScene();
			}
		}
		
	}
}
