using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    [SerializeField]
    private Transform playerTrans;
    private float spawnZ = -50.0f;
    private float tileLength = 50.0f;
    private int amountTilesOnScreen = 10;
    private float saveZone = 75.0f;
    private int lastPrefabIndex = 0;
    public Text scoreHolder;
    public TextMeshProUGUI scoreDisplay;
    private int scoreCounter = -10;
    public GameObject phoneObject = null;
    public Text HighScore;
    public TextMeshProUGUI highScoreDisplay;
    public GameObject appManager = null;
    public AchievementManager achievementManager;
    public bool easy;
    public bool medium;
    public bool hard;

    public int EasyHighScore;
    public int MediumHighScore;
    public int HardHighScore;
    public int ZombieHighScore;

    private List<GameObject> activeTiles;

    // Start is called before the first frame update
    private void Start()
    {
        //appManager = GameObject.FindGameObjectWithTag("AppManager");
        EasyHighScore = appManager.GetComponent<AppManagerScript>().EasyHighScore;
        MediumHighScore = appManager.GetComponent<AppManagerScript>().MediumHighScore;
        HardHighScore = appManager.GetComponent<AppManagerScript>().HardHighScore;
        ZombieHighScore = appManager.GetComponent<AppManagerScript>().ZombieHighScore;
        if (easy == true)
        {
            highScoreDisplay.SetText(EasyHighScore.ToString());
            HighScore.text = EasyHighScore.ToString();
        }
        else if(medium == true)
        {
            highScoreDisplay.SetText(MediumHighScore.ToString());
            HighScore.text = MediumHighScore.ToString();
        }
        else if(hard == true)
        {
            highScoreDisplay.SetText(HardHighScore.ToString());
            HighScore.text = HardHighScore.ToString();
        }
        else
        {
            highScoreDisplay.SetText(ZombieHighScore.ToString());
            HighScore.text = ZombieHighScore.ToString();
        }
        activeTiles = new List<GameObject>();
        for(int i = 0; i < amountTilesOnScreen; i++)
        {
            if (i < 4)
                SpawnTile(0);
            else
                SpawnTile();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if((playerTrans.position.z - saveZone) > (spawnZ - amountTilesOnScreen * tileLength))
        {
            SpawnTile();
            DeleteTile();
        }

        if (easy == true)
        {
            if (scoreCounter == 0)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Started Easy");
            }
            if (scoreCounter >= 50)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Scored 50 On Easy");
            }
            if (scoreCounter >= 100)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Scored 100 On Easy");
            }
            if (scoreCounter >= 250)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Scored 250 On Easy");
            }
            if (scoreCounter == 500)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Scored 500 On Easy");
            }
        }
        else if (medium == true)
        {
            if (scoreCounter == 0)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Started Medium");
            }
            if (scoreCounter >= 50)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Scored 50 On Medium");
            }
            if (scoreCounter >= 100)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Scored 100 On Medium");
            }
            if (scoreCounter >= 250)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Scored 250 On Medium");
            }
            if (scoreCounter == 500)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Scored 500 On Medium");
            }
        }
        else if (hard == true)
        {
            if (scoreCounter == 0)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Started Hard");
            }
            if (scoreCounter >= 50)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Scored 50 On Hard");
            }
            if (scoreCounter >= 100)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Scored 100 On Hard");
            }
            if (scoreCounter >= 250)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Scored 250 On Hard");
            }
            if (scoreCounter == 500)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Scored 500 On Hard");
            }
        }
        else
        {
            if (scoreCounter == 0)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Started Zombies");
            }
            if (scoreCounter >= 500)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Scored 500 On Zombies");
            }
            if (scoreCounter >= 1000)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Scored 1000 On Zombies");
            }
            if (scoreCounter >= 5000)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Scored 5000 On Zombies");
            }
            if (scoreCounter == 10000)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Scored 10000 On Zombies");
            }
            if (scoreCounter == 50000)
            {
                achievementManager.GetComponent<AchievementManager>().EarnAchievement("Scored 50000 On Zombies");
            }
        }
    }

    private void SpawnTile(int prefabIndex = -1)
    {
        int check = phoneObject.transform.GetComponent<ScreenController>().NumCorrectReturn();
        if (check < 10)
            scoreCounter = scoreCounter + 1;
        else
            scoreCounter = scoreCounter + ((int)((check+10)/10));
        scoreDisplay.SetText(scoreCounter.ToString());
        scoreHolder.text = scoreCounter.ToString();
        GameObject go;
        if (prefabIndex == -1)
            go = Instantiate(tilePrefabs[RandomPrefabIndex()]) as GameObject;
        else
            go = Instantiate(tilePrefabs[prefabIndex]) as GameObject;
        go.transform.SetParent(transform);
        go.transform.position = Vector3.forward * spawnZ;
        spawnZ += tileLength;
        activeTiles.Add(go);
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    private int RandomPrefabIndex()
    {
        if (tilePrefabs.Length <= 1)
            return 0;

        int randomIndex = lastPrefabIndex;
        while(randomIndex == lastPrefabIndex)
        {
            randomIndex = UnityEngine.Random.Range(0, tilePrefabs.Length);
        }

        lastPrefabIndex = randomIndex;
        return randomIndex;
    }

    public void TryUpdateHighScore()
    {
        if (easy == true)
        {
            highScoreDisplay.SetText(EasyHighScore.ToString());
            HighScore.text = EasyHighScore.ToString();
            int a = Int32.Parse(HighScore.text);
            int b = Int32.Parse(scoreHolder.text);
            if (a < b)
            {
                HighScore.text = scoreHolder.text;
                highScoreDisplay.SetText(scoreHolder.text);
                appManager.GetComponent<AppManagerScript>().EasyHighScore = b;
                appManager.GetComponent<AppManagerScript>().SavePlayer();
            }
        }
        else if (medium == true)
        {
            highScoreDisplay.SetText(MediumHighScore.ToString());
            HighScore.text = MediumHighScore.ToString();
            int a = Int32.Parse(HighScore.text);
            int b = Int32.Parse(scoreHolder.text);
            if (a < b)
            {
                HighScore.text = scoreHolder.text;
                highScoreDisplay.SetText(scoreHolder.text);
                appManager.GetComponent<AppManagerScript>().MediumHighScore = b;
                appManager.GetComponent<AppManagerScript>().SavePlayer();
            }
        }
        else if (hard == true)
        {
            highScoreDisplay.SetText(HardHighScore.ToString());
            HighScore.text = HardHighScore.ToString();
            int a = Int32.Parse(HighScore.text);
            int b = Int32.Parse(scoreHolder.text);
            if (a < b)
            {
                HighScore.text = scoreHolder.text;
                highScoreDisplay.SetText(scoreHolder.text);
                appManager.GetComponent<AppManagerScript>().HardHighScore = b;
                appManager.GetComponent<AppManagerScript>().SavePlayer();
            }
        }
        else
        {
            highScoreDisplay.SetText(ZombieHighScore.ToString());
            HighScore.text = ZombieHighScore.ToString();
            int a = Int32.Parse(HighScore.text);
            int b = Int32.Parse(scoreHolder.text);
            if (a < b)
            {
                HighScore.text = scoreHolder.text;
                highScoreDisplay.SetText(scoreHolder.text);
                appManager.GetComponent<AppManagerScript>().ZombieHighScore = b;
                appManager.GetComponent<AppManagerScript>().SavePlayer();
            }
        }
    }
}
