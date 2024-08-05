using UnityEngine;
using UnityEngine.InputSystem;

namespace GorillaTycoon
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Ins;
        public bool leftGrip;
        public bool rightGrip;
        public bool leftSecondaryButton;
        public bool rightSecondaryButton;
        public float leftSecondaryBtnLast;
        public float rightSecondaryBtnLast;
        public bool leftSecondaryBtnDouble;
        public bool rightSecondaryBtnDouble;
        
        public void Start()
        {
            Ins = this;
        }
        
        private void Update()
        {
            leftGrip = ControllerInputPoller.instance.leftGrab;
            rightGrip = ControllerInputPoller.instance.rightGrab;
            
            if (leftSecondaryBtnDouble && !ControllerInputPoller.instance.leftControllerSecondaryButton)
                leftSecondaryBtnDouble = false;

            if (rightSecondaryBtnDouble && !ControllerInputPoller.instance.rightControllerSecondaryButton)
                rightSecondaryBtnDouble = false;
            
            if (!leftSecondaryButton && ControllerInputPoller.instance.leftControllerSecondaryButton)
            {
                if (Time.time - leftSecondaryBtnLast <= 0.5f)
                    leftSecondaryBtnDouble = true;
                leftSecondaryButton = true;
                leftSecondaryBtnLast = Time.time;
            }
            if (!rightSecondaryButton && ControllerInputPoller.instance.rightControllerSecondaryButton)
            {
                if (Time.time - rightSecondaryBtnLast <= 0.5f)
                    rightSecondaryBtnDouble = true;
                rightSecondaryButton = true;
                rightSecondaryBtnLast = Time.time;
            }

            if (leftSecondaryButton && !ControllerInputPoller.instance.leftControllerSecondaryButton)
                leftSecondaryButton = false;

            if (rightSecondaryButton && !ControllerInputPoller.instance.rightControllerSecondaryButton)
                rightSecondaryButton = false;
        }
    }
}
