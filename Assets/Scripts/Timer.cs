using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    public Image fillImage;
    const float permTimeAmt = 10;
    float timeAmt = 10;
    float time;
    private GameObject phoneScript;
    private void Start()
    {
        phoneScript = GameObject.FindGameObjectWithTag("PhoneScript");
        time = timeAmt;
    }

    private void Update()
    {
        if(time > 0)
        {
            time -= Time.deltaTime;
            fillImage.fillAmount = time / timeAmt;
        }
        else
        {
            phoneScript.GetComponent<ScreenController>().DisplayNextPuzzle(5);
            resetTimer();
        }
    }

    public void resetTimer()
    {
        time = timeAmt = permTimeAmt;
    }
}
