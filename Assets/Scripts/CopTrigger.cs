using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CopTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Phone"))
        {
            Debug.Log("Phone Catch");

            CarMovementStateMachine cmsm = GameObject.FindObjectOfType<CarMovementStateMachine>();

            int check = cmsm.heartsRemaining;
            if (check > 1)
            {
                cmsm.heartsRemaining = cmsm.heartsRemaining - 1;
                Destroy(cmsm.Hearts[check - 1]);
                cmsm.Pulse(.5f, 150, 125, SteamVR_Input_Sources.LeftHand);
                cmsm.Pulse(.5f, 150, 125, SteamVR_Input_Sources.RightHand);
            }
            else if (check == 1)
            {
                cmsm.heartsRemaining = cmsm.heartsRemaining - 1;
                Destroy(cmsm.Hearts[check - 1]);
                cmsm.currentState = FSMStateMovement_Waypoint.GameOver;
                cmsm.GetComponent<PlayerMotor>().GameOverSpeed();
                cmsm.LerpGameObject.GetComponent<PlayerMotor>().GameOverSpeed();
            }

            GameObject.Destroy(this);
        }
    }
}
