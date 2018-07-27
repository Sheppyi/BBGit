using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour {

    public bool inAttack = false;

    AnimationController animationController;
    Player player;
    public IAttack fastUp = new IAttack();

    
    public int[] currentDamageFrame;


    [HideInInspector]
    public class IAttack {      //needs to be class
        public int[] damageFrames = new int[3];
        public Vector2[] origin;
        public Vector2[] size;
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
    }

    public void Attack(string attack) {
        player.inlandingAnimation = false;
        switch (attack){
            case "FastUp":
                animationController.PlayAnimation("BladeFastUp",this.gameObject,false,0,null,true);
                inAttack = true;    //THIS NEEDS TO BE HERE BECAUSE IT NEEDS TO BE CHANGED AFTER THE ANIMATION IS CALLED
                animationController.checkForDamageFrame();
                for(int i = 0; i < 3; i++){
                    currentDamageFrame[i] = fastUp.damageFrames[i];
                    Debug.Log(currentDamageFrame[i]);
                }
                break;
            default:
                Debug.Log("non existent attack string passed");
                cancelAttack();
                return;
        }
    }

    public void applyDamage(int frame){
        if(inAttack){
            Debug.Log("Attack");
        }else{
            Debug.Log("Trying to apply damage but no attacka animation is playing");
        }
    }

    public void cancelAttack(){
        inAttack = false;
        animationController.isAttack = false;
    }
}
