using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public enum FSMState_Waypoint
{
    None, Tutorial1, Tutorial2, Tutorial3, MainMenu, SelectLevel
}


public class StateMachina : MonoBehaviour
{


    //keeps track of states
    FSMState_Waypoint currentState;

    public SteamVR_Action_Vibration hapticAction;
    public GameObject AchievementManagerRef;
    public GameObject AppManager;

    static bool UpLeftHand = false;
    static bool DownLeftHand = false;
    static bool RightLeftHand = false;
    static bool LeftLeftHand = false;
    static bool UpRightHand = false;
    static bool DownRightHand = false;
    static bool RightRightHand = false;
    static bool LeftRightHand = false;
    static bool ClickedOnUI = false;
    static bool ResetTutorialGame = false;
    static bool Skip = false;

    public GameObject LeftDPad;            //to use for tutorial lighting
    public GameObject RightDPad;           //to use for tutorial lighting

    void Start()
    {
        AppManager.GetComponent<AppManagerScript>().Tutorial1();
        currentState = FSMState_Waypoint.Tutorial1;
    }

    void Update()
    {

        switch (currentState)
        {
            case FSMState_Waypoint.Tutorial1: UpdateToTutorial1State(); break;
            case FSMState_Waypoint.Tutorial2: UpdateToTutorial2State(); break;
            case FSMState_Waypoint.Tutorial3: UpdateToTutorial3State(); break;
            case FSMState_Waypoint.MainMenu: UpdateToMainMenuState(); break;
        }

    }

    void FixedUpdate()
    {

    }


    //in this state you learn how to move with left d-pad
    void UpdateToTutorial1State()
    {

        Debug.Log("Entering tut1");
        if (SteamVR_Actions._default.SkipButton.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Skip = true;
        }

        //for up clicks on (LeftHand) up
        if (SteamVR_Actions._default.UpClick.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Pulse(.1f, 150, 25, SteamVR_Input_Sources.LeftHand);
            LeftDPad.transform.Find("UpLeftHand").gameObject.SetActive(true);
            UpLeftHand = true;
            //Debug.Log("UpClickPressed");
        }

        //for up clicks on (LeftHand) Down
        if (SteamVR_Actions._default.DownClick.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Pulse(.1f, 150, 25, SteamVR_Input_Sources.LeftHand);
            LeftDPad.transform.Find("DownLeftHand").gameObject.SetActive(true);
            DownLeftHand = true;
            //Debug.Log("DownClickPressed");
        }

        //for up clicks on (LeftHand) Right
        if (SteamVR_Actions._default.RightClick.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Pulse(.1f, 150, 25, SteamVR_Input_Sources.LeftHand);
            LeftDPad.transform.Find("RightLeftHand").gameObject.SetActive(true);
            RightLeftHand = true;
            //Debug.Log("RightClickPressed");
        }

        //for up clicks on (LeftHand) left
        if (SteamVR_Actions._default.LeftClick.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Pulse(.1f, 150, 25, SteamVR_Input_Sources.LeftHand);
            LeftDPad.transform.Find("LeftLeftHand").gameObject.SetActive(true);
            LeftLeftHand = true;
            //Debug.Log("LeftClickPressed");
        }
        if (UpLeftHand == true && DownLeftHand == true && RightLeftHand == true && LeftLeftHand == true)
        {
            Debug.Log("Entering tut2");
            AppManager.GetComponent<AppManagerScript>().SkipTutorial();
            AppManager.GetComponent<AppManagerScript>().Tutorial2();
            AchievementManagerRef.GetComponent<AchievementManager>().EarnAchievement("Tutorial 1");
            currentState = FSMState_Waypoint.Tutorial2;
        }
        if (Skip == true)
        {
            AchievementManagerRef.GetComponent<AchievementManager>().EarnAchievement("Found The Skip Button");
            AppManager.GetComponent<AppManagerScript>().SkipTutorial();
            AppManager.GetComponent<AppManagerScript>().MainMenu();
            currentState = FSMState_Waypoint.MainMenu;
        }
    }

