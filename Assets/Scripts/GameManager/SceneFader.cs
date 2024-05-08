using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public Image sceneTransion;
    public AnimationCurve curve;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    IEnumerator FadeIn()
    {
        float t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            sceneTransion.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
    }

    IEnumerator FadeOut(string scene)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 2;
            float a = curve.Evaluate(t);
            sceneTransion.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }

        SceneManager.LoadScene(scene);
    }
}

