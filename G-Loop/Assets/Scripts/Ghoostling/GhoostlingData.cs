using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GhoostlingData {

    // Note: doesn't account mouse inputs -- camera is just set according to
    // eulerAngles and cameraPitch.
    public struct UserInputs {
        public float horizontal;  // result of Input.GetAxis("Horizontal")
        public float vertical;  // result of Input.GetAxis("Vertical")
        public float mouseX;
        public float mouseY;
        public bool jumpButtonDown;
        public bool jumpButtonUp;
        public bool crouchButtonDown;
        public bool crouchButtonUp;
        // public bool honkButtonDown;
        // public bool honkButtonUp;
        public static int READ_USER_INPUTS = -1;
        public UserInputs(int meow) {  // Need a signature different from UserInputs()
                                       // Would recommend passing READ_USER_INPUTS for clarity :)
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
            jumpButtonDown = Input.GetButtonDown("Jump");
            jumpButtonUp = Input.GetButtonUp("Jump");
            crouchButtonDown = Input.GetButtonDown("Crouch");
            crouchButtonUp = Input.GetButtonUp("Crouch");
            // honkButtonDown = Input.GetButtonDown("Honk");
            // honkButtonUp = Input.GetButtonUp("Honk");
        }
    }
    public struct ShotFired {
        public Vector3 origin;
        public Vector3 direction_normalized;
        public float speed;
    }
    public struct ItemInteraction {
        // TODO:
        //public Item pickUp;
        //Item select;
    }
    public struct Death {
        public enum Cause {
            EXTERNAL,
            SUICIDE,
        };
        public Cause cause;
    }
    public struct NonBreakZone {
        public bool ignoreAxisY;
    }
    public struct Frame {
        public int tick;
        public UserInputs inputs;
        public Vector3 position;
        public Vector3 eulerAngles;
        public ShotFired? shotFired;
        public ItemInteraction? itemInteraction;
        public NonBreakZone? nonBreakZone;
    }
    private List<Frame> frames = new List<Frame>();

    public void AddFrame(Frame f) {
        frames.Add(f);
    }

    public Frame GetFrame(int tick) {
        return frames[tick];
    }

    public int GetFrameCount() {
        return frames.Count;
    }
}
