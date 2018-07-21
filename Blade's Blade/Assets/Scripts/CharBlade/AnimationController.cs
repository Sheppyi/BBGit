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
    public string currentAnimation = "null";
    int currentLoopFrame = 1;
    bool playAnimationAfterLastFrame;
    GameObject currentObject;

    public ISpriteAnimation bladeIdle = new ISpriteAnimation();
    public ISpriteAnimation bladeDashBackwards = new ISpriteAnimation();
    public ISpriteAnimation bladeDashBackwardsGrounded = new ISpriteAnimation();
    public ISpriteAnimation bladeDashForwards = new ISpriteAnimation();
    public ISpriteAnimation bladeDashForwardsGrounded = new ISpriteAnimation();
    public ISpriteAnimation bladeDashUp = new ISpriteAnimation();
    public ISpriteAnimation bladeFalling = new ISpriteAnimation();
    public ISpriteAnimation bladeJumping = new ISpriteAnimation();
    public ISpriteAnimation bladeLanding = new ISpriteAnimation();
    public ISpriteAnimation bladeRunning = new ISpriteAnimation();
    public ISpriteAnimation bladeAccel = new ISpriteAnimation();
    public ISpriteAnimation bladeBrake = new ISpriteAnimation();
    public ISpriteAnimation bladeSoftBrake = new ISpriteAnimation();
    public ISpriteAnimation bladeBrakeToIdle = new ISpriteAnimation();
    public ISpriteAnimation bladeDashDown = new ISpriteAnimation();

    [System.Serializable]
    public class ISpriteAnimation {     //needs to be class in order to show in inspector but its bascially a struct
        public Sprite[] sprites;
        [HideInInspector]
        public int frameRate;
        [HideInInspector]
        public bool loop;
        [HideInInspector]
        public int loopFrame = 1;
        [HideInInspector]
        public bool playAnimationAfter = false;
    }

    private void Start() {
        InitializeSpriteAnimations();
    }

    private void InitializeSpriteAnimations() {
        bladeIdle.frameRate = 10;
        bladeIdle.loop = true;
        bladeIdle.loopFrame = 1;

        bladeDashBackwards.frameRate = 20;
        bladeDashBackwards.loop = false;

        bladeDashForwards.frameRate = 20;
        bladeDashForwards.loop = false;

        bladeDashBackwardsGrounded.frameRate = 20;
        bladeDashBackwardsGrounded.loop = false;
        bladeDashBackwardsGrounded.loopFrame = 1;
        bladeDashBackwardsGrounded.playAnimationAfter = true;

        bladeDashForwardsGrounded.frameRate = 20;
        bladeDashForwardsGrounded.loop = false;

        bladeDashUp.frameRate = 15;
        bladeDashUp.loop = false;

        bladeDashDown.frameRate = 15;
        bladeDashDown.loop = false;

        bladeFalling.frameRate = 15;
        bladeFalling.loop = true;
        bladeFalling.loopFrame = 4;

        bladeJumping.frameRate = 20;
        bladeJumping.loop = true;
        bladeJumping.loopFrame = 2;

        bladeLanding.frameRate = 20;
        bladeLanding.loop = true;
        bladeLanding.loopFrame = 1;
        bladeLanding.playAnimationAfter = true;

        bladeRunning.frameRate = 14;
        bladeRunning.loop = true;
        bladeRunning.loopFrame = 1;

        bladeAccel.frameRate = 10;
        bladeAccel.loop = true;
        bladeAccel.loopFrame = 1;

        bladeBrake.frameRate = 15;
        bladeBrake.loop = true;
        bladeBrake.loopFrame = 2;

        bladeSoftBrake.frameRate = 10;
        bladeSoftBrake.loop = true;
        bladeSoftBrake.loopFrame = 2;

        bladeBrakeToIdle.frameRate = 45;
        bladeBrakeToIdle.loop = false;
        bladeBrakeToIdle.loopFrame = 1;

    }


    //THIS PLAYS AN ANIMATION   (THE ANIMATION NAME, THE GAMEOBJECT THAT THE ANIMATION WILL BE APPLIED TO, WEATHER THE ANIMATION WILL RESET ITSELF IF CALLED AGAIN,THE STARTING FRAME, NAME OF THE ANIMATION A SMOOTH TRANSITION IS DONE WITH)
    public void PlayAnimation(string animation, GameObject toChange, bool overrideAnimation, int startAtFrame, string smoothTransitionWithAnimation = null) {
        //exits if an animation that is already running is called again                                 <-----------
        if ((animation == currentAnimation && !overrideAnimation) || (!overrideAnimation && animation == "BladeIdle" && (currentAnimation == "BladeBrake" || currentAnimation == "BladeBrakeToIdle" || currentAnimation == "BladeDashHorizontalBackwardsGrounded"))) {
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
                currentFrameLength = 1f / bladeIdle.frameRate;  //f is important for floatpoint division
                doesLoop = bladeIdle.loop;
                currentLoopFrame = bladeIdle.loopFrame;
                playAnimationAfterLastFrame = bladeIdle.playAnimationAfter;
                currentAnimationLength = bladeIdle.sprites.Length;
                for (int i = 0; i < bladeIdle.sprites.Length; i++) {
                    currentSprite[i] = bladeIdle.sprites[i];
                }
                break;
            case "BladeDashHorizontalBackwards":
                currentFrameLength = 1f / bladeDashBackwards.frameRate;
                doesLoop = bladeDashBackwards.loop;
                playAnimationAfterLastFrame = bladeDashBackwards.playAnimationAfter;
                currentLoopFrame = bladeDashBackwards.loopFrame;
                currentAnimationLength = bladeDashBackwards.sprites.Length;
                for (int i = 0; i < bladeDashBackwards.sprites.Length; i++) {
                    currentSprite[i] = bladeDashBackwards.sprites[i];
                }
                break;
            case "BladeDashHorizontalForward":
                currentFrameLength = 1f / bladeDashForwards.frameRate;
                doesLoop = bladeDashForwards.loop;
                playAnimationAfterLastFrame = bladeDashForwards.playAnimationAfter;
                currentLoopFrame = bladeDashForwards.loopFrame;
                currentAnimationLength = bladeDashForwards.sprites.Length;
                for (int i = 0; i < bladeDashForwards.sprites.Length; i++) {
                    currentSprite[i] = bladeDashForwards.sprites[i];
                }
                break;
            case "BladeDashHorizontalBackwardsGrounded":
                currentFrameLength = 1f / bladeDashBackwardsGrounded.frameRate;
                doesLoop = bladeDashBackwardsGrounded.loop;
                playAnimationAfterLastFrame = bladeDashBackwardsGrounded.playAnimationAfter;
                currentLoopFrame = bladeDashBackwardsGrounded.frameRate;
                currentAnimationLength = bladeDashBackwardsGrounded.sprites.Length;
                for (int i = 0; i < bladeDashBackwardsGrounded.sprites.Length; i++) {
                    currentSprite[i] = bladeDashBackwardsGrounded.sprites[i];
                }
                break;
            case "BladeDashHorizontalForwardGrounded":
                currentFrameLength = 1f / bladeDashForwardsGrounded.frameRate;
                doesLoop = bladeDashForwardsGrounded.loop;
                playAnimationAfterLastFrame = bladeDashForwardsGrounded.playAnimationAfter;
                currentLoopFrame = bladeDashForwardsGrounded.loopFrame;
                currentAnimationLength = bladeDashForwardsGrounded.sprites.Length;
                for (int i = 0; i < bladeDashForwardsGrounded.sprites.Length; i++) {
                    currentSprite[i] = bladeDashForwardsGrounded.sprites[i];
                }
                break;
            case "BladeDashUp":
                currentFrameLength = 1f / bladeDashUp.frameRate;
                doesLoop = bladeDashUp.loop;
                currentLoopFrame = bladeDashUp.loopFrame;
                playAnimationAfterLastFrame = bladeDashUp.playAnimationAfter;
                currentAnimationLength = bladeDashUp.sprites.Length;
                for (int i = 0; i < bladeDashUp.sprites.Length; i++) {
                    currentSprite[i] = bladeDashUp.sprites[i];
                }
                break;
            case "BladeDashDown":
                currentFrameLength = 1f / bladeDashDown.frameRate;
                doesLoop = bladeDashDown.loop;
                currentLoopFrame = bladeDashDown.loopFrame;
                playAnimationAfterLastFrame = bladeDashDown.playAnimationAfter;
                currentAnimationLength = bladeDashDown.sprites.Length;
                for (int i = 0; i < bladeDashDown.sprites.Length; i++) {
                    currentSprite[i] = bladeDashDown.sprites[i];
                }
                break;
            case "BladeFalling":
                currentFrameLength = 1f / bladeFalling.frameRate;
                doesLoop = bladeFalling.loop;
                currentLoopFrame = bladeFalling.loopFrame;
                playAnimationAfterLastFrame = bladeFalling.playAnimationAfter;
                currentAnimationLength = bladeFalling.sprites.Length;
                for (int i = 0; i < bladeFalling.sprites.Length; i++) {
                    currentSprite[i] = bladeFalling.sprites[i];
                }
                break;
            case "BladeJumping":
                currentFrameLength = 1f / bladeJumping.frameRate;
                doesLoop = bladeJumping.loop;
                currentLoopFrame = bladeJumping.loopFrame;
                playAnimationAfterLastFrame = bladeJumping.playAnimationAfter;
                currentAnimationLength = bladeJumping.sprites.Length;
                for (int i = 0; i < bladeJumping.sprites.Length; i++) {
                    currentSprite[i] = bladeJumping.sprites[i];
                }
                break;
            case "BladeLanding":
                currentFrameLength = 1f / bladeLanding.frameRate;
                doesLoop = bladeLanding.loop;
                currentLoopFrame = bladeLanding.loopFrame;
                playAnimationAfterLastFrame = bladeLanding.playAnimationAfter;
                currentAnimationLength = bladeLanding.sprites.Length;
                for (int i = 0; i < bladeLanding.sprites.Length; i++) {
                    currentSprite[i] = bladeLanding.sprites[i];
                }
                break;
            case "BladeRunning":
                currentFrameLength = 1f / bladeRunning.frameRate;
                doesLoop = bladeRunning.loop;
                currentLoopFrame = bladeRunning.loopFrame;
                playAnimationAfterLastFrame = false;
                currentAnimationLength = bladeRunning.sprites.Length;
                for (int i = 0; i < bladeRunning.sprites.Length; i++) {
                    currentSprite[i] = bladeRunning.sprites[i];
                }
                break;
            case "BladeAccel":
                currentFrameLength = 1f / bladeAccel.frameRate;
                doesLoop = bladeAccel.loop;
                currentLoopFrame = bladeAccel.loopFrame;
                playAnimationAfterLastFrame = false;
                currentAnimationLength = bladeAccel.sprites.Length;
                for (int i = 0; i < bladeAccel.sprites.Length; i++) {
                    currentSprite[i] = bladeAccel.sprites[i];
                }
                break;
            case "BladeBrake":
                currentFrameLength = 1f / bladeBrake.frameRate;
                doesLoop = bladeBrake.loop;
                currentLoopFrame = bladeBrake.loopFrame;
                playAnimationAfterLastFrame = true;
                currentAnimationLength = bladeBrake.sprites.Length;
                for (int i = 0; i < bladeBrake.sprites.Length; i++) {
                    currentSprite[i] = bladeBrake.sprites[i];
                }
                break;
            case "BladeSoftBrake":
                currentFrameLength = 1f / bladeSoftBrake.frameRate;
                doesLoop = bladeSoftBrake.loop;
                currentLoopFrame = bladeSoftBrake.loopFrame;
                playAnimationAfterLastFrame = false;
                currentAnimationLength = bladeSoftBrake.sprites.Length;
                for (int i = 0; i < bladeSoftBrake.sprites.Length; i++) {
                    currentSprite[i] = bladeSoftBrake.sprites[i];
                }
                break;
            case "BladeBrakeToIdle":
                currentFrameLength = 1f / bladeBrakeToIdle.frameRate;
                doesLoop = bladeBrakeToIdle.loop;
                currentLoopFrame = bladeBrakeToIdle.loopFrame;
                playAnimationAfterLastFrame = true;
                currentAnimationLength = bladeBrakeToIdle.sprites.Length;
                for (int i = 0; i < bladeBrakeToIdle.sprites.Length; i++) {
                    currentSprite[i] = bladeBrakeToIdle.sprites[i];
                }
                break;
            default:
                Debug.LogWarning("non-existent Animation String passed");
                break;
        }
        ApplySprite(currentSprite[currentSpriteFrame]);
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
                        case "BladeBrakeToIdle":
                            PlayAnimation("BladeIdle", currentObject, false, 3);
                            currentAnimation = null;    //
                            break;
                        case "BladeBrake":
                            PlayAnimation("BladeBrakeToIdle", currentObject, false, 1);
                            break;
                        case "BladeDashHorizontalBackwardsGrounded":
                            PlayAnimation("BladeIdle", currentObject, true, 3);
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
                ApplySprite(currentSprite[currentSpriteFrame]);
            }
        }
    }

    public void ApplySprite(Sprite sprite){
        currentSpriteRenderer.sprite = sprite;
            if(this.GetComponent<Player>().facingDirection == 1){
                currentSpriteRenderer.flipX = false;
            }
            else{
                currentSpriteRenderer.flipX = true;
            }
    }



}
