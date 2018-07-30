using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(AnimationController))]

public class Player : MonoBehaviour {
    float jumpVelocity;                         //the velocity that the player Jumps with. Gets calculated in CalculatePhysics() !CAN BE OVERWRITTEN. IF YOU WANT TO CHANGE BACK TO DEFAULT CALL CALCULATE PHYSICS AGAIN!
    float gravity;                              //the gravity that is applied to velocity.y each second. Gets calculated in CalculatePhysics() !CAN BE OVERWRITTEN. IF YOU WANT TO CHANGE BACK TO DEFAULT CALL CALCULATE PHYSICS AGAIN!
    public Vector3 velocity;                    //the velocity value. all movements are stored in here and are sent over to the movement movementController for collision checking
    public Vector3 oldVelocity;                 //the velocity of the previous frame
    MovementController movementController;              //The movementcontroller script component
    AnimationController animationController;    //The animationController script component.
    AttackController attackController;

    public bool movementEnabled = true;                //Is movement enabled or not?
    public bool attacksEnabled = true;                 //are attacks enabled?
    public bool jumpEnabled = true;                    //is Jumping enabled or not?
    public bool dashEnabled = true;                    //is Dash enabled or not?
    public bool inAttack = false;
    public bool gravityEnabled = true;          //Is gravity enabled or not?
    bool lockDirection = false;                 //is the horizontal velocity not allowed to change the direction the player is facing?
    bool wasAirborne;

    float dashDoubleClickSpeedTR = 0;       //is used internally for timing the doubleclick of the dash (R= right, L = left etc)
    float dashDoubleClickSpeedTL = 0;       //is used internally for timing the doubleclick of the dash (R= right, L = left etc)
    float dashDoubleClickSpeedTU = 0;       //is used internally for timing the doubleclick of the dash (R= right, L = left etc)
    float dashDoubleClickSpeedTD = 0;       //is used internally for timing the doubleclick of the dash (R= right, L = left etc)
    public float dashLengthT = 0f;                  //is used internally to measure and time the lenght of the dash
    float dashCooldownT = 0;
    bool wasFullSpeed = false;              //turns true if the player hit full speed before velocity.x == 0. Used for animation
    public bool inDash = false;             //is the player in Dash or not
    int AirDashesT = 0;                     //the number of airdashes that are left 
    float dashActivationDelayT = 0;         //is used internally to measure and time the activation Delay for the dash
    public int facingDirection = 1;         //This is the direction the player is facing in  LEAVE PUBLIC PLS K THX
    int oldFacingDirection;                 //previous direction
    bool airborne = true;                   //Is the player in the Air or not?
    [HideInInspector]
    public float disableGravityT;             //will count down to 0 and then activate gravity
    [HideInInspector]
    public float disableMovementT;
    [HideInInspector]
    public float disableJumpT;
    [HideInInspector]
    public float disableDashT;
    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //movement props
    float jumpHeight = 4;                       //the Height that the player jumps
    float timeToJumpApex = 0.35f;               //Time to reach the highest point of the jump
    float accelerationAmount = 50;              //the speed that is added each second.
    float maxHorizontalMovementSpeed = 14;      //the maximum horizontal speed. THIS ISNT GLOBAL AND NEEDS TO BE CODED INTO A FUNCTION IF YOU WANT TO USE IT  
    float maxVerticalMovementSpeed = 20;        //the maximum vertical speed. THIS ISNT GLOBAL AND NEEDS TO BE CODED INTO A FUNCTION IF YOU WANT TO USE IT
    float airborneModifier = 0.5f;              //when holding a how much is the gravity reduced?
    float airbornRotationDeadZone = 3f;         //how fast you need to move in the air (x) in order to change the direction the player is facing in
    float noInputStopping = 1.8f;                 //this adjusts the time the character needs to stop when the player is not pressing any inputs. 2 means twice as long as when he was pressing to the opposite direction
    //animation
    float fallingAnimationThreshold = -100;       //speed needed for falling animation to play
    public bool inlandingAnimation = false;     //needs to be public for animator
    bool landingEnabled = false;                //is landing animation enabled  (ensures it doesnt play the entire time)
    float velocityNeededForLanding =  -20;      //velocity thats needed for the landing animation to play
    //dash
    Vector2 dashSpeed = new Vector2(23,23); 	//horizontal and vertical speed
    float dashDoubleClickSpeed = 0.13f;         //how fast do you have to double click in order to activate the dash
    float dashLength = 0.15f;                    //the length of the dash in seconds
    Vector2 dashExitSpeed = new Vector2(0,0);   //the speed the dash exits with
    static int AirDashes = 2;                   //the number of airdashes available to the player (maybe add upgrades in the future)
    float dashActivationDelay = 0.3f;           //the for the gravity to reactivate.  !This is skipped if the player touches the ground OR the ceiling  OR the player moves in the air!
    float dashCooldown = 0.3f;                  //the cooldown of the dash 

