using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveManger : MonoBehaviour
{
    public GameObject[] lockChar;
    public GameObject[] unlockChar;
    public GameObject UINotice;

    enum Achieve { UnlockHardCore }
    Achieve[] achieves;
    WaitForSecondsRealtime wait;

    void Awake()
    {
        achieves = (Achieve[])Enum.GetValues(typeof(Achieve));

        wait = new WaitForSecondsRealtime(5);

        if (!PlayerPrefs.HasKey("MyData"))
            Init();
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);

        foreach (Achieve achieve in achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 0);
        }
    }

    void Start()
    {
        UnlockChar();
    }

    void UnlockChar()
    {
        for (int index = 0; index < lockChar.Length; index++)
        {
            string achieveName = achieves[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1;
            lockChar[index].SetActive(!isUnlock);
            unlockChar[index].SetActive(isUnlock);
        }
    }

    void LateUpdate()
    {
        foreach(Achieve achieve in achieves)
        {
            CheckAchieve(achieve);
        }
    }

    void CheckAchieve(Achieve achieve)
    {
        bool isAchieve = false;

        switch(achieve)
        {
            case Achieve.UnlockHardCore:
                isAchieve = GameManager.Instance.gameTime == GameManager.Instance.maxGameTime;
                break;
        }

        if (isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);

            for(int index = 0; index < UINotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achieve;
                UINotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        UINotice.SetActive(true);
        yield return wait;
        UINotice.SetActive(false);
    }

}
