using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject tutorial;

    public void GoTutorial()
    {
        tutorial.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}
