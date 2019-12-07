using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementManager : MonoBehaviour
{
    /* when you want achievement to be earned go to script where it
     * will be earned in. ex: highscore script. clarify name of
     * in a string. so ...
     * //public string achievementName;
     * and have a function that goes and uses
     * //AchievementManager.Instance.EarnAchievement(achievementName);
     * 
     */

    public GameObject achievementPrefab; //holds achievement prefab

    //used for referencing the content boxes of each category
    public GameObject Tutorial;
    public GameObject Easy;
    public GameObject Medium;
    public GameObject Hard;
    public GameObject Zombie;

    //holds sprites for achievements
    public Sprite[] Sprites;

    public GameObject visualAchievement;

    public Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();

    public GameObject EarnCanvas;

    public Color Gold;

    public TextMeshProUGUI textPoints;

    private static AchievementManager instance;

    private int fadeTime = 2;

    public static AchievementManager Instance
    { get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<AchievementManager>();
            }
            return AchievementManager.instance;
        }

    }


    //These will be used to make achievements in the inspector if wanted.
    //to make one with dependencies hard code them.
    public GameObject[] Category;
    public string[] AchievementTitle;
    public string[] AchievementDescription;
    public int[] AchievementScore;
    public int[] AchievementSprite;

    // Start is called before the first frame update
    // used to instantiate all achievements we want 
    void Start()
    {
        //CreateAchievement(Easy, "Pressed Space", "Wow it Works", 5, 0);
        //CreateAchievement(Easy, "Pressed W", "Wow it Works", 5, 0);
        //CreateAchievement(Easy, "All Keys", "Wow it Works", 10, 0, new string[] {"Pressed Space","Pressed W"});
        if(Category.Length != 0)
        {
            for (int i = 0; i < Category.Length; i++)
            {
                CreateAchievement(Category[i], AchievementTitle[i], AchievementDescription[i], AchievementScore[i], AchievementSprite[i]);
            }
        }

            
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.Space))
        {
            EarnAchievement("Press Space");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            EarnAchievement("Press W");
        }
        */
    }

    public void CreateAchievement(GameObject category, string title, string description, int points, int spriteIndex, string[] dependencies = null)
    {
        GameObject achievement = (GameObject)Instantiate(achievementPrefab, category.transform);

        Achievement newAchievement = new Achievement(title, description, points, spriteIndex, achievement);

        achievements.Add(title, newAchievement);

        achievement.transform.GetChild(0).GetComponent<Image>().sprite = Sprites[spriteIndex];
        achievement.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(title);
        achievement.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(description);
        achievement.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText(points.ToString());

        if(dependencies != null)
        {
            foreach (string achievementTitle in dependencies)
            {
                Achievement dependency = achievements[achievementTitle];
                dependency.Baby = title;
                newAchievement.AddDependency(achievements[achievementTitle]);

                //dependemcy = press space <-- child/baby = press w
                //new achievement = press w --> press space
            }
        }
    }

    public void SetAchievementInfo(GameObject achievement, string title)
    {
        achievement.transform.SetParent(EarnCanvas.transform);
        achievement.transform.GetChild(0).GetComponent<Image>().sprite = Sprites[achievements[title].SpriteIndex];
        achievement.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText(title);
        achievement.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(achievements[title].Description);
        achievement.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText(achievements[title].Points.ToString());
    }

    public void EarnAchievement(string title)
    {
        if(achievements[title].EarnAchievement())
        {
            //earned new achievement
            GameObject achievement = (GameObject)Instantiate(visualAchievement, EarnCanvas.transform);
            SetAchievementInfo(achievement, title);
            textPoints.SetText("Points: " + PlayerPrefs.GetInt("Points"));
            StartCoroutine(FadeAchievement(achievement));
        }
    }

    /*
    public IEnumerator HideAchievement(GameObject achievement)
    {
        yield return new WaitForSeconds(5);
        Destroy(achievement);
    }
    */

    private IEnumerator FadeAchievement(GameObject achievement)
    {
        CanvasGroup canvasGroup = achievement.GetComponent<CanvasGroup>();

        float rate = 1.0f / fadeTime;

        int startAlpha = 0;
        int endAlpha = 1;

        for(int i = 0; i < 2; i++)
        {
            float progress = 0.0f;

            while (progress < 1.0)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);

                progress += rate * Time.deltaTime;

                yield return null;
            }

            yield return new WaitForSeconds(2);
            startAlpha = 1;
            endAlpha = 0;
        }
        Destroy(achievement);
    }

    public void ResetAchievements()
    {
        //comment out on release vvv
        PlayerPrefs.DeleteAll();
    }
}
