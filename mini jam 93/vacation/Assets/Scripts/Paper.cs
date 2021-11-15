using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Paper : MonoBehaviour
{
    public GameObject slider, sliderI;
    private bool created = false;
    private Slider sliderC;
    private GameManager gameManager;
    private void OnMouseDown()
    {
        if (!created && !gameManager.curWorking && !gameManager.lost)
        {
            sliderI = Instantiate(slider, Input.mousePosition, slider.transform.rotation);
            sliderI.transform.SetParent(GameManager.Instance.canvas);
            sliderC = sliderI.GetComponent<Slider>();
            created = true;
            gameManager.curWorking = true;
        }
        
        //sliderI.transform.position = Input.mousePosition;
    }
    private void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.curPapers++;
    }
    private void FixedUpdate()
    {
        if (created)
        {
            sliderC.value += 0.01f;
            if (sliderC.value == 1f)
            {
                gameManager.curPapers--;
                gameManager.AddScore();
                gameManager.curWorking = false;
                gameManager.UpdateProductivity(gameManager.productivity + 10);
                gameManager.UpdateStress(gameManager.stress + 10);
                Destroy(sliderI);
                Destroy(gameObject);
            }
        }
        
    }
}
