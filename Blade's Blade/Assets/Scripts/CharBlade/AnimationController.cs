using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    SpriteRenderer currentSpriteRenderer;
    int currentSpriteFrame = 0;
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
    bool bladeLandingLoop = false;
    int bladeLandingLoopFrame = 1;

    public void PlayAnimation(string animation, GameObject toChange, bool overrideAnimation, int startAtFrame) {
        if (animation == currentAnimation && !overrideAnimation) {
            return;
        }
        currentObject = toChange;
        currentAnimation = animation;
        currentLoopFrame = 0;
        currentSpriteRenderer = toChange.GetComponent<SpriteRenderer>();
        currentSpriteFrame = startAtFrame;
        currentAnimationLength = 0;
        currentFrameLength = 0;
        switch (animation) {
            case "BladeIdle":
                currentFrameLength = 1f / BladeIdleFrameRate;  //f is important for floatpoint division
                currentFrameLengthT = 0;
                doesLoop = BladeIdleLoop;
                currentLoopFrame = BladeIdleLoopFrame;
                playAnimationAfterLastFrame = false;
                for (int i = 0; i < BladeIdle.Length; i++) {
                    currentSprite[i] = BladeIdle[i];
                    currentAnimationLength++;
                }
                break;
            case "BladeDashHorizontalBackwards":
                currentFrameLength = 1f / BladeDashHorizontalBackwardsFrameRate;
                currentFrameLengthT = 0;
                doesLoop = BladeDashHorizontalBackwardsLoop;
                playAnimationAfterLastFrame = false;
                currentLoopFrame = BladeDashHorizontalBackwardsLoopFrame;
                for (int i = 0; i < BladeDashHorizontalBackwards.Length; i++) {
                    currentSprite[i] = BladeDashHorizontalBackwards[i];
                    currentAnimationLength++;
                }
                break;
            case "BladeFalling":
                currentFrameLength = 1f / bladeFallingFrameRate;
                currentFrameLengthT = 0;
                doesLoop = bladeFallingLoop;
                currentLoopFrame = bladeFallingLoopFrame;
                playAnimationAfterLastFrame = false;
                for (int i = 0; i < bladeFalling.Length; i++) {
                    currentSprite[i] = bladeFalling[i];
                    currentAnimationLength++;
                }
                break;
            case "BladeJumping":
                currentFrameLength = 1f / bladeJumpingFrameRate;
                currentFrameLengthT = 0;
                doesLoop = bladeJumpingLoop;
                currentLoopFrame = bladeJumpingLoopFrame;
                playAnimationAfterLastFrame = false;
                for (int i = 0; i < bladeJumping.Length; i++) {
                    currentSprite[i] = bladeJumping[i];
                    currentAnimationLength++;
                }
                break;
            case "BladeLanding":
                currentFrameLength = 1f / bladeLandingFrameRate;
                currentFrameLengthT = 0;
                doesLoop = bladeLandingLoop;
                currentLoopFrame = bladeLandingLoopFrame;
                playAnimationAfterLastFrame = true;
                for (int i = 0; i < bladeLanding.Length; i++) {
                    currentSprite[i] = bladeLanding[i];
                    currentAnimationLength++;
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
                            PlayAnimation("BladeIdle",currentObject,false,3);
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
