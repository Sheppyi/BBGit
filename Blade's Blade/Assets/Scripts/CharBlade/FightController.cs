using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour {


    AnimationController animationController;
    Player player;
    public IAttack fastUp = new IAttack();

    
    public int[] currentDamageFrame;


    [HideInInspector]
    public class IAttack {      //needs to be class
        public int[] damageFrames = new int[3];
        public Vector2[] origin;
        public Vector2[] size;
        public float disableGravityTime;
        //other variables
    }

    private void Start() {
        animationController = this.GetComponent<AnimationController>();
        player = this.GetComponent<Player>();
        currentDamageFrame = new int[]{99,99,99};
        InitializeAttacks();
    }

    private void InitializeAttacks() {
        //fastUp
        fastUp.damageFrames = new int[]{0,99,99};
        fastUp.origin[0] = new Vector2(3,0);
        fastUp.size[0] = new Vector2(1,6);
        fastUp.disableGravityTime = 0f;
    }

    public void Attack(string attack) {
        player.inlandingAnimation = false;
        switch (attack){
            case "FastUp":
                for (int i = 0; i < 3; i++) {
                    currentDamageFrame[i] = fastUp.damageFrames[i];
                }
                animationController.PlayAnimation("BladeFastUp",this.gameObject,false,0,null,true);
                break;
            default:
                Debug.Log("non existent attack string passed");
                cancelAttack();
                return;
        }
    }

    public void ApplyDamage(int frame){
        Debug.Log("Attack");
    }

    public void cancelAttack(){
        animationController.isAttack = false;
    }
}