    //in this state you learn how to do puzzles on right d-pad
    void UpdateToTutorial2State()
    {
        if (SteamVR_Actions._default.SkipButton.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Skip = true;
        }

        //for up clicks on (RightHand) up
        if (SteamVR_Actions._default.UpClick.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            Pulse(.1f, 150, 25, SteamVR_Input_Sources.RightHand);
            RightDPad.transform.Find("UpRightHand").gameObject.SetActive(true);
            UpRightHand = true;
            //Debug.Log("UpClickPressed");
        }

        //for up clicks on (RightHand) Down
        if (SteamVR_Actions._default.DownClick.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            Pulse(.1f, 150, 25, SteamVR_Input_Sources.RightHand);
            RightDPad.transform.Find("DownRightHand").gameObject.SetActive(true);
            DownRightHand = true;
            //Debug.Log("DownClickPressed");
        }

        //for up clicks on RightHand) Right
        if (SteamVR_Actions._default.RightClick.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            Pulse(.1f, 150, 25, SteamVR_Input_Sources.RightHand);
            RightDPad.transform.Find("RightRightHand").gameObject.SetActive(true);
            RightRightHand = true;
            //Debug.Log("RightClickPressed");
        }

        //for up clicks on (RightHand) left
        if (SteamVR_Actions._default.LeftClick.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            Pulse(.1f, 150, 25, SteamVR_Input_Sources.RightHand);
            RightDPad.transform.Find("LeftRightHand").gameObject.SetActive(true);
            LeftRightHand = true;
            //Debug.Log("LeftClickPressed");
        }
        if (UpRightHand == true && DownRightHand == true && RightRightHand == true && LeftRightHand == true)
        {
            Debug.Log("Entering tut3");
            AppManager.GetComponent<AppManagerScript>().SkipTutorial();
            AppManager.GetComponent<AppManagerScript>().Tutorial3();
            AchievementManagerRef.GetComponent<AchievementManager>().EarnAchievement("Tutorial 2");
            currentState = FSMState_Waypoint.Tutorial3;
        }
        if (Skip == true)
        {
            AchievementManagerRef.GetComponent<AchievementManager>().EarnAchievement("Found The Skip Button");
            AppManager.GetComponent<AppManagerScript>().SkipTutorial();
            AppManager.GetComponent<AppManagerScript>().MainMenu();
            currentState = FSMState_Waypoint.MainMenu;
        }
    }

    //in this state you learn how to click on UI buttons
    void UpdateToTutorial3State()
    {
        if (SteamVR_Actions._default.SkipButton.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Skip = true;
        }

        if (ClickedOnUI == true)
        {
            Debug.Log("Entering Main Menu");
            AppManager.GetComponent<AppManagerScript>().SkipTutorial();
            AppManager.GetComponent<AppManagerScript>().MainMenu();
            AchievementManagerRef.GetComponent<AchievementManager>().EarnAchievement("Tutorial 3");
            currentState = FSMState_Waypoint.MainMenu;
        }
        if (Skip == true)
        {
            AchievementManagerRef.GetComponent<AchievementManager>().EarnAchievement("Found The Skip Button");
            AppManager.GetComponent<AppManagerScript>().SkipTutorial();
            AppManager.GetComponent<AppManagerScript>().MainMenu();
            currentState = FSMState_Waypoint.MainMenu;
        }
    }

    void UpdateToMainMenuState()
    {
        if (ResetTutorialGame == true)
        {
            Debug.Log("Reseting Tutorial");
            AppManager.GetComponent<AppManagerScript>().SkipTutorial();
            AppManager.GetComponent<AppManagerScript>().Tutorial1();
            AchievementManagerRef.GetComponent<AchievementManager>().EarnAchievement("Need More Practice");
            ResetTutorial();
            currentState = FSMState_Waypoint.Tutorial1;
        }
    }

    //called on button ui
    public void ClickBoolTrue()
    {
        ClickedOnUI = true;
    }

    public void ClickedOnResetTutorial()
    {
        ResetTutorialGame = true;
    }

    private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        hapticAction.Execute(0, duration, frequency, amplitude, source);
    }

    public void RightButtonClick()
    {

            Pulse(.1f, 150, 25, SteamVR_Input_Sources.RightHand);
            //Debug.Log("UpClickPressed");

    }

    public void ResetTutorial()
    {
        UpLeftHand = false;
        DownLeftHand = false;
        RightLeftHand = false;
        LeftLeftHand = false;
        UpRightHand = false;
        DownRightHand = false;
        RightRightHand = false;
        LeftRightHand = false;
        ClickedOnUI = false;
        Skip = false;
        ResetTutorialGame = false;
    }
}
