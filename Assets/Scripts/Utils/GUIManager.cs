using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public string textToDisplay = "";

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.Instance.AddListener<OnSquareButton>(UpdateTextSquare);
        EventsManager.Instance.AddListener<OnTriangleButton>(UpdateTextTriangle);
        EventsManager.Instance.AddListener<OnRoundButton>(UpdateTextRound);
        EventsManager.Instance.AddListener<OnCrossButton>(UpdateTextCross);

        EventsManager.Instance.AddListener<ONR1Button>(UpdateTextR1);
        EventsManager.Instance.AddListener<ONR2Button>(UpdateTextR2);
        EventsManager.Instance.AddListener<ONL1Button>(UpdateTextL1);
        EventsManager.Instance.AddListener<ONL2Button>(UpdateTextL2);

        EventsManager.Instance.AddListener<OnMenuButton>(UpdateTextMenu);
        EventsManager.Instance.AddListener<OnShareButton>(UpdateTextShare);
        EventsManager.Instance.AddListener<OnOptionsButton>(UpdateTextOptions);

        EventsManager.Instance.AddListener<OnLeftStickButton>(UpdateTextLSB);
        EventsManager.Instance.AddListener<OnRightStickButton>(UpdateTextRSB);

        EventsManager.Instance.AddListener<OnDPadRightButton>(UpdateTextDPadRight);
        EventsManager.Instance.AddListener<OnDPadLeftButton>(UpdateTextDPadLeft);
        EventsManager.Instance.AddListener<OnDPadBottomButton>(UpdateTextDPadDown);
        EventsManager.Instance.AddListener<OnDPadUpButton>(UpdateTextDPadUp);
    }

    private void OnGUI()
    {
        GUILayout.Label("DualShock rotation : " + PS4Controller.Instance.rotation);
        GUILayout.Label("DualShock acceleration : " + PS4Controller.Instance.acceleration);
        GUILayout.Label("First touch input position : " + PS4Controller.Instance.firstPosition);
        GUILayout.Label("Second touch input position : " + PS4Controller.Instance.secondTouchPosition);
        GUILayout.Label(textToDisplay);

        if (GUI.Button(new Rect(10, 570, 100, 30), "Left Vibrate"))
            EventsManager.Instance.Raise(new OnVibrate(false));

        if (GUI.Button(new Rect(115, 570, 100, 30), "Right Vibrate"))
            EventsManager.Instance.Raise(new OnVibrate(true));

        if (GUI.Button(new Rect(220, 570, 100, 30), "Set Light Color"))
            EventsManager.Instance.Raise(new OnLightSwitchColor());

        if (GUI.Button(new Rect(10, 605, 100, 30), "ON/OFF Flash"))
            EventsManager.Instance.Raise(new OnLightFlash());

        if (GUI.Button(new Rect(115, 605, 100, 30), "Vibrate All"))
            EventsManager.Instance.Raise(new OnVibrate(false, true));

        if (GUI.Button(new Rect(220, 605, 100, 30), "Stop Vibrate"))
            EventsManager.Instance.Raise(new OnStopVibrate());
    }

    public void UpdateTextCross(OnCrossButton e)
    {
        textToDisplay = "cross button";
    }

    public void UpdateTextSquare(OnSquareButton e)
    {
        textToDisplay = "square button";
    }

    public void UpdateTextTriangle(OnTriangleButton e)
    {
        textToDisplay = "triangle button";
    }

    public void UpdateTextRound(OnRoundButton e)
    {
        textToDisplay = "round button";
    }

    public void UpdateTextR1(ONR1Button e)
    {
        textToDisplay = "R1 Button";
    }

    public void UpdateTextR2(ONR2Button e)
    {
        textToDisplay = "R2 Button";
    }

    public void UpdateTextL1(ONL1Button e)
    {
        textToDisplay = "L1 Button";
    }

    public void UpdateTextL2(ONL2Button e)
    {
        textToDisplay = "L2 Button";
    }

    public void UpdateTextMenu(OnMenuButton e)
    {
        textToDisplay = "Menu Button";
    }

    public void UpdateTextOptions(OnOptionsButton e)
    {
        textToDisplay = "Options Button";
    }

    public void UpdateTextShare(OnShareButton e)
    {
        textToDisplay = "Share Button";
    }

    public void UpdateTextLSB(OnLeftStickButton e)
    {
        textToDisplay = "Left Stick Button";
    }

    public void UpdateTextRSB(OnRightStickButton e)
    {
        textToDisplay = "Right Stick Button";
    }

    public void UpdateTextDPadLeft(OnDPadLeftButton e)
    {
        textToDisplay = "Left D-Pad Button";
    }

    public void UpdateTextDPadRight(OnDPadRightButton e)
    {
        textToDisplay = "Right D-Pad Button";
    }

    public void UpdateTextDPadUp(OnDPadUpButton e)
    {
        textToDisplay = "Up D-Pad Button";
    }

    public void UpdateTextDPadDown(OnDPadBottomButton e)
    {
        textToDisplay = "Bottom D-Pad Button";
    }

    private void OnDestroy()
    {
        EventsManager.Instance.RemoveListener<OnSquareButton>(UpdateTextSquare);
        EventsManager.Instance.RemoveListener<OnTriangleButton>(UpdateTextTriangle);
        EventsManager.Instance.RemoveListener<OnRoundButton>(UpdateTextRound);
        EventsManager.Instance.RemoveListener<OnCrossButton>(UpdateTextCross);

        EventsManager.Instance.RemoveListener<ONR1Button>(UpdateTextR1);
        EventsManager.Instance.RemoveListener<ONR2Button>(UpdateTextR2);
        EventsManager.Instance.RemoveListener<ONL1Button>(UpdateTextL1);
        EventsManager.Instance.RemoveListener<ONL2Button>(UpdateTextL2);

        EventsManager.Instance.RemoveListener<OnMenuButton>(UpdateTextMenu);
        EventsManager.Instance.RemoveListener<OnShareButton>(UpdateTextShare);
        EventsManager.Instance.RemoveListener<OnOptionsButton>(UpdateTextOptions);

        EventsManager.Instance.RemoveListener<OnLeftStickButton>(UpdateTextLSB);
        EventsManager.Instance.RemoveListener<OnRightStickButton>(UpdateTextRSB);

        EventsManager.Instance.RemoveListener<OnDPadRightButton>(UpdateTextDPadRight);
        EventsManager.Instance.RemoveListener<OnDPadLeftButton>(UpdateTextDPadLeft);
        EventsManager.Instance.RemoveListener<OnDPadBottomButton>(UpdateTextDPadDown);
        EventsManager.Instance.RemoveListener<OnDPadUpButton>(UpdateTextDPadUp);
    }
}
