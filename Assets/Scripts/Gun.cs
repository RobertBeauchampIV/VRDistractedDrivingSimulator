using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Gun : MonoBehaviour
{
    [Tooltip("Set bullet prefab")]
    [SerializeField]
    private GameObject m_bulletPrefab = null;

    [Tooltip("Set bullet spawn location")]
    [SerializeField]
    private Transform m_leftbulletSpawnLocation = null;
    [SerializeField]
    private Transform m_rightbulletSpawnLocation = null;
    [Tooltip("Set speed of the bullet")]
    [SerializeField]
    private float m_bulletSpeed = 25.0f;

    [Tooltip("How long in seconds the bullet will fly for.")]
    [SerializeField]
    private float m_bulletTravelTime = 1.0f;

    [Tooltip("What sound will play when the trigger is pressed.")]
    [SerializeField]
    private AudioSource m_bulletSound = null;

    //Used to hold attached hand to gun
    public Hand m_leftattachedHand = null;
    public Hand m_rightattachedHand = null;

    //Used to handle which hand is holding gun
    public SteamVR_Input_Sources m_controllerleft;
    public SteamVR_Input_Sources m_controllerright;

    //Used to handle if gun shootable or not
    private bool shootable = false;

    public AudioClip shoot;
    public AudioSource leftGun;
    public AudioSource rightGun;

    public SteamVR_Action_Vibration hapticAction;

    // Update is called once per frame
    void Update()
    {
        //If shootable, then check to see if attached controller's trigger is pressed
        if (m_leftattachedHand.grabPinchAction.GetStateDown(m_controllerleft))
        {
            Pulse(.1f, 150, 25, SteamVR_Input_Sources.LeftHand);
            //Shoot the bullet!
            //Debug.Log("Gun Trigger Pressed!");
            leftGun.PlayOneShot(shoot);
            GameObject bullet = Instantiate(m_bulletPrefab, m_leftbulletSpawnLocation.transform.position, m_leftbulletSpawnLocation.transform.rotation, GameObject.FindObjectOfType<CarMovementStateMachine>().transform) as GameObject;
            bullet.GetComponent<Rigidbody>().velocity = (bullet.transform.forward * m_bulletSpeed);
            //bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * m_bulletSpeed);

            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(bullet.transform.position, bullet.transform.forward, out hitInfo) && hitInfo.rigidbody != null && hitInfo.rigidbody.GetComponent<Zombie>() != null)
            {
                Zombie zombie = hitInfo.rigidbody.GetComponent<Zombie>();
                if (zombie != null && zombie.CompareTag("Obstacle"))
                {
                    bullet.tag = "Untagged";
                    zombie.tag = "Untagged";
                    Destroy(zombie.GetComponent<CapsuleCollider>());
                    Destroy(zombie.GetComponent<Rigidbody>());

                    zombie.GetComponent<Animator>().SetBool("Dead", true);

                    GameObject.FindObjectOfType<ScreenController>().PassCorrectAnswer();
                }
            }

            //m_bulletSound.Play();

            Destroy(bullet, m_bulletTravelTime);
        }
        if (m_rightattachedHand.grabPinchAction.GetStateDown(m_controllerright))
        {
            Pulse(.1f, 150, 25, SteamVR_Input_Sources.RightHand);
            //Shoot the bullet!
            //Debug.Log("Gun Trigger Pressed!");
            rightGun.PlayOneShot(shoot);
            GameObject bullet = Instantiate(m_bulletPrefab, m_rightbulletSpawnLocation.transform.position, m_rightbulletSpawnLocation.transform.rotation, GameObject.FindObjectOfType<CarMovementStateMachine>().transform) as GameObject;
            bullet.GetComponent<Rigidbody>().velocity = (bullet.transform.forward * m_bulletSpeed);
            //bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * m_bulletSpeed);

            RaycastHit hitInfo = new RaycastHit();
            if(Physics.Raycast(bullet.transform.position, bullet.transform.forward, out hitInfo) && hitInfo.rigidbody != null && hitInfo.rigidbody.GetComponent<Zombie>() != null)
            {
                Zombie zombie = hitInfo.rigidbody.GetComponent<Zombie>();
                if (zombie != null && zombie.CompareTag("Obstacle"))
                {
                    bullet.tag = "Untagged";
                    zombie.tag = "Untagged";
                    Destroy(zombie.GetComponent<CapsuleCollider>());
                    Destroy(zombie.GetComponent<Rigidbody>());

                    zombie.GetComponent<Animator>().SetBool("Dead", true);

                    GameObject.FindObjectOfType<ScreenController>().PassCorrectAnswer();
                }
            }

            //m_bulletSound.Play();

            Destroy(bullet, m_bulletTravelTime);
        }
    }

    public void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        hapticAction.Execute(0, duration, frequency, amplitude, source);
    }
}
