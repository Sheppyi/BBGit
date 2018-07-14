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

    public  Sprite[] BladeIdle = new Sprite[6];
    int BladeIdleFrameRate = 8;
    bool BladeIdleLoop = true;

    public Sprite[] BladeDashLeft = new Sprite[6];
    int BladeDashLeftFrameRate = 12;
    bool BladeDashLeftLoop = false;

    public Sprite[] BladeDashRight = new Sprite[6];
    int BladeDashRightFrameRate = 12;
    bool BladeDashRightLoop = false;




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
            case "BladeDashLeft":
                currentAnimation = animation;
                currentFrameLength = 1f / BladeDashLeftFrameRate;  
                currentFrameLengthT = 0;
                doesLoop = BladeDashLeftLoop;
                for (int i = 0; i < BladeDashLeft.Length; i++) {
                    currentSprite[i] = BladeDashLeft[i];
                    currentAnimationLength++;
                }
                break;
            case "BladeDashRight":
                currentAnimation = animation;
                currentFrameLength = 1f / BladeDashRightFrameRate;
                currentFrameLengthT = 0;
                doesLoop = BladeDashRightLoop;
                for (int i = 0; i < BladeDashRight.Length; i++) {
                    currentSprite[i] = BladeDashRight[i];
                    currentAnimationLength++;
                }
                break;
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
