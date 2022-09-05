using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun.Demo.PunBasics;

public class TimingUp : MonoBehaviour
{

    int timeInSeconds;
    public string finalScore;

    [SerializeField] TMP_Text timerText;
    //OPTIONAL IMPLEMENTATION: use a signleton pattern and do not destroy keyword.
    public static TimingUp instance;
    private void Awake()
    {
        #region Singleton pattern
        Debug.Log("script is reffrencing the instance (TimingUp)"); 
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        #endregion
    }

    #region subscribe and unsubscribe
    private void OnEnable()
    {
        Debug.Log("script is being activated");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("sceneloading");
        timerText = GameObject.FindGameObjectWithTag("TimerText").GetComponent<TMP_Text>();
        if(scene.buildIndex == 0)
        {
            //update your score board
            timerText.text = finalScore;
        }
        else
        {
            Debug.Log("Not in launcher scene");
            StopCoroutine(Timer());
            StartCoroutine(Timer());
        }
    }

    public IEnumerator Timer()
    {
        Debug.Log("Coroutine started");
        timeInSeconds = 0;
        var minutes = Mathf.Floor(timeInSeconds / 60).ToString("00");
        var seconds = (timeInSeconds % 60).ToString("00");
        timerText.text = minutes + " : " + seconds;

        while (SceneManager.GetActiveScene().buildIndex != 0)
        {
            timeInSeconds += 1;

            minutes = Mathf.Floor(timeInSeconds / 60).ToString("00");
            seconds = (timeInSeconds % 60).ToString("00");
            timerText.text = minutes + " : " + seconds;
            finalScore = timerText.text; //passing this above info through for the launcher screens "finalScore". i must put timerText.text as it's TMP and wont read otherwise
            yield return new WaitForSeconds(1);
        }
        yield return null;
    }

}
