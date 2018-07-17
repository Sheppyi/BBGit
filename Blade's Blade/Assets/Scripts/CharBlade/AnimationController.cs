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

    public Sprite[] BladeIdleRight = new Sprite[11];
    int BladeIdleRightFrameRate =10;
    bool BladeIdleRightLoop = true;

    public Sprite[] BladeIdleLeft = new Sprite[11];
    int BladeIdleLeftFrameRate = 10;
    bool BladeIdleLeftLoop = true;

    public Sprite[] BladeDashLeftBackWards = new Sprite[5];
    int BladeDashLeftBackWardsFrameRate = 14;
    bool BladeDashLeftBackWardsLoop = false;

    public Sprite[] BladeDashRightBackWards = new Sprite[5];
    int BladeDashRightBackWardsFrameRate = 14;
    bool BladeDashRightBackWardsLoop = false;

    public void PlayAnimation(string animation, GameObject toChange, bool overrideAnimation) {
        if (animation == currentAnimation && !overrideAnimation) {
            return;
        }
        currentSpriteRenderer = toChange.GetComponent<SpriteRenderer>();
        currentSpriteFrame = 0;
        currentAnimationLength = 0;
        currentFrameLength = 0;
        bool Error = true;
        switch (animation) { 
            case "BladeIdleRight":
                currentAnimation = animation;
                currentFrameLength = 1f / BladeIdleRightFrameRate;  //f is important for floatpoint division
                currentFrameLengthT = 0;
                doesLoop = BladeIdleRightLoop;
                Error = false;
                for (int i = 0; i < BladeIdleRight.Length; i++) {
                    currentSprite[i] = BladeIdleRight[i];
                    currentAnimationLength++;
                }
                break;
            case "BladeIdleLeft":
                currentAnimation = animation;
                currentFrameLength = 1f / BladeIdleLeftFrameRate;  //f is important for floatpoint division
                currentFrameLengthT = 0;
                doesLoop = BladeIdleLeftLoop;
                Error = false;
                for (int i = 0; i < BladeIdleLeft.Length; i++) {
                    currentSprite[i] = BladeIdleLeft[i];
                    currentAnimationLength++;
                }
                break;
            case "BladeDashLeftBackWards":
                currentAnimation = animation;
                currentFrameLength = 1f / BladeDashLeftBackWardsFrameRate;  
                currentFrameLengthT = 0;
                doesLoop = BladeDashLeftBackWardsLoop;
                Error = false;
                for (int i = 0; i < BladeDashLeftBackWards.Length; i++) {
                    currentSprite[i] = BladeDashLeftBackWards[i];
                    currentAnimationLength++;
                }
                break;
            case "BladeDashRightBackWards":
                currentAnimation = animation;
                currentFrameLength = 1f / BladeDashRightBackWardsFrameRate;
                currentFrameLengthT = 0;
                doesLoop = BladeDashRightBackWardsLoop;
                Error = false;
                for (int i = 0; i < BladeDashRightBackWards.Length; i++) {
                    currentSprite[i] = BladeDashRightBackWards[i];
                    currentAnimationLength++;
                }
                break;
        }
        currentSpriteRenderer.sprite = currentSprite[currentSpriteFrame];
        if (Error) {
            Debug.LogWarning("non.existent Animation String passed");
        }
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
                currentSpriteRenderer.sprite = currentSprite[currentSpriteFrame];
            }
        }

    }

}
