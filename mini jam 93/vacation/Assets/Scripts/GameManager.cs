using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public float stress = 0f;
    public float productivity = 100f;
    public float coffee = 100f;
    public float minTime = 10f, maxTime = 15f;
    public int maxPaper = 6, curPapers = 0, score = 0;
    public bool gaming = false;
    public bool drinking = false;
    public bool forcedGaming = false;
    public bool curWorking = false;
    public bool watched = false;
    public bool lost = false;
    public TMPro.TextMeshProUGUI scoreText, flavorText, finalScoreText;

    public Transform bossSpawn, bossDesk, paperSpot, bossEnd, canvas;

    public GameObject boss, gameOverPanel;

    public MeshRenderer screen;

    public Material baseMat, vidMat;
    public VideoPlayer videoPlayer;

    public Slider coffeeSlider, productivitySlider, stressSlider;

    public Rigidbody deskRb;

    public Animator playerAnim;

    public UnityEvent lostEvent;

    public AudioClip crash;
    public AudioSource audioSource;

    public void Retry()
    {
        SceneManager.LoadScene(1);
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
    }

    public void AddScore()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateCoffee(float nc)
    {
        coffee = Mathf.Clamp(nc, 0f, 100f);
        coffeeSlider.value = coffee / 100f;
        if (coffee == 0f) Drink(false);
    }

    public void UpdateProductivity(float np)
    {
        productivity = Mathf.Clamp(np, 0f, 100f);
        productivitySlider.value = productivity / 100f;
        if (productivity == 0)
        {
            Lose("You were not productive enough! :(");
        }
    }
    public void UpdateStress(float ns)
    {
        stress = Mathf.Clamp(ns, 0f, 100f);
        stressSlider.value = stress / 100f;
        if (!forcedGaming && stress >= 80f) 
        {
            forcedGaming = true;
            Game(true);
        }
        if(forcedGaming && stress < 30f)
        {
            forcedGaming = false;
            Game(false);
        }
        if (stress == 100f) Lose("You got too stressed! :(");
    }

    public void Game(bool start)
    {
        if (start && !gaming)
        {
            gaming = true;
            screen.material = vidMat;
            videoPlayer.Play();
        }
        else if (!forcedGaming && gaming)
        {
            gaming = false;
            screen.material = baseMat;
            videoPlayer.Pause();
        }
    }

    public void Drink(bool start)
    {
        if (start && !drinking && coffee > 10f)
        {
            drinking = true;
        }
        else if(!start && drinking)
        {
            drinking = false;
        }
    }

    public void Lose(string message)
    {
        lostEvent.Invoke();
        playerAnim.enabled = true;
        deskRb.AddForce(new Vector3(0, 30, 100), ForceMode.Impulse);
        audioSource.PlayOneShot(crash);
        lost = true;
        finalScoreText.text = "Your score: " + score.ToString();
        flavorText.text = message;
        gameOverPanel.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (gaming) UpdateStress(stress - 0.25f);
        if (drinking)
        {
            UpdateCoffee(coffee - 0.1f);
            UpdateStress(stress - 0.1f);
        }
        UpdateStress(stress + curPapers * 0.005f);
    }
    private void Update()
    {
        if (!lost)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                //Debug.Log(gaming);
                Game(!gaming);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                Drink(!drinking);
            }
            if (gaming && watched)
            {
                Lose("You got caught gaming! :(");
            }
        }
    }
    private void Start()
    {
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "game.mp4");
        UpdateCoffee(coffee);
        UpdateProductivity(productivity);
        UpdateStress(stress);
        StartCoroutine(CoffeeRecharge());
        StartCoroutine(ProductivityDecrease());
        StartCoroutine(SendBoss(5f));
    }

    private IEnumerator CoffeeRecharge()
    {
        yield return new WaitForSeconds(3f);
        if(!drinking) UpdateCoffee(coffee + 1);
        StartCoroutine(CoffeeRecharge());
    }

    private IEnumerator ProductivityDecrease()
    {
        yield return new WaitForSeconds(0.5f);
        UpdateProductivity(productivity - 1f);
        StartCoroutine(ProductivityDecrease());
    }

    private IEnumerator SendBoss(float time)
    {
        yield return new WaitForSeconds(time);
        Instantiate(boss, bossSpawn.position, bossSpawn.rotation);
        maxTime = Mathf.Clamp(maxTime - 5f, 15f, 60f);
        minTime = Mathf.Clamp(minTime - 5f, 10f, 60f);
        StartCoroutine(SendBoss(Random.Range(minTime, maxTime)));
    }
}
