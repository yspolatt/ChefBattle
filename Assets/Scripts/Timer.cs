using System.Collections;
using TMPro;
using UnityEngine;
public class Timer: MonoBehaviour
{
    public float timeRemaining = 300f;

    public TextMeshProUGUI timeText;



    void Start()
    {   
        
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {   
       
        while (timeRemaining > 0)
        {   
            int minutes = Mathf.FloorToInt(timeRemaining / 60F);
            int seconds = Mathf.FloorToInt(timeRemaining - minutes * 60);

            yield return new WaitForSeconds(1);
            timeRemaining--;
            
            timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }

        // Game Over
    }
}