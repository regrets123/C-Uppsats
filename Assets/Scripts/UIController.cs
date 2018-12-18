using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    Text objective;

    // Use this for initialization
    void Start()
    {

        objective = GameObject.Find("Objective").GetComponent<Text>();
        StartCoroutine(DisplayObjective(1f, objective));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator DisplayObjective(float time, Text objective)
    {
        StartCoroutine(FadeInText(time, objective));
        yield return new WaitForSeconds(5);
        StartCoroutine(FadeOutText(time, objective));
    }

    public IEnumerator FadeInText(float t, Text textToFade)
    {
        textToFade.color = new Color(textToFade.color.r, textToFade.color.g, textToFade.color.b, 0);

        while (textToFade.color.a < 1.0f)
        {
            textToFade.color = new Color(textToFade.color.r, textToFade.color.g, textToFade.color.b, textToFade.color.a + (Time.deltaTime / t));
            yield return null;
        }

    }

    public IEnumerator FadeOutText(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);

        while (i.color.a > 0)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
