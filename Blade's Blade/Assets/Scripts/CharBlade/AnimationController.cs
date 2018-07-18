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
    bool flipX = false;

    public Sprite[] BladeIdle = new Sprite[11];
    int BladeIdleFrameRate =10;
    bool BladeIdleLoop = true;

    public Sprite[] BladeIdleLeft = new Sprite[11];
    int BladeIdleLeftFrameRate = 10;
    bool BladeIdleLeftLoop = true;

    public Sprite[] BladeDashHorizontalBackwards = new Sprite[5];
    int BladeDashHorizontalBackwardsFrameRate = 14;
    bool BladeDashHorizontalBackwardsLoop = false;

    public void PlayAnimation(string animation, GameObject toChange, bool overrideAnimation) {
        if (animation == currentAnimation && !overrideAnimation) {
            return;
        }
        currentSpriteRenderer = toChange.GetComponent<SpriteRenderer>();
        currentSpriteFrame = 0;
        currentAnimationLength = 0;
        currentFrameLength = 0;
        switch (animation) { 
            case "BladeIdle":
                currentAnimation = animation;
                currentFrameLength = 1f / BladeIdleFrameRate;  //f is important for floatpoint division
                currentFrameLengthT = 0;
                doesLoop = BladeIdleLoop;
                for (int i = 0; i < BladeIdle.Length; i++) {
                    currentSprite[i] = BladeIdle[i];
                    currentAnimationLength++;
                }
                break;
            case "BladeDashHorizontalBackwards":
                currentAnimation = animation;
                currentFrameLength = 1f / BladeDashHorizontalBackwardsFrameRate;
                currentFrameLengthT = 0;
                doesLoop = BladeDashHorizontalBackwardsLoop;
                for (int i = 0; i < BladeDashHorizontalBackwards.Length; i++) {
                    currentSprite[i] = BladeDashHorizontalBackwards[i];
                    currentAnimationLength++;
                }
                break;
            default:
                Debug.LogWarning("non.existent Animation String passed");
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
                if (currentSpriteFrame >= currentAnimationLength && doesLoop) {
                    currentSpriteFrame = 0;
                }
                if (currentSpriteFrame >= currentAnimationLength && !doesLoop) {
                    currentSpriteFrame = currentAnimationLength - 1;
                }
                //apply frame
                applySprite(currentSprite[currentSpriteFrame]);
            }
        }
    }

        public void applySprite(Sprite sprite){
            currentSpriteRenderer.sprite = sprite;
                if(this.GetComponent<Player>().facingDirection == 1){
                    flipX = false;
                    currentSpriteRenderer.flipX = false;
                }
                else{
                    flipX = true;
                    currentSpriteRenderer.flipX = true;
                }
        }



}
