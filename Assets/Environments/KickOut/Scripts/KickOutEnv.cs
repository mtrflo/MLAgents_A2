using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class KickOutEnv : MonoBehaviour
{
    public bool isGameStarted = false;
    public int delayTime;
    public TextMeshProUGUI timer_text;
    Coroutine coroutine;
    public void LevelStarted()
    {
        isGameStarted = false;
        
        if (coroutine == null)
            coroutine = StartCoroutine(DelayedStart());
    }
    IEnumerator DelayedStart()
    {
        print("aasdasd");
        for (int i = 0; i < delayTime; i++)
        {
            timer_text.text = (delayTime - i).ToSafeString();
            yield return new WaitForSeconds(1);
        }

        isGameStarted = true;
        coroutine = null;
    }
}
