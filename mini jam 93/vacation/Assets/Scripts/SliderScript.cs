using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textBox;

    public void UpdatePercent(float val)
    {
        textBox.text = (val * 100).ToString("0") + "%";
    }
}
