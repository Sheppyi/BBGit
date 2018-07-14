using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(GameSettings))]
public class GetInput : MonoBehaviour {


    //initialization

    //4 main buttons
    public static bool button_A;
    public static bool button_B;
    public static bool button_X;
    public static bool button_Y;

    public static bool button_A_pressed;
    public static bool button_B_pressed;
    public static bool button_X_pressed;
    public static bool button_Y_pressed;

    public static bool button_A_released;
    public static bool button_B_released;
    public static bool button_X_released;
    public static bool button_Y_released;

    //Left stick
    public static float axis_left_horizontal;
    public static float axis_left_vertical;

    public static bool axis_left_right;
    public static bool axis_left_left;
    public static bool axis_left_up;
    public static bool axis_left_down;

    public static bool axis_left_right_pressed;
    public static bool axis_left_left_pressed;
    public static bool axis_left_up_pressed;
    public static bool axis_left_down_pressed;  

    public static bool axis_left_right_released;
    public static bool axis_left_left_released;
    public static bool axis_left_up_released;
    public static bool axis_left_down_released;

    //right stick
    public static float axis_right_horizontal;
    public static float axis_right_vertical;

    public static bool axis_right_right;
    public static bool axis_right_left;
    public static bool axis_right_up;
    public static bool axis_right_down;

    public static bool axis_right_right_pressed;
    public static bool axis_right_left_pressed;
    public static bool axis_right_up_pressed;
    public static bool axis_right_down_pressed;

    public static bool axis_right_right_released;
    public static bool axis_right_left_released;
    public static bool axis_right_up_released;
    public static bool axis_right_down_released;

    //Dpad
    public static bool dpad_right;
    public static bool dpad_left;
    public static bool dpad_up;
    public static bool dpad_down;
    public static bool dpad_right_pressed;
    public static bool dpad_left_pressed;
    public static bool dpad_up_pressed;
    public static bool dpad_down_pressed;
    public static bool dpad_right_released;
    public static bool dpad_left_released;
    public static bool dpad_up_released;
    public static bool dpad_down_released;



    //trigger
    public static float axis_trigger_right;
    public static float axis_trigger_left;

    public static bool trigger_right;
    public static bool trigger_left;

    public static bool trigger_right_pressed;
    public static bool trigger_left_pressed;

    public static bool trigger_right_released;
    public static bool trigger_left_released;

    //bumper
    public static bool bumper_right;
    public static bool bumper_left;

    public static bool bumper_right_pressed;
    public static bool bumper_left_pressed;

    public static bool bumper_right_released;
    public static bool bumper_left_released;


