using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]

public class Player : MonoBehaviour {
    float jumpVelocity;
    float gravity;
    Vector3 velocity;
    MovementController controller;

    bool movementEnabled = true;
    bool gravityEnabled = true;
    bool jumpEnabled = true;
    bool dashEnabled = true;
    bool allowGravityReduction = true;
        float dashDoubleClickSpeedTR = 0;
        float dashDoubleClickSpeedTL = 0;
        float dashDoubleClickSpeedTU = 0;
        float dashDoubleClickSpeedTD = 0;
        float dashLengthT = 0;
        bool inDash = false;
        int AirDashesT = 0;
    float dashActivationDelayT = 0;
    int directionX = 1;
    bool airborne;
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //movement props
    float jumpHeight = 4;
    float timeToJumpApex = 0.35f;
    float accelerationAmount = 50;
    float maxHorizontalMovementSpeed = 12;
    float airborneModifier = 0.5f;
    float gravityReductionAmount = 0.6f;        //reduction when holding space  (1 is 1:1 (lower is more))
    //dash
    Vector2 dashSpeed = new Vector2(25,25); 	//horizontal and vertical speed
    float dashDoubleClickSpeed = 0.13f;
    float dashLength = 0.1f;
    Vector2 dashExitSpeed = new Vector2(0,0);
    static int AirDashes = 2;
    float dashActivationDelay = 0.3f;




    private void Start() {
        controller = GetComponent<MovementController>();
        CalculatePhysics(jumpHeight, timeToJumpApex);
    }

    private void Update() {
        //initilization
        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
            AirDashesT = 0;
            airborne = false;
        }
        if (controller.collisions.right || controller.collisions.left) {
            velocity.x = 0;
        }
        if (velocity.x != 0) {   //set the direction the character is looking at
            directionX = (velocity.x > 0) ? 1 : -1;
        }
        if (velocity.y != 0) { airborne = true; }

        //timing
        if (dashLengthT < dashLength + 0.1f) {
            dashLengthT += Time.deltaTime;
            if(dashLengthT >= dashLength) {
                MovDash(dashExitSpeed , false);
            }
        }
        if (dashActivationDelayT < dashActivationDelay + 0.1f && !gravityEnabled) {
            dashActivationDelayT += Time.deltaTime;
            if (dashActivationDelayT >= dashActivationDelay) {
                gravityEnabled = true;
                dashActivationDelayT = dashActivationDelay + 0.2f;
                movementEnabled = true;
            }
            if ((GetInput.dpad_right || GetInput.dpad_left) && dashActivationDelayT >= dashActivationDelay * 0.1) {
                movementEnabled = true;
                gravityEnabled = true;
                dashActivationDelayT = dashActivationDelay + 0.2f;
            }
        }
        
        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
        //default movement
        if (movementEnabled) {

            //moving Right
            if (GetInput.dpad_right) {
                MovMoveRight();
            }   //moving left
            else if (GetInput.dpad_left) {
                MovMoveLeft();
            }
            else {
                MovMoveNone();
            }
        }
        //jump
        if (jumpEnabled && !airborne && GetInput.button_A_pressed) {
                MovJump();
        }
        //dash
        if (dashEnabled && AirDashesT < AirDashes) {
        		CheckDash();
        }
        //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■

        //gravity
        if (gravityEnabled) {
            if(allowGravityReduction && GetInput.button_A && velocity.y <= 0) {
                velocity.y += gravity * Time.deltaTime * gravityReductionAmount;
            }
            else {
                velocity.y += gravity * Time.deltaTime;
            }
        }

