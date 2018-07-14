using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    GameObject currentObject;
    int currentSpriteFrame = 0;
    Sprite[] currentSprite = new Sprite[50];    //kann so groß sein wie will nur nicht zu klein
    int currentAnimationLength;
    float currentFrameLength;
    float currentFrameLengthT;
    string CurrentAnimation = "null";

    public  Sprite[] BladeIdle = new Sprite[6];
    int BladeIdleFrameRate = 8;



    public void PlayAnimation(string animation, GameObject toChange) {
        if (animation == CurrentAnimation) {
            return;
        }
        currentObject = toChange;
        currentSpriteFrame = 0;
        currentAnimationLength = 0;
        currentFrameLength = 0;
        switch (animation) {
            case "BladeIdle":
                CurrentAnimation = "BladeIdle";
                currentFrameLength = 1 / BladeIdleFrameRate;
                Debug.Log(currentFrameLength);      //why is this 0
                currentFrameLengthT = 0;
                for (int i = 0; i < BladeIdle.Length; i++) {
                    currentSprite[i] = BladeIdle[i];
                    currentAnimationLength++;
                }
                break;
        }
    }


    public void Update() {

        if (currentObject != null) {
            currentFrameLengthT += Time.deltaTime;
            if (currentFrameLengthT >= currentFrameLength) {
                currentFrameLengthT = 0;
                currentSpriteFrame++;
                if (currentSpriteFrame >= currentAnimationLength) {
                    currentSpriteFrame = 0;
                }
                currentObject.GetComponent<SpriteRenderer>().sprite = currentSprite[currentSpriteFrame];
            }
        }

    }

}
