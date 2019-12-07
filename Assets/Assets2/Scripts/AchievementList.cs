using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementList : MonoBehaviour
{
    public GameObject achievementList;
    public ScrollRect scrollRect;

    public void ChangeCategory()
    {
        scrollRect.content = achievementList.GetComponent<RectTransform>();
    }
}
