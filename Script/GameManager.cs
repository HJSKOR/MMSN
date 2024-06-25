using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Game Control")]
    public PoolManager pool;
    public Player player;
    public bool islive;
    [Header("Player info")]
    public int PlayerID;
    public int level;
    public float health;
    public float maxhealth = 100;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
    [Header("Game Object")]
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    public LevelUp LevelUpUI;
    public Result UIResult;
    public Transform uiJoyStick;
    public GameObject EnemyCleaner;

    void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }

    public void GameStart(int id)
    {
        PlayerID = id;
        health = maxhealth;

        player.gameObject.SetActive(true);
        LevelUpUI.select(PlayerID % 2);
        Resume();
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        islive = false;

        yield return new WaitForSeconds(0.5f);

        UIResult.gameObject.SetActive(true);
        UIResult.Lose();
        Stop();
    }
    public void GameClear()
    {
        StartCoroutine(GameClearRoutine());
    }

    IEnumerator GameClearRoutine()
    {
        islive = false;
        EnemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        UIResult.gameObject.SetActive(true);
        UIResult.Win();
        Stop();
    }

    void Update()
    {
        if (!islive)
            return;

        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameClear();
        }
    }

    public void GetExp()
    {
        if (!islive)
            return;

        exp++;

        if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)])
        {
            level++;
            exp = 0;
            LevelUpUI.show();
        }
    }

    public void Stop()
    {
        islive = false;
        Time.timeScale = 0;
        uiJoyStick.localScale = Vector3.zero;
    }
    public void Resume()
    {
        islive = true;
        Time.timeScale = 1;
        uiJoyStick.localScale = Vector3.one;
    }
}