    void Update() {

        //XBOX controller

        //4 main buttons
        button_A = Input.GetButton("A_button");
        button_B = Input.GetButton("B_button");
        button_X = Input.GetButton("X_button");
        button_Y = Input.GetButton("Y_button");

        button_A_pressed = Input.GetButtonDown("A_button");
        button_B_pressed = Input.GetButtonDown("B_button");
        button_X_pressed = Input.GetButtonDown("X_button");
        button_Y_pressed = Input.GetButtonDown("Y_button");

        button_A_released = Input.GetButtonUp("A_button");
        button_B_released = Input.GetButtonUp("B_button");
        button_X_released = Input.GetButtonUp("X_button");
        button_Y_released = Input.GetButtonUp("Y_button");

        //AXIS LEFT JOYSTICK
        axis_left_horizontal = Input.GetAxis("LeftJoystickHorizontal");
        axis_left_vertical = Input.GetAxis("LeftJoystickVertical");
        //Right
        axis_left_right_pressed = false;
        axis_left_right_released = false;
        if (axis_left_horizontal >= GameSettings.axisLeftHorizontalDeadzone) {
            if (axis_left_right == false) {
                axis_left_right_pressed = true;
            }
            axis_left_right = true;

        }
        else {
            if (axis_left_right == true) {
                axis_left_right_released = true;
            }
            axis_left_right = false;
        }
        //left
        axis_left_left_pressed = false;
        axis_left_left_released = false;
        if (axis_left_horizontal <= -GameSettings.axisLeftHorizontalDeadzone) {
            if (axis_left_left == false) {
                axis_left_left_pressed = true;
            }
            axis_left_left = true;

        }
        else {
            if (axis_left_left == true) {
                axis_left_left_released = true;
            }
            axis_left_left = false;
        }
        //down
        axis_left_down_pressed = false;
        axis_left_down_released = false;
        if (axis_left_vertical >= GameSettings.axisLeftVerticalDeadzone) {
            if (axis_left_down == false) {
                axis_left_down_pressed = true;
            }
            axis_left_down = true;

        }
        else {
            if (axis_left_down == true) {
                axis_left_down_released = true;
            }
            axis_left_down = false;
        }
        //up
        axis_left_up_pressed = false;
        axis_left_up_released = false;
        if (axis_left_vertical <= -GameSettings.axisLeftVerticalDeadzone) {
            if (axis_left_up == false) {
                axis_left_up_pressed = true;
            }
            axis_left_up = true;

        }
        else {
            if (axis_left_up == true) {
                axis_left_up_released = true;
            }
            axis_left_up = false;
        }

        //AXIS RIGHT JOYSTICK
        axis_right_horizontal = Input.GetAxis("RightJoystickHorizontal");
        axis_right_vertical = Input.GetAxis("RightJoystickVertical");
        //Right
        axis_right_right_pressed = false;
        axis_right_right_released = false;
        if (axis_right_horizontal >= GameSettings.axisRightHorizontalDeadzone) {
            if (axis_right_right == false) {
                axis_right_right_pressed = true;
            }
            axis_right_right = true;

        }
        else {
            if (axis_right_right == true) {
                axis_right_right_released = true;
            }
            axis_right_right = false;
        }
        //left
        axis_right_left_pressed = false;
        axis_right_left_released = false;
        if (axis_right_horizontal <= -GameSettings.axisRightHorizontalDeadzone) {
            if (axis_right_left == false) {
                axis_right_left_pressed = true;
            }
            axis_right_left = true;

        }
        else {
            if (axis_right_left == true) {
                axis_right_left_released = true;
            }
            axis_right_left = false;
        }
        //down
        axis_right_down_pressed = false;
        axis_right_down_released = false;
        if (axis_right_vertical >= GameSettings.axisRightVerticalDeadzone) {
            if (axis_right_down == false) {
                axis_right_down_pressed = true;
            }
            axis_right_down = true;

        }
        else {
            if (axis_right_down == true) {
                axis_right_down_released = true;
            }
            axis_right_down = false;
        }
        //up
        axis_right_up_pressed = false;
        axis_right_up_released = false;
        if (axis_right_vertical <= -GameSettings.axisRightVerticalDeadzone) {
            if (axis_right_up == false) {
                axis_right_up_pressed = true;
            }
            axis_right_up = true;

        }
        else {
            if (axis_right_up == true) {
                axis_right_up_released = true;
            }
            axis_right_up = false;
        }

        //AXIS TRIGGER
        axis_trigger_left = Input.GetAxis("TriggerLeft");
        axis_trigger_right = Input.GetAxis("TriggerRight");
        //Right
        trigger_right_pressed = false;
        trigger_right_released = false;
        if (axis_trigger_right >= GameSettings.TriggerRightDeadzone) {
            if (trigger_right == false) {
                trigger_right_pressed = true;
            }
            trigger_right = true;
        }
        else {
            if (trigger_right == true) {
                trigger_right_released = true;
            }
            trigger_right = false;
        }
        //Left
        trigger_left_pressed = false;
        trigger_left_released = false;
        if (axis_trigger_left >= GameSettings.TriggerLeftDeadzone) {
            if (trigger_left == false) {
                trigger_left_pressed = true;
            }
            trigger_left = true;
        }
        else {
            if (trigger_left == true) {
                trigger_left_released = true;
            }
            trigger_left = false;
        }

        //BUMPER TRIGGER
        bumper_right = Input.GetButton("BumperRight");
        bumper_left = Input.GetButton("BumperLeft");

        bumper_right_pressed = Input.GetButtonDown("BumperRight");
        bumper_left_pressed = Input.GetButtonDown("BumperLeft");

        bumper_right_released = Input.GetButtonUp("BumperRight");
        bumper_left_released = Input.GetButtonUp("BumperLeft");


        //DPAD
        var a = Input.GetAxis("dpad_vertical");
        if (a > 0.1) {
            if (!dpad_up) { dpad_up_pressed = true; } else { dpad_up_pressed = false; }
            dpad_up = true;
            if (dpad_down) { dpad_down_released = true; } else { dpad_down_released = false; }
            dpad_down = false;
        }
        else if (a == -1) {
            if (!dpad_down) { dpad_down_pressed = true; } else { dpad_down_pressed = false; }
            dpad_down = true;
            if (dpad_up) { dpad_up_released = true; } else { dpad_up_released = false; }
            dpad_up = false;
        }
        else {
            if (dpad_up) { dpad_up_released = true; } else { dpad_up_released = false; }
            if (dpad_down) { dpad_down_released = true; } else { dpad_down_released = false; }
            dpad_up = false;
            dpad_down = false;
        }

        var b = Input.GetAxis("dpad_horizontal"); 
        if (b > 0.1) {
            if (!dpad_right) { dpad_right_pressed = true; } else { dpad_right_pressed = false; }
            dpad_right = true;
            if (dpad_left) { dpad_left_released = true; } else { dpad_left_released = false; }
            dpad_left = false;
        }
        else if (b == -1) {
            if (!dpad_left) { dpad_left_pressed = true; } else { dpad_left_pressed = false; }
            dpad_left = true;
            if(dpad_right) { dpad_right_released = true; } else { dpad_right_released = false; }
            dpad_right = false;
        }
        else {
            if (dpad_right) { dpad_right_released = true; } else { dpad_right_released = false; }
            if (dpad_left) { dpad_left_released = true; } else { dpad_left_released = false; }
            dpad_right = false;
            dpad_left = false;
        }




        //███████████████████████████████████████████████████████████████████████████████████████████████
        //DEBUG

        /*

        //main buttons
        if (button_A_pressed == true) {
            Debug.Log("button_A_pressed");
        }
        if (button_B_pressed == true) {
            Debug.Log("button_B_pressed");
        }
        if (button_X_pressed == true) {
            Debug.Log("button_X_pressed");
        }
        if (button_Y_pressed == true) {
            Debug.Log("button_Y_pressed");
        }
        if (button_A_released == true) {
            Debug.Log("button_A_released");
        }
        if (button_B_released == true) {
            Debug.Log("button_B_released");
        }
        if (button_X_released == true) {
            Debug.Log("button_X_released");
        }
        if (button_Y_released == true) {
            Debug.Log("button_Y_released");
        }

        //axis left
        if (axis_left_right_released == true) {
            Debug.Log("axis_left_right_released");
        }
        if (axis_left_right_pressed == true) {
            Debug.Log("axis_left_right_pressed");
        }
        if (axis_left_up_released == true) {
            Debug.Log("axis_left_up_released");
        }
        if (axis_left_up_pressed == true) {
            Debug.Log("axis_left_up_pressed");
        }
        if (axis_left_down_released == true) {
            Debug.Log("axis_left_down_released");
        }
        if (axis_left_down_pressed == true) {
            Debug.Log("axis_left_down_pressed");
        }
        if (axis_left_left_released == true) {
            Debug.Log("axis_left_left_released ");
        }
        if (axis_left_left_pressed == true) {
            Debug.Log("axis_left_left_pressed");
        }

        //axis right
        if (axis_right_right_released == true) {
            Debug.Log("axis_right_right_released");
        }
        if (axis_right_right_pressed == true) {
            Debug.Log("axis_right_right_pressed");
        }
        if (axis_right_up_released == true) {
            Debug.Log("axis_right_up_released");
        }
        if (axis_right_up_pressed == true) {
            Debug.Log("axis_right_up_pressed");
        }
        if (axis_right_down_released == true) {
            Debug.Log("axis_right_down_released");
        }
        if (axis_right_down_pressed == true) {
            Debug.Log("axis_right_down_pressed");
        }
        if (axis_right_left_released == true) {
            Debug.Log("axis_right_left_released ");
        }
        if (axis_right_left_pressed == true) {
            Debug.Log("axis_right_left_pressed");
        }

        //trigger
        if (trigger_right_pressed == true) {
            Debug.Log("trigger_right_pressed");
        }
        if (trigger_right_released == true) {
            Debug.Log("trigger_right_released");
        }
        if (trigger_left_pressed == true) {
            Debug.Log("trigger_left_pressed");
        }
        if (trigger_left_released == true) {
            Debug.Log("trigger_left_released");
        }

        //bumper
        if (bumper_right_pressed == true) {
            Debug.Log("bumper_right_pressed");
        }
        if (bumper_left_pressed == true) {
            Debug.Log("bumper_left_pressed");
        }
        if (bumper_right_released == true) {
            Debug.Log("bumper_right_released");
        }
        if (bumper_left_released == true) {
            Debug.Log("bumper_left_released");
        }



        //dpad
        if (dpad_up_pressed) {
            Debug.Log("dpad_up_pressed");
        }
        if (dpad_down_pressed) {
            Debug.Log("dpad_down_pressed");
        }
        if (dpad_right_pressed) {
            Debug.Log("dpad_right_pressed");
        }
        if (dpad_left_pressed) {
            Debug.Log("dpad_left_pressed");
        }

        if (dpad_up_released) {
            Debug.Log("dpad_up_released");
        }
        if (dpad_down_released) {
            Debug.Log("dpad_down_released");
        }
        if (dpad_right_released) {
            Debug.Log("dpad_right_released");
        }
        if (dpad_left_released) {
            Debug.Log("dpad_left_released");
        }


        
        */

    }
}

