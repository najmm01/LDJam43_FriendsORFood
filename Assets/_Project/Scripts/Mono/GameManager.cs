using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum ZombieState
{
    Seeking, Idle
}

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyTypes;
    private int score;
    public GameObject hero;
    public GameManagerData data;
    public Slider[] hungerSliders;
    public GameObject gameOverPanel;
    public Text endGameMessage;
    public Text scoreLabel;

    [Header("Set Dynamically")]
    public float globalHunger;

    [Header("Invincibility")]
    public Human[] humans;
    public float invincibilityPeriod;


    string[] tipMessages =
    {
        "You control the hero, everyone else is expendable. Sacrifices must be made.",
        "Remember, you cannot pick up health vials if the medic is dead.",
        "Remember, allies take a share of your food and health pickups.",
        "Keep an eye on those Hunger levelbars.",
        "You control the hero, everyone else is expendable. Sacrifices must be made."
    };
    string messageString = "";

    public float Hunger
    {
        get
        {
            return globalHunger;
        }

        set
        {
            //set the hunger value, update each of the slider UI 
            if (value < 0)
                value = 0;

            globalHunger = value;
            foreach (var slider in hungerSliders)
            {
                slider.value = value / 100;
            }

            //call KillEveryOne if hunger levels go beyond 100
            if (globalHunger >= 100)
            {
                StopAllCoroutines();
                KillEveryOne();
            }
        }
    }

    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
            scoreLabel.text = score.ToString("N0");
        }
    }

    public void StartInvincibility()
    {
        foreach (var human in humans)
        {
            //if human has not been destroyed, start blinking
            if (human)
                human.StartInvincibility(invincibilityPeriod);
        }
    }

    //this method returns an enemy gameobject randomly from the enemyTypes array
    public GameObject GenAnEnemy()
    {
        return enemyTypes[Random.Range(0, enemyTypes.Length)];
    }

    internal int humansAliveCount = 6;
    internal bool medicDead = false;

    bool _loadingNextScene;
    bool _gameOver;

    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Score = 0;
        Hunger = 0;
        StartCoroutine(IncreaseHunger());
    }

    IEnumerator IncreaseHunger()
    {
        //keep increasing the hunger after the hungerIncreaseTimeGap
        while (true)
        {
            yield return new WaitForSeconds(data.hungerIncreaseTimeGap);
            Hunger++;
        }
    }

    public void ReduceHunger(float value)
    {
        //the more humans are alive, the less is the decrease to the global hunger value
        Hunger -= value / humansAliveCount;
    }


    void KillEveryOne()
    {
        //set the message string if gameover is due to hunger
        messageString = "Hunger Kills. You didn't eat enough food.";
        //kill every human, starting from the end of the array
        for (int i = 5; i >= 0; i--)
        {
            if(humans[i])
                humans[i].Die();
        }

    }

    public void OnHeroDead()
    {
        if (_gameOver)
        {
            return;
        }
        _gameOver = true;
        Invoke("GameOver", 3);
    }

    void GameOver()
    {
        Cursor.visible = true;
        SoundManager.instance.PlayAudio("GameOverSFX", 3);
        StopAllCoroutines();

        if (gameOverPanel)
        {
            //if messageString is empty display a random tip otherwise display the message
            if (messageString == "")
            {
                endGameMessage.text = "Tip: " + tipMessages[Random.Range(0, tipMessages.Length)];
            }
            else
            {
                endGameMessage.text = messageString;
            }

            gameOverPanel.SetActive(true);

        }


    }

    public void OnRetry()
    {
        //reload the current scene, if not loading already
        if (_loadingNextScene)
        {
            return;
        }
        _loadingNextScene = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnHumanDeath(bool isMedic)
    {

        if (isMedic)
        {
            medicDead = true;

        }

        humansAliveCount--;
    }



}