        //finish
        controller.Move(velocity * Time.deltaTime);
    }

    public void CalculatePhysics(float jumpHeight, float timeToJumpApex) {
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        Debug.Log("JumpVelocity =  " + jumpVelocity + "    gravity =  " + gravity);
    }





    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //movement functions
    void MovMoveRight() {
        if (!airborne) {
            if (directionX == -1) {
                //play brake animation
            }
            else {
                //play accel animation
            }
            if (velocity.x + accelerationAmount * Time.deltaTime < maxHorizontalMovementSpeed) {
                velocity.x += accelerationAmount * Time.deltaTime;
                if (velocity.x + accelerationAmount * Time.deltaTime > maxHorizontalMovementSpeed) {
                    velocity.x = maxHorizontalMovementSpeed;
                }
            }
        }
        else {
            if (velocity.x + accelerationAmount * Time.deltaTime * airborneModifier < maxHorizontalMovementSpeed) {
                velocity.x += accelerationAmount * Time.deltaTime * airborneModifier;
                if (velocity.x + accelerationAmount * Time.deltaTime * airborneModifier > maxHorizontalMovementSpeed) {
                    velocity.x = maxHorizontalMovementSpeed;
                }
            }
        }
    }

    void MovMoveLeft() {
        if (!airborne) {
            if (directionX == -1) {
                //play brake animation
            }
            else {
                //play accel animation
            }
            if (velocity.x - accelerationAmount * Time.deltaTime > -maxHorizontalMovementSpeed) {
                velocity.x -= accelerationAmount * Time.deltaTime;
                if (velocity.x - accelerationAmount * Time.deltaTime < -maxHorizontalMovementSpeed) {
                    velocity.x = -maxHorizontalMovementSpeed;
                }
            }
        }
        else {
            //airborn
            if (velocity.x - accelerationAmount * Time.deltaTime * airborneModifier > -maxHorizontalMovementSpeed) {
                velocity.x -= accelerationAmount * Time.deltaTime * airborneModifier;
                if (velocity.x - accelerationAmount * Time.deltaTime * airborneModifier < -maxHorizontalMovementSpeed) {
                    velocity.x = -maxHorizontalMovementSpeed;
                }
            }
        }
    }

    void MovMoveNone() {
        if (!airborne) {
            if (directionX == -1) {
                velocity.x += accelerationAmount * Time.deltaTime;
                if (velocity.x > 0) {
                    velocity.x = 0;
                }
            }
            else {
                velocity.x -= accelerationAmount * Time.deltaTime;
                if (velocity.x < 0) {
                    velocity.x = 0;
                }
            }
        }
        else {
            //airborne
            if (directionX == -1) {
                velocity.x += accelerationAmount * Time.deltaTime * airborneModifier;
                if (velocity.x > 0) {
                    velocity.x = 0;
                }
            }
            else {
                velocity.x -= accelerationAmount * Time.deltaTime * airborneModifier;
                if (velocity.x < 0) {
                    velocity.x = 0;
                }
            }
        }

    }

    void MovJump() {
        velocity.y += jumpVelocity;
        airborne = true;
    }

    void CheckDash(){
		//analog activation
		if(GetInput.axis_left_right_pressed){
			MovDash(Vector2.right, true);
		}
		if(GetInput.axis_left_left_pressed){
			MovDash(Vector2.left, true);
		}
		if(GetInput.axis_left_up_pressed){
			MovDash(Vector2.up, true);
		}
		if(GetInput.axis_left_down_pressed){
			MovDash(Vector2.down, true);
		}
    	//dpad double click
        if (GetInput.dpad_right_released) {
            dashDoubleClickSpeedTR = 0;
        }
        if (!GetInput.dpad_right) {
            if(dashDoubleClickSpeedTR < dashDoubleClickSpeed + 0.1f) {
                dashDoubleClickSpeedTR += Time.deltaTime;
            }
        }
        if (GetInput.dpad_right_pressed) {
            if (dashDoubleClickSpeedTR < dashDoubleClickSpeed) {
                dashDoubleClickSpeedTR = dashDoubleClickSpeed + 0.1f;
                MovDash(Vector2.right , true);
            }
        }
        if (GetInput.dpad_left_released) {
            dashDoubleClickSpeedTL = 0;
        }
        if (!GetInput.dpad_left) {
            if (dashDoubleClickSpeedTL < dashDoubleClickSpeed + 0.1f) {
                dashDoubleClickSpeedTL += Time.deltaTime;
            }
        }
        if (GetInput.dpad_left_pressed) {
            if (dashDoubleClickSpeedTL < dashDoubleClickSpeed) {
                dashDoubleClickSpeedTL = dashDoubleClickSpeed + 0.1f;
                MovDash(Vector2.left, true);
            }
        }

        if (GetInput.dpad_up_released) {
            dashDoubleClickSpeedTU = 0;
        }
        if (!GetInput.dpad_up) {
            if (dashDoubleClickSpeedTU < dashDoubleClickSpeed + 0.1f) {
                dashDoubleClickSpeedTU += Time.deltaTime;
            }
        }
        if (GetInput.dpad_up_pressed) {
            if (dashDoubleClickSpeedTU < dashDoubleClickSpeed) {
                dashDoubleClickSpeedTU = dashDoubleClickSpeed + 0.1f;
                MovDash(Vector2.up, true);
            }
        }
        if (GetInput.dpad_down_released) {
            dashDoubleClickSpeedTD = 0;
        }
        if (!GetInput.dpad_down) {
            if (dashDoubleClickSpeedTD < dashDoubleClickSpeed + 0.1f) {
                dashDoubleClickSpeedTD += Time.deltaTime;
            }
        }
        if (GetInput.dpad_down_pressed) {
            if (dashDoubleClickSpeedTD < dashDoubleClickSpeed) {
                dashDoubleClickSpeedTD = dashDoubleClickSpeed + 0.1f;
                MovDash(Vector2.down, true);
            }
        }
    }

    void MovDash(Vector2 direction, bool activate) {
        if (activate) {
            gravityEnabled = false;
            dashActivationDelayT = dashActivationDelay + 0.2f;
            movementEnabled = false;
            jumpEnabled = false;
            dashEnabled = false;
            inDash = true;
            velocity = direction * dashSpeed;
            dashLengthT = 0;
            AirDashesT++;
        }
        else {
        	controller.Move(new Vector2(0, -0.01f));	//to recheck collision
            if (!controller.collisions.below) { airborne = true; } else {airborne = false;}
            if (!airborne) {
                movementEnabled = true;
                gravityEnabled = true;
            }
            else {
                dashActivationDelayT = 0;
            }
            jumpEnabled = true;
            dashEnabled = true;
            inDash = false;
            dashLengthT = dashLength + 0.1f;
            velocity = dashExitSpeed;
            
        }
    }
}

    
