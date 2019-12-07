using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement
{
    private string name;

    private string description;

    private bool unlocked;

    private int points;

    private int spriteIndex;

    private GameObject achievementRef;

    private string baby;

    public Achievement(string name, string description, int points, int spriteIndex, GameObject achievementRef)
    {
        this.Name = name;
        this.Description = description;
        this.Unlocked = false;
        this.Points = points;
        this.SpriteIndex = spriteIndex;
        this.achievementRef = achievementRef;
        LoadAchievement();
    }

    public string Name { get => name; set => name = value; }
    public string Description { get => description; set => description = value; }
    public bool Unlocked { get => unlocked; set => unlocked = value; }
    public int Points { get => points; set => points = value; }
    public int SpriteIndex { get => spriteIndex; set => spriteIndex = value; }
    public string Baby { get => baby; set => baby = value; }

    private List<Achievement> dependencies = new List<Achievement>();




    public void AddDependency(Achievement dependendancy)
    {
        dependencies.Add(dependendancy);
    }

    public bool EarnAchievement()
    {
        if(!Unlocked && !dependencies.Exists(x => x.Unlocked == false))
        {
            achievementRef.GetComponent<Image>().color = AchievementManager.Instance.Gold;
            SaveAchievements(true);
            Unlocked = true;

            if (baby != null)
            {
                AchievementManager.Instance.EarnAchievement(baby);
            }

            return true;
        }
        return false;
    }

    public void SaveAchievements(bool value)
    {
        unlocked = value;

        int tmpPoints = PlayerPrefs.GetInt("Points");

        PlayerPrefs.SetInt("Points", tmpPoints += points);

        PlayerPrefs.SetInt(name, value ? 1 : 0);

        PlayerPrefs.Save();

    }

    public void LoadAchievement()
    {
        unlocked = PlayerPrefs.GetInt(name) == 1 ? true : false;

        if(unlocked)
        {
            AchievementManager.Instance.textPoints.SetText("Points: " + PlayerPrefs.GetInt("Points"));
            achievementRef.GetComponent<Image>().color = AchievementManager.Instance.Gold;
        }
    }
}
