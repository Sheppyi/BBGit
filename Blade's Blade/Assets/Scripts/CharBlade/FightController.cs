using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour {

    AnimationController animationController;
    IAttack fastUp = new IAttack();


    public class IAttack {      //needs to be class
        int damageFrame = 1;
        Vector2 origin;
        Vector2 size;
        //other variables
    }

    private void Start() {
        animationController = this.GetComponent<AnimationController>();
        InitializeAttacks();
    }

    private void InitializeAttacks() {

    }

    void Attack() {

    }
}
