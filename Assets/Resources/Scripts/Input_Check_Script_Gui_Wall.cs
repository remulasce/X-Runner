using UnityEngine;
using System.Collections;

public class Input_Check_Script_Gui_Wall : MonoBehaviour {

    public Texture keyboardTexture;
    public Texture controllerTexture;

    GUITexture guiTexture;

    void Start()
    {
        guiTexture = this.gameObject.GetComponent<GUITexture>();
        if (Input.GetJoystickNames().Length > 0)
        {
            if (guiTexture)
            {
                guiTexture.texture = controllerTexture;
                m_State = eInputState.Controller;
            }
            else
            {
                this.renderer.material.mainTexture = controllerTexture;
                m_State = eInputState.Controller;
            }
        }
    }

    // Credits: http://answers.unity3d.com/questions/131899/how-do-i-check-what-input-device-is-currently-beei.html
    // There is no way I would have had the time to do this myself, but I needed to get this code running fast.  Give credit for bulit - in stuff for when it is due, right?

    //*********************//
    // Public member data //
    //*********************//


    //*********************//
    // Private member data //
    //*********************//

    public enum eInputState
    {
        MouseKeyboard,
        Controller
    };
    private eInputState m_State = eInputState.MouseKeyboard;

    //*************************//
    // Unity member methods //
    //*************************//

    void OnGUI()
    {
        switch (m_State)
        {
            case eInputState.MouseKeyboard:
                if (isControllerInput())
                {
                    m_State = eInputState.Controller;
                    Debug.Log("JoyStick being used");
                    if (controllerTexture)
                    {
                        if (guiTexture)
                        {
                            guiTexture.texture = controllerTexture;
                        }
                        else
                        {
                            this.renderer.material.mainTexture = controllerTexture;
                        }

                    }
                }
                break;
            case eInputState.Controller:
                if (isMouseKeyboard())
                {
                    m_State = eInputState.MouseKeyboard;
                    Debug.Log("Mouse & Keyboard being used");
                    if (keyboardTexture)
                    {
                        if (guiTexture)
                        {
                            guiTexture.texture = keyboardTexture;
                        }
                        else
                        {
                            this.renderer.material.mainTexture = keyboardTexture;
                        }
                    }
                }
                break;
        }
    }

    //***************************//
    // Public member methods //
    //***************************//

    public eInputState GetInputState()
    {
        return m_State;
    }

    //****************************//
    // Private member methods //
    //****************************//

    private bool isMouseKeyboard()
    {
        // mouse & keyboard buttons
        if (Event.current.isKey ||
        Event.current.isMouse)
        {
            return true;
        }
        // mouse movement
        if (Input.GetAxis("Mouse X") != 0.0f ||
        Input.GetAxis("Mouse Y") != 0.0f)
        {
            return true;
        }
        return false;
    }

    private bool isControllerInput()
    {
        // joystick buttons
        if (Input.GetKey(KeyCode.Joystick1Button0) ||
        Input.GetKey(KeyCode.Joystick1Button1) ||
        Input.GetKey(KeyCode.Joystick1Button2) ||
        Input.GetKey(KeyCode.Joystick1Button3) ||
        Input.GetKey(KeyCode.Joystick1Button4) ||
        Input.GetKey(KeyCode.Joystick1Button5) ||
        Input.GetKey(KeyCode.Joystick1Button6) ||
        Input.GetKey(KeyCode.Joystick1Button7) ||
        Input.GetKey(KeyCode.Joystick1Button8) ||
        Input.GetKey(KeyCode.Joystick1Button9) ||
        Input.GetKey(KeyCode.Joystick1Button10) ||
        Input.GetKey(KeyCode.Joystick1Button11) ||
        Input.GetKey(KeyCode.Joystick1Button12) ||
        Input.GetKey(KeyCode.Joystick1Button13) ||
        Input.GetKey(KeyCode.Joystick1Button14) ||
        Input.GetKey(KeyCode.Joystick1Button15) ||
        Input.GetKey(KeyCode.Joystick1Button16) ||
        Input.GetKey(KeyCode.Joystick1Button17) ||
        Input.GetKey(KeyCode.Joystick1Button18) ||
        Input.GetKey(KeyCode.Joystick1Button19))
        {
            return true;
        }

        return false;
    }
}
