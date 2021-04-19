using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Converters;
using SharpDX.DirectInput;

namespace PenguinNav.Classes
{
    class MyController
    {
        DirectInput directInput = new DirectInput();
        public Guid JoystickGuid = Guid.Empty;
        Joystick joystick;

        //Controller variables
        public int LxAxis, LyAxis, RxAxis, RyAxis;
        public bool[] Buttons = new bool[12];
        public bool[] Keys = new bool[4];



        /// <summary>
        /// Find a gamepad/joystick connected to the system
        /// </summary>
        public void ScanDevices()
        {
            //Look for a gamepad
            foreach(var deviceInstance in directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
            {
                JoystickGuid = deviceInstance.InstanceGuid;
            }

            //Look for a joystick if guid still empty
            if (JoystickGuid == Guid.Empty)
            {
                foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                {
                    JoystickGuid = deviceInstance.InstanceGuid;
                }

            }
        }

        /// <summary>
        /// Configure device 
        /// </summary>
        public void SetupDevice()
        {    
            joystick = new Joystick(directInput, JoystickGuid);
            joystick.Acquire();
        }


        /// <summary>
        /// Returns the current state of the controller
        /// </summary>
        public void GetControllerState()
        {
            JoystickState joystickState = joystick.GetCurrentState();
            //From here on, assign joystickstate to out values. I'm tired. Use consoleapp to determine which is which
            LxAxis = DeadZone(joystickState.X);
            LyAxis = DeadZone(joystickState.Y);
            RxAxis = DeadZone(joystickState.Z);
            RyAxis = DeadZone(joystickState.RotationZ);
            Buttons = joystickState.Buttons;
            Keys = ArrowKeys(joystickState);
        }

        /// <summary>
        /// Simple deadzone function. Compare value within range to filter jitter
        /// </summary>
        /// <param name="Axis"></param>
        /// <returns></returns>
        private int DeadZone (int Axis)
        {
            //In case of jitter edit this
            int deadzone = 350;

            //Expected value when stick is 0
            int center = 32767;

            if (Axis < center + deadzone && Axis > center - deadzone)
            {
                Axis = center;
            }

            return Axis;
        }


        /// <summary>
        /// Read joystickState and return arrow keys value as bool[]
        /// </summary>
        /// <param name="joystickState"></param>
        /// <returns></returns>
        private bool[] ArrowKeys(JoystickState joystickState)
        {
            int[] pov = joystickState.PointOfViewControllers;

            //Up, Right, Down, Left
            bool[] Keys = { false, false, false, false };

            for (int i=0; i<pov.Length; i++)
            {
                if (pov[i] != -1)
                {
                    Keys[i] = true;
                }
            }

            return Keys;
        }
    }
}
