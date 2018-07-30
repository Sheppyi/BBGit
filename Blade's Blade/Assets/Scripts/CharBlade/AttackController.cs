using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour {

    AnimationController animationController;
    Player player;
    public IAttack fastUp = new IAttack();
    public IAttack current = new IAttack();
    Iorigin origin = new Iorigin();
    public float cooldownT;
    public LayerMask collisionMask;
 
    [HideInInspector]
    public int[] currentDamageFrame;
    private const int maxDamageFrames = 3;    //should be 3

    [HideInInspector]
    public class IAttack {      //needs to be class //THIS IS THE IATTACK CLASS. EVERY ATTACK DERIVES FROM THIS CLASS
        public int[] damageFrames = new int[maxDamageFrames];
        public Vector2[] origin = new Vector2[maxDamageFrames];
        public Vector2[] size = new Vector2[maxDamageFrames];       //width , height
        public Vector2[] rayCastDirection = new Vector2[maxDamageFrames];
        public float disableGravityTime;        //gravity time and movement time should usually be the same
        public float disableMovementTime;
        public float disableDashTime;
        public float disableJumpTime;
        public float cooldown;      //the cooldown for the attack. Shouldn't exceed animation length
        public int damage;          //the damage the attack deals
        public Vector2 momentum;    //the speed the player has when he uses this attack. By default its {0,0}
        //other variables
    }

    public class Iorigin {
        public Vector2 bottomLeft = new Vector2();
        public Vector2 bottomRight = new Vector2();
        public Vector2 topLeft = new Vector2();
        public Vector2 topRight = new Vector2();
        public void SetAll(Vector2 vector) {
            bottomLeft = bottomRight = topLeft = topRight = vector;
        }
    }

    void Start () {
        animationController = this.GetComponent<AnimationController>();
        player = this.GetComponent<Player>();
        currentDamageFrame = new int[] { 99, 99, 99 };
        Invoke("InitializeAttacks", 0f);    //calling the function normally breaks everything
    }

     void InitializeAttacks() {
        //fastUp
        fastUp.damageFrames = new int[] { 0, 99, 99 };
        fastUp.origin[0] = new Vector2(0, 0.45f);
        fastUp.size[0] = new Vector2(0.5f, 1);
        fastUp.rayCastDirection[0] = Vector2.up;
        fastUp.disableGravityTime = 0.3f;
        fastUp.disableMovementTime = 0.3f;
        fastUp.disableDashTime = 0.1f;
        fastUp.disableJumpTime = 0.3f;
        fastUp.cooldown = 0.3f;
        fastUp.damage = 3;
    }

    public bool Attack(string attack) {
        if (cooldownT <= 0) {
            player.inlandingAnimation = false;
            switch (attack) {
                case "FastUp":
                    for (int i = 0; i < maxDamageFrames; i++) {
                        currentDamageFrame[i] = fastUp.damageFrames[i];
                    }
                    if (fastUp.disableGravityTime > 0) {
                        player.gravityEnabled = false;
                        player.disableGravityT = fastUp.disableGravityTime;
                    }
                    if (fastUp.disableMovementTime > 0) {
                        player.movementEnabled = false;
                        player.disableMovementT = fastUp.disableMovementTime;
                    }
                    if (fastUp.disableJumpTime > 0) {
                        player.jumpEnabled = false;
                        player.disableJumpT = fastUp.disableJumpTime;
                    }
                    if (fastUp.disableDashTime > 0) {
                        player.dashEnabled = false;
                        player.disableDashT = fastUp.disableDashTime;
                    }
                    cooldownT = fastUp.cooldown;
                    player.velocity = fastUp.momentum;
                    current.size = fastUp.size;
                    current.origin = fastUp.origin;
                    current.damage = fastUp.damage;
                    current.rayCastDirection = fastUp.rayCastDirection;
                    animationController.PlayAnimation("BladeFastUp", this.gameObject, false, 0, null, true);
                    break;
                default:
                    Debug.Log("non existent attack string passed");
                    cancelAttack();
                    return false;
            }
            return true;
        }
        else {
            return false;
        }
    }

    public void ApplyDamage(int frame) {
        int raycount = 5;
        float raySpacing = current.size[frame].x / raycount ;
        origin.SetAll(new Vector2(transform.position.x, transform.position.y));
        if (current.rayCastDirection[frame] == Vector2.up) {
            origin.bottomRight += new Vector2(current.origin[frame].x * player.facingDirection - current.size[frame].x / 2 + raySpacing/2, current.origin[frame].y - current.size[frame].y / 2 + raySpacing/2);
            for (int i = 0; i < raycount; i++) {
                RaycastHit2D hit = Physics2D.Raycast(origin.bottomRight, current.rayCastDirection[frame], current.size[frame].y, collisionMask);
                Debug.DrawRay(origin.bottomRight, Vector2.up * current.size[frame].y, Color.red);
                origin.bottomRight.x += raySpacing;
                if (hit.collider != null) {
                    i = raycount;   //exits out of the loop
                    bool wasKilled;
                    try {
                        wasKilled = hit.collider.GetComponent<DamageController>().TakeDamage(current.damage);
                    }
                    catch {
                        Debug.Log("the object hit doesnt have a DamageController");
                        continue;
                    }
                    if (wasKilled) {
                        Debug.Log("The enemy was killed");
                    }
                }
            }
        }
    }

    public void cancelAttack() {
        animationController.isAttack = false;
        player.inAttack = false;
    }

    void Update () {
        if (cooldownT > 0) {
            cooldownT -= Time.deltaTime;
        }
    }
}
