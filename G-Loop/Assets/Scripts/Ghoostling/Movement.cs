using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    public bool AcceptPlayerInput = true;
    public const float SPEED = 12f;
    private float speedFactor = 1f;
    public const float GRAVITY = -30f;
    public const float JUMP_HEIGHT = 2f;

    // 0  = instant accelleration (infinetely snappy)
    // 1  = default
    // >1 = slower (not snappy)
    public const float MOVEMENT_SNAPINESS = 0.3f;
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Vector3 velocity;
    private bool isGrounded;
    public Animator anim;
    public float pushPower = 10f;
    public float weight = 1f;
    private GhoostlingManager gman;
    private GooseController gcon;
    private void Start() {
        gman = GhoostlingManager.GetInstance();
        gcon = GetComponentInChildren<GooseController>();
    }

    private static float Snap(float d) {
        return Mathf.Sign(d) * Mathf.Pow(Mathf.Abs(d), MOVEMENT_SNAPINESS);
    }

//------------------------------------------------------------------------------------------------------------------------------
    private void OnControllerColliderHit(ControllerColliderHit hit) {
        /*
        TODO:
            If the object that was hit is a goose and that goose's id is
            higher than our id, push that goose out of the way.  Issue #17
        */

        GooseController other_goose= hit.gameObject.GetComponentInChildren<GooseController>();
        Rigidbody body = hit.collider.attachedRigidbody;
        Vector3 force;

        if (other_goose!= null && other_goose!= gcon){ 
            if(other_goose.GetId() < gcon.GetId()){
                Debug.Log("I get push: " + other_goose.GetId() + "   I push: " + gcon.GetId());
                if(other_goose.GetError() >= 0.1f)
                velocity.y = Mathf.Sqrt(120f);  
                //apply
            }
            else{
                Debug.Log("I am a stone: " + other_goose.GetId());
            }
            /* if(gcon.GetState() == GooseController.GooseState.ACTIVE)
                Debug.Log("Collided Goose: " + other_goose.GetId()); */
        }


        if (body == null || body.isKinematic){
            return;
        }

        //Debug.Log(hit.moveDirection);
        if (hit.moveDirection.y < -0.3){
            force = new Vector3(0f, 0.5f, 0f) * Movement.GRAVITY * weight;
        } else {
            force = hit.controller.velocity.magnitude * hit.normal * -1 * pushPower * 100;
        }
        body.AddForceAtPosition(force, hit.point);
    }
//------------------------------------------------------------------------------------------------------------------------------

    void FixedUpdate() {
        // Inputs are managed by GooseController, which invokes the ProcessInputs method
    }

    public void PerformGroundCheck() {
        isGrounded = Physics.CheckSphere(groundCheck.position,
                GroundCheck.GROUND_CHECK_RADIUS,
                groundLayer
        );
    }

    public void Crouch(bool crouch = true) {
        anim.SetBool("Crouch", crouch);
        speedFactor = crouch ? 0.5f : 1f;
    }

    public void ProcessInputs(GhoostlingData.UserInputs inputs) {
        PerformGroundCheck();

        float x = Snap(inputs.horizontal);
        float z = Snap(inputs.vertical);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * SPEED * speedFactor * Time.deltaTime);

        if (isGrounded && inputs.jumpButtonDown) {
            velocity.y = Mathf.Sqrt(JUMP_HEIGHT * 2f * -GRAVITY);
        }
        
        velocity.y += GRAVITY * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (inputs.crouchButtonDown) {
            // TODO crouch 
        } else if (inputs.crouchButtonUp) {
            // TODO uncrouch 
        }
    }

}
