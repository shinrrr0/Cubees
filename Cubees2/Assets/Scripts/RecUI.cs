using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class RecUI : MonoBehaviour
{
    private Color startColor;
    private Color endColor;
    private float currentTime = 0;
    private float timerTime;
    private float time;
    private float speed;
    // private int metronome_counter = 0;
    private AudioSource sound;

    private string timerTimeString = "";
    private NumberFormatInfo nfi;

    private float testTime = 0;
    
    public GameObject cube;
    public GameObject recPicture;
    public GameObject timerLabel;

    [HideInInspector] public bool isAnimationPlaying;
    

    void Start()
    {
        startColor = new Color(0, 0, 1, 0);
        endColor = new Color(0, 0, 1, 1);
        sound = gameObject.GetComponent<AudioSource>();
        nfi = new NumberFormatInfo();
        nfi.NumberDecimalSeparator = ":";
    }

    void Update() 
    {
        if (cube.GetComponent<Moving>().isRecording) {
            timerTime += Time.deltaTime;  
            timerTimeString = string.Format(nfi, "{0:f2}", timerTime);
            timerLabel.GetComponent<TMPro.TextMeshProUGUI>().text = timerTimeString;
        }
        else if (isAnimationPlaying) {
            StartCoroutine("ControllTimer");
        }
    }

    IEnumerator ChangeColor()
    {
        yield return null;
        recPicture.gameObject.GetComponent<Image>().color = Color.Lerp(startColor, endColor, (Mathf.Sin(currentTime - 1.5f) + 1f) / 2f);
        currentTime += Time.deltaTime * speed / 2;
        if ((currentTime * 180f / 3.14f) > 360) currentTime = 0;
        if (!cube.GetComponent<Moving>().isRecording && currentTime == 0)
        {
            isAnimationPlaying = false; 
            timerTime = 0;
            // metronome_counter = 0;
        } 
        else StartCoroutine("ChangeColor");
    }
    
    IEnumerator Metronome()
    {
        // yield return new WaitForSeconds(time - (time * 0.5f * (metronome_counter++ > 0? 0 : 1)));
        yield return new WaitForSeconds(time);
        if (cube.GetComponent<Moving>().isRecording && isAnimationPlaying) {
            sound.Play();
            StartCoroutine("Metronome");
        }
    }

    IEnumerator ControllTimer()
    {
        yield return null;
        timerLabel.gameObject.GetComponent<TMPro.TextMeshProUGUI>().color = Color.Lerp(startColor, endColor, (Mathf.Sin(currentTime - 1.5f) + 1f) / 2f);
        if ((currentTime * 180f / 3.14f) < 180) StartCoroutine("ControllTimer"); 
    }

    public void startChangingColor() {
        time = cube.GetComponent<Moving>().getTotalTime();
        speed = getSpeedFromTimerTickTime(time);
        isAnimationPlaying = true;
        StartCoroutine("ChangeColor");
        StartCoroutine("ControllTimer");
        if (isAnimationPlaying) StartCoroutine("Metronome");
    }

    public void stopChangingColor() {
        isAnimationPlaying = false;
    }

    private float getSpeedFromTimerTickTime(float time)
    {
        return 6.23f / time;
    }

}