    //input queue
    public bool queueEnabled;
    private bool queueAttackFastUp;



    private void Start() {
        movementController = this.GetComponent<MovementController>();
        animationController = this.GetComponent<AnimationController>();
        attackController = this.GetComponent<AttackController>();
        CalculatePhysics(jumpHeight, timeToJumpApex);
        if(fallingAnimationThreshold > gravity){
            Debug.Log("Falling animation threshhold is too small");
        }
    }

    public void CalculatePhysics(float jumpHeight, float timeToJumpApex) {
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        Debug.Log("JumpVelocity =  " + jumpVelocity + "    gravity =  " + gravity);
    }

    private void Update() {
        Timings();
        Attacks();
        Movement();
        Finish();   //also includes default animations
    }

    //■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    //movement functions

    void Timings() {
        //initialization
        oldVelocity = velocity;
        wasAirborne = airborne;
        if (oldFacingDirection != facingDirection) {
            oldFacingDirection = facingDirection;
            wasFullSpeed = false;
        }
        if (movementController.collisions.above || movementController.collisions.below) {           //velocity reset vertical collision
            velocity.y = 0;
            AirDashesT = 0;
            airborne = false;
        }
        if (movementController.collisions.right || movementController.collisions.left) {            //velocity reset horizontal collision
            velocity.x = 0;
        }
        if (velocity.x != 0 && !lockDirection) {                                    //direction
            if (airborne) {
                if (velocity.x > airbornRotationDeadZone) {
                    facingDirection = 1;
                }
                else if (velocity.x < -airbornRotationDeadZone) {
                    facingDirection = -1;
                }
            }
            else {
                facingDirection = (velocity.x > 0) ? 1 : -1;
            }
        }
        if (velocity.y != 0) { airborne = true; }                                   //airborn

        //timing
        if (dashLengthT < dashLength) {                          //dash length
            dashLengthT += Time.deltaTime;
            if (dashLengthT >= dashLength) {
                MovDash(dashExitSpeed, false);
            }
        }
        if (dashActivationDelayT < dashActivationDelay + 0.1f && !gravityEnabled) { //dash activationd elay
            dashActivationDelayT += Time.deltaTime;
            if (dashActivationDelayT >= dashActivationDelay) {
                gravityEnabled = true;
                dashActivationDelayT = dashActivationDelay + 0.2f;
                movementEnabled = true;
            }
            if ((GetInput.dpad_right || GetInput.dpad_left) && dashActivationDelayT >= dashActivationDelay * 0.2) {
                movementEnabled = true;
                gravityEnabled = true;
                dashActivationDelayT = dashActivationDelay + 0.2f;
            }
        }
        if (dashCooldownT > 0) {     //dashcooldown
            dashCooldownT -= Time.deltaTime;
        }
        else if(disableDashT <= 0){
            dashEnabled = true;
        }

        //disable xxx timer (mostly used while attacking)
        if (disableGravityT > 0) {
            disableGravityT -= Time.deltaTime;
            if (disableGravityT <= 0) {
                gravityEnabled = true;
            }
        }   //same for movement
        if (disableMovementT > 0) {
            disableMovementT -= Time.deltaTime;
            if (disableMovementT <= 0) {
                movementEnabled = true;
            }
        }
        if (disableJumpT > 0) {
            disableJumpT -= Time.deltaTime;
            if (disableJumpT <= 0) {
                jumpEnabled = true;
            }
        }
        if (disableDashT > 0) {
            disableDashT -= Time.deltaTime;
            if (disableDashT <= 0) {
                dashEnabled = true;
            }
        }

    }

