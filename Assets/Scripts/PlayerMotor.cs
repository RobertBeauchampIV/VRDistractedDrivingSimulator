using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerMotor : MonoBehaviour
{
    public float speed;
    private static float curSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        curSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.deltaTime);
        //Debug.Log(curSpeed);
        transform.Translate(Vector3.back * Time.deltaTime * curSpeed);
// MoveHandler();
    }
    /*
    void MoveHandler()
    {
        if (SteamVR_Actions._default.LeftClick.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            transform.Translate(Vector3.right * (curSpeed/6));
        }
        if (SteamVR_Actions._default.RightClick.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            transform.Translate(Vector3.left * (curSpeed/6));
        }
    }
    */

    public void GameOverSpeed()
    {
        curSpeed = 0;
    }
}