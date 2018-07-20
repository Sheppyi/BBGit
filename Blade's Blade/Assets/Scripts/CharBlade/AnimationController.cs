using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    SpriteRenderer currentSpriteRenderer;
    public int currentSpriteFrame = 0;
    Sprite[] currentSprite = new Sprite[50];    //kann so groß sein wie will nur nicht zu klein
    int currentAnimationLength;
    bool doesLoop;                   
    float currentFrameLength;
    float currentFrameLengthT;
    string currentAnimation = "null";
    int currentLoopFrame = 1;
    bool playAnimationAfterLastFrame;
    GameObject currentObject;

    public Sprite[] BladeIdle = new Sprite[12];
    int BladeIdleFrameRate =10;
    bool BladeIdleLoop = true;
    int BladeIdleLoopFrame = 1;

    public Sprite[] BladeDashHorizontalBackwards = new Sprite[5];
    int BladeDashHorizontalBackwardsFrameRate = 11;
    bool BladeDashHorizontalBackwardsLoop = false;
    int BladeDashHorizontalBackwardsLoopFrame = 1;

    public Sprite[] bladeFalling = new Sprite[8];
    int bladeFallingFrameRate = 15;
    bool bladeFallingLoop = true;
    int bladeFallingLoopFrame = 4;

    public Sprite[] bladeJumping = new Sprite[2];
    int bladeJumpingFrameRate = 20;
    bool bladeJumpingLoop = true;
    int bladeJumpingLoopFrame = 2;

    public Sprite[] bladeLanding = new Sprite[9];
    int bladeLandingFrameRate = 20;
    bool bladeLandingLoop = true;
    int bladeLandingLoopFrame = 1;

    public Sprite[] bladeRunning = new Sprite[6];
    int bladeRunningFrameRate = 14;
    bool bladeRunningLoop = true;
    int bladeRunningLoopFrame = 1;

    public Sprite[] bladeAccel = new Sprite[6];
    int bladeAccelFrameRate = 10;
    bool bladeAccelLoop = true;
    int bladeAccelLoopFrame = 1;

    //THIS PLAYS AN ANIMATION   (THE ANIMATION NAME, THE GAMEOBJECT THAT THE ANIMATION WILL BE APPLIED TO, WEATHER THE ANIMATION WILL RESET ITSELF IF CALLED AGAIN,THE STARTING FRAME, NAME OF THE ANIMATION A SMOOTH TRANSITION IS DONE WITH)
    public void PlayAnimation(string animation, GameObject toChange, bool overrideAnimation, int startAtFrame, string smoothTransitionWithAnimation = null) {
        if (animation == currentAnimation && !overrideAnimation) {      //exits if an animation that is already running is called again                                 <-----------
            return;
        }
        if (smoothTransitionWithAnimation != currentAnimation) {        //allows for smooth transition
            currentSpriteFrame = startAtFrame;
            currentFrameLengthT = 0;
        }
        currentObject = toChange;                                               //the object that called the function
        currentSpriteRenderer = toChange.GetComponent<SpriteRenderer>();        //the SpriteRenderer that called the function
        currentAnimation = animation;                                           //saves the animation string locally to compare with new animations that are being called. See above 
        switch (animation) {
            case "BladeIdle":
                currentFrameLength = 1f / BladeIdleFrameRate;  //f is important for floatpoint division
                doesLoop = BladeIdleLoop;
                currentLoopFrame = BladeIdleLoopFrame;
                playAnimationAfterLastFrame = false;
                currentAnimationLength = BladeIdle.Length;
                for (int i = 0; i < BladeIdle.Length; i++) {
                    currentSprite[i] = BladeIdle[i];
                }
                break;
            case "BladeDashHorizontalBackwards":
                currentFrameLength = 1f / BladeDashHorizontalBackwardsFrameRate;
                doesLoop = BladeDashHorizontalBackwardsLoop;
                playAnimationAfterLastFrame = false;
                currentLoopFrame = BladeDashHorizontalBackwardsLoopFrame;
                currentAnimationLength = BladeDashHorizontalBackwards.Length;
                for (int i = 0; i < BladeDashHorizontalBackwards.Length; i++) {
                    currentSprite[i] = BladeDashHorizontalBackwards[i];
                }
                break;
            case "BladeFalling":
                currentFrameLength = 1f / bladeFallingFrameRate;
                doesLoop = bladeFallingLoop;
                currentLoopFrame = bladeFallingLoopFrame;
                playAnimationAfterLastFrame = false;
                currentAnimationLength = bladeFalling.Length;
                for (int i = 0; i < bladeFalling.Length; i++) {
                    currentSprite[i] = bladeFalling[i];
                }
                break;
            case "BladeJumping":
                currentFrameLength = 1f / bladeJumpingFrameRate;
                doesLoop = bladeJumpingLoop;
                currentLoopFrame = bladeJumpingLoopFrame;
                playAnimationAfterLastFrame = false;
                currentAnimationLength = bladeJumping.Length;
                for (int i = 0; i < bladeJumping.Length; i++) {
                    currentSprite[i] = bladeJumping[i];
                }
                break;
            case "BladeLanding":
                currentFrameLength = 1f / bladeLandingFrameRate;
                doesLoop = bladeLandingLoop;
                currentLoopFrame = bladeLandingLoopFrame;
                playAnimationAfterLastFrame = true;
                currentAnimationLength = bladeLanding.Length;
                for (int i = 0; i < bladeLanding.Length; i++) {
                    currentSprite[i] = bladeLanding[i];
                }
                break;
            case "BladeRunning":
                currentFrameLength = 1f / bladeRunningFrameRate;
                doesLoop = bladeRunningLoop;
                currentLoopFrame = bladeRunningLoopFrame;
                playAnimationAfterLastFrame = false;
                currentAnimationLength = bladeRunning.Length;
                for (int i = 0; i < bladeRunning.Length; i++) {
                    currentSprite[i] = bladeRunning[i];
                }
                break;
            case "BladeAccel":
                currentFrameLength = 1f / bladeAccelFrameRate;
                doesLoop = bladeAccelLoop;
                currentLoopFrame = bladeAccelLoopFrame;
                playAnimationAfterLastFrame = false;
                currentAnimationLength = bladeAccel.Length;
                for (int i = 0; i < bladeAccel.Length; i++) {
                    currentSprite[i] = bladeAccel[i];
                }
                break;
            default:
                Debug.LogWarning("non-existent Animation String passed");
                break;
        }
        applySprite(currentSprite[currentSpriteFrame]);
    }


    public void Update() {
        if (currentSpriteRenderer != null) {
            currentFrameLengthT += Time.deltaTime;
            if (currentFrameLengthT >= currentFrameLength) {
                currentFrameLengthT = 0;
                currentSpriteFrame++;
                if (currentSpriteFrame >= currentAnimationLength && playAnimationAfterLastFrame) {
                    switch (currentAnimation) {
                        case "BladeLanding":
                            PlayAnimation("BladeIdle",currentObject,false, 3);
                            currentObject.GetComponent<Player>().inlandingAnimation = false;
                            break;
                        default:
                            Debug.Log("Automatic animation play after another animation has incorrect string");
                            break;
                    }
                }
                if (currentSpriteFrame >= currentAnimationLength && doesLoop) {
                    currentSpriteFrame = currentLoopFrame - 1;
                }
                if (currentSpriteFrame >= currentAnimationLength && !doesLoop) {
                    currentSpriteFrame = currentAnimationLength - 1;
                }
                applySprite(currentSprite[currentSpriteFrame]);
            }
        }
    }

    public void applySprite(Sprite sprite){
        currentSpriteRenderer.sprite = sprite;
            if(this.GetComponent<Player>().facingDirection == 1){
                currentSpriteRenderer.flipX = false;
            }
            else{
                currentSpriteRenderer.flipX = true;
            }
    }



}