    void Attacks() {
        if (attacksEnabled || queueEnabled) {
            //fast attacks
            if (GetInput.bumper_left && GetInput.button_Y_pressed || queueAttackFastUp == true) {
                if (!queueEnabled) {
                    inAttack = attackController.Attack("FastUp");
                }
                else {
                    queueAttackFastUp = true;
                }
            }

        }
    }

    void Movement() {
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
    }

    void Finish() {

        //gravity
        if (gravityEnabled) {
            velocity.y += gravity * Time.deltaTime;
            if (velocity.y < -maxVerticalMovementSpeed / 2) {
                velocity.y = -maxVerticalMovementSpeed;
            }
        }

        //landing animation
        if (airborne) {
            if (oldVelocity.y <= velocityNeededForLanding) {
                landingEnabled = true;
            }
            else {
                landingEnabled = false;
            }
            inlandingAnimation = false;
        }
        else if (landingEnabled) {
            animationController.PlayAnimation("BladeLanding", this.gameObject, false, 0, null, false, true);
            landingEnabled = false;
            inlandingAnimation = true;
        }

        //default animations
        if (velocity.y <= fallingAnimationThreshold * Time.deltaTime && !inDash) {          //falling
            animationController.PlayAnimation("BladeFalling", this.gameObject, false, 0);
        }
        if (velocity.y > 0 && !inDash) {
            animationController.PlayAnimation("BladeFallingUp",this.gameObject,false,0);
        }
        if (Mathf.Abs(velocity.x) == maxHorizontalMovementSpeed) {
            wasFullSpeed = true;
        }
        else if (velocity.x == 0) {
            wasFullSpeed = false;
        }
        if (airborne) {
            wasFullSpeed = false;
        }

        //late timings
       

        //finish
        movementController.Move(velocity * Time.deltaTime);
        Physics2D.SyncTransforms(); //sync hitbox after transform
    }


