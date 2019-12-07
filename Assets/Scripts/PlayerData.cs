using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int EasyHScore;
    public int MediumHScore;
    public int HardHScore;
    public int ZombieHScore;

    public PlayerData(AppManagerScript player)
    {
        EasyHScore = player.GetComponent<AppManagerScript>().EasyHighScore;
        MediumHScore = player.GetComponent<AppManagerScript>().MediumHighScore;
        HardHScore = player.GetComponent<AppManagerScript>().HardHighScore;
        ZombieHScore = player.GetComponent<AppManagerScript>().ZombieHighScore;
    }
}
