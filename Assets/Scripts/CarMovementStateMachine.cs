using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public enum FSMStateMovement_Waypoint
{
    None, LeftLane, MidLane, RightLane, GameOver
}

public class CarMovementStateMachine : MonoBehaviour
{
    private bool gameOverBool = false;
    private bool Restart = false;
    private bool BackHome = false;
    public float laneSwitchSpeed = 0.1f;
    public GameObject LerpLeftLocation;
    public GameObject LerpMidLocation;
    public GameObject LerpRightLocation;
    public GameObject LerpGameObject;
    public GameObject AppManager;
    public Image[] Hearts;
    public int heartsRemaining = 3;
    public GameObject instructionPanel;
    public GameObject TileManager;
    public FSMStateMovement_Waypoint currentState;

    public SteamVR_Action_Vibration hapticAction;


    // Start is called before the first frame update
    void Start()
    {
        instructionPanel.SetActive(false);
        currentState = FSMStateMovement_Waypoint.MidLane;
    }

    // Update is called once per frame
    void Update()
    {

        switch (currentState)
        {
            case FSMStateMovement_Waypoint.MidLane: UpdateToMidLane(); break;
            case FSMStateMovement_Waypoint.LeftLane: UpdateToLeftLane(); break;
            case FSMStateMovement_Waypoint.RightLane: UpdateToRightLane(); break;
            case FSMStateMovement_Waypoint.GameOver: GameOver(); break;
        }
    }


    void UpdateToMidLane()


    {
        transform.position = Vector3.Lerp(this.transform.position, LerpMidLocation.transform.position, laneSwitchSpeed);
        //for up clicks on (LeftHand) Right
        if (SteamVR_Actions._default.RightClick.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Pulse(.1f, 150, 25, SteamVR_Input_Sources.LeftHand);
            //Debug.Log("RightClickPressed");
            currentState = FSMStateMovement_Waypoint.RightLane;
        }

        //for up clicks on (LeftHand) left
        if (SteamVR_Actions._default.LeftClick.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Pulse(.1f, 150, 25, SteamVR_Input_Sources.LeftHand);
            //Debug.Log("LeftClickPressed");
            currentState = FSMStateMovement_Waypoint.LeftLane;
        }
    }

    void UpdateToLeftLane()
    {
        transform.position = Vector3.Lerp(this.transform.position, LerpLeftLocation.transform.position, laneSwitchSpeed);
        //for up clicks on (LeftHand) Right
        if (SteamVR_Actions._default.RightClick.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Pulse(.1f, 150, 25, SteamVR_Input_Sources.LeftHand);
            //Debug.Log("RightClickPressed");
            currentState = FSMStateMovement_Waypoint.MidLane;
        }
    }

    void UpdateToRightLane()
    {
        transform.position = Vector3.Lerp(this.transform.position, LerpRightLocation.transform.position, laneSwitchSpeed);
        //for up clicks on (LeftHand) left
        if (SteamVR_Actions._default.LeftClick.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Pulse(.1f, 150, 25, SteamVR_Input_Sources.LeftHand);
            //Debug.Log("LeftClickPressed");
            currentState = FSMStateMovement_Waypoint.MidLane;
        }
    }

    void GameOver()
    {
        if (gameOverBool == false)
        {
            TileManager.GetComponent<TileManager>().TryUpdateHighScore();
            instructionPanel.SetActive(true);
            Pulse(3, 150, 125, SteamVR_Input_Sources.LeftHand);
            Pulse(3, 150, 125, SteamVR_Input_Sources.RightHand);
            gameOverBool = true;
        }
        if (SteamVR_Actions._default.SkipButton.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            Restart = true;
        }
        if (Restart == true)
        {
            AppManager.GetComponent<AppManagerScript>().PlayEasy();
        }
        if (SteamVR_Actions._default.SkipButton.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            Restart = true;
        }
        if (Restart == true)
        {
            AppManager.GetComponent<AppManagerScript>().RestartScene();
        }
        if (SteamVR_Actions._default.SkipButton.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            BackHome = true;
        }
        if (BackHome == true)
        {
            AppManager.GetComponent<AppManagerScript>().MainMenuReturn();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            collision.gameObject.tag = "Untagged";
            int check = heartsRemaining;
            if(check > 1)
            {
                heartsRemaining = heartsRemaining - 1;
                Destroy(Hearts[check-1]);
                Pulse(.5f, 150, 125, SteamVR_Input_Sources.LeftHand);
                Pulse(.5f, 150, 125, SteamVR_Input_Sources.RightHand);
            }
            else if (check == 1)
            {
                heartsRemaining = heartsRemaining - 1;
                Destroy(Hearts[check - 1]);
                currentState = FSMStateMovement_Waypoint.GameOver;
                this.GetComponent<PlayerMotor>().GameOverSpeed();
                LerpGameObject.GetComponent<PlayerMotor>().GameOverSpeed();
            }
        }
    }


    public void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        hapticAction.Execute(0, duration, frequency, amplitude, source);
    }
}