    void MovMoveRight() {
        if (!airborne) {
            if (facingDirection == -1) {
                //brake animation
                if (wasFullSpeed) {
                    animationController.PlayAnimation("BladeBrake", this.gameObject, false, 0);
                    inlandingAnimation = false;
                }
                else {
                    animationController.PlayAnimation("BladeSoftBrake", this.gameObject, false, 0);
                    inlandingAnimation = false;
                }
                inlandingAnimation = false;
            }
            else {
                if (velocity.x >= maxHorizontalMovementSpeed) {
                    //running animation
                    animationController.PlayAnimation("BladeRunning", this.gameObject, false, 0, "BladeAccel");
                    inlandingAnimation = false;
                }
                else {
                    //accelanimation
                    animationController.PlayAnimation("BladeAccel", this.gameObject, false, 0);
                    inlandingAnimation = false;
                }
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
        if (wasAirborne == true && airborne == false && Mathf.Abs(velocity.x) < maxHorizontalMovementSpeed) {
            velocity.x /= 2;
        }
    }

    void MovMoveLeft() {
        if (!airborne) {
            if (facingDirection == -1) {
                if (velocity.x <= -maxHorizontalMovementSpeed) {
                    //running animation
                    animationController.PlayAnimation("BladeRunning", this.gameObject, false, 0, "BladeAccel");
                    inlandingAnimation = false;
                }
                else {
                    //accel animation
                    animationController.PlayAnimation("BladeAccel", this.gameObject, false, 0);
                    inlandingAnimation = false;
                }
            }
            else {
                //brake animatino
                if (wasFullSpeed) {
                    animationController.PlayAnimation("BladeBrake", this.gameObject, false, 0);
                    inlandingAnimation = false;
                }
                else {
                    animationController.PlayAnimation("BladeSoftBrake", this.gameObject, false, 0);
                    inlandingAnimation = false;
                }
                inlandingAnimation = false;
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
        if (wasAirborne == true && airborne == false && Mathf.Abs(velocity.x) < maxHorizontalMovementSpeed) {
            velocity.x /= 2;
        }
    }

    void MovMoveNone() {
        if (!airborne) {
            if (facingDirection == -1) {
                velocity.x += accelerationAmount * Time.deltaTime / noInputStopping;
                if (velocity.x > 0) {
                    velocity.x = 0;
                }
                else if (velocity.x > -8 && animationController.currentAnimation != "BladeBrakeToIdle") {
                    if (wasFullSpeed) {
                        animationController.PlayAnimation("BladeBrake", this.gameObject, false, 0);
                        inlandingAnimation = false;
                    }
                    else {
                        animationController.PlayAnimation("BladeSoftBrake", this.gameObject, false, 0);
                        inlandingAnimation = false;
                    }
                }
                else {
                    animationController.PlayAnimation("BladeAccel", this.gameObject, false, 0, "BladeRunning");
                    inlandingAnimation = false;
                }
            }
            else {
                velocity.x -= accelerationAmount * Time.deltaTime / noInputStopping;
                if (velocity.x < 0) {
                    velocity.x = 0;
                }
                else if (velocity.x < 8 && animationController.currentAnimation != "BladeBrakeToIdle") {
                    if (wasFullSpeed) {
                        animationController.PlayAnimation("BladeBrake", this.gameObject, false, 0);
                        inlandingAnimation = false;
                    }
                    else {
                        animationController.PlayAnimation("BladeSoftBrake", this.gameObject, false, 0);
                        inlandingAnimation = false;
                    }                
                }
                else {
                    animationController.PlayAnimation("BladeAccel", this.gameObject, false, 0, "BladeRunning");
                    inlandingAnimation = false;
                }
            }
            //animation
            if (velocity.x == 0 && velocity.y == 0 && !inDash && !inlandingAnimation) {
                animationController.PlayAnimation("BladeIdle", this.gameObject, false, 3);
            }
        }
        else {
            //airborne
            if (facingDirection == -1) {
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
        inlandingAnimation = false;
        velocity.y += jumpVelocity;
        airborne = true;
        animationController.PlayAnimation("BladeJumping", this.gameObject, false, 0);
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
        inlandingAnimation = false;
        if (activate) {
            EnableQueue();
            attacksEnabled = false;
            gravityEnabled = false;
            dashActivationDelayT = dashActivationDelay + 0.2f;
            movementEnabled = false;
            jumpEnabled = false;
            dashEnabled = false;
            inDash = true;
            lockDirection = true;
            velocity = direction * dashSpeed;
            dashLengthT = 0;
            AirDashesT++;
            if (!airborne) {
                dashCooldownT = dashCooldown;
            }
            else {
                dashCooldownT = dashLength;
            }
            //animation
            if (direction == Vector2.up) {
                animationController.PlayAnimation("BladeDashUp", this.gameObject, true, 0, null, false, true);
            }else if (direction == Vector2.down) {
                animationController.PlayAnimation("BladeDashDown", this.gameObject, true, 0, null, false, true);
            }
            else if (airborne) {
                if ((direction == Vector2.left && facingDirection == 1) || (direction == Vector2.right && facingDirection == -1)) {
                    animationController.PlayAnimation("BladeDashHorizontalBackwards", this.gameObject, true, 0, null, false, true);
                }
                else if (direction == Vector2.left || direction == Vector2.right) {
                    animationController.PlayAnimation("BladeDashHorizontalForward", this.gameObject, true, 0, null, false, true);
                }
            }
            else {
                if ((direction == Vector2.left && facingDirection == -1) || (direction == Vector2.right && facingDirection == 1)) {
                    animationController.PlayAnimation("BladeDashHorizontalForwardGrounded", this.gameObject, true, 0, null, false, true);
                }
                else if(direction == Vector2.left || direction == Vector2.right){
                    animationController.PlayAnimation("BladeDashHorizontalBackwardsGrounded", this.gameObject, true, 0, null, false, true);
                }
            }
        }
        else {
        	movementController.Move(new Vector2(0, -0.01f));	//to recheck collision
            if (!movementController.collisions.below) { airborne = true; } else {airborne = false;}
            if (!airborne) {
                movementEnabled = true;
                gravityEnabled = true;
            }
            else {
                dashActivationDelayT = 0;
            }
            jumpEnabled = true;
            inDash = false;
            attacksEnabled = true;
            lockDirection = false;
            dashLengthT = dashLength;
            velocity = dashExitSpeed;
            EndQueue();
        }
    }


    void EnableQueue() {
        queueEnabled = true;
    }

    void EndQueue() {
        queueEnabled = false;
        //call that shit
        Attacks();
        //reset that shit
        queueAttackFastUp = false;
    }
}

    
