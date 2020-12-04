using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutUI : MonoBehaviour
{
    [SerializeField]
    public GameObject blackOutSquare;

    //void Update()
    //{
        
    //    if (Input.GetKeyDown(KeyCode.B))
    //    {
    //        StartCoroutine(FadeBlackOutSquare());
    //    }

    //    if (Input.GetKeyDown(KeyCode.O))
    //    {
    //        StartCoroutine(ShowBlackOutSquare());
    //    }
        
    //}

    /// <summary>
    /// Fades the screen to black when called. FadeSpeed tells how quickly it happens.
    /// </summary>
    public IEnumerator FadeBlackOutSquare(bool fadeToBlack = true, int fadeSpeed = 1)
    {
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeToBlack)
        {
            while (blackOutSquare.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
    }

    /// <summary>
    /// Fades away from the black when called. FadeSpeed tells how quickly it happens.
    /// </summary>
    public IEnumerator ShowBlackOutSquare(bool fadeOutBlack = true, int fadeSpeed = 3)
    {
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;

        if (fadeOutBlack)
        {
            while (blackOutSquare.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackOutSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
    }

    public void StartFadeOut(bool fadeToBlack = true, int fadeSpeed = 1)
    {
	    StartCoroutine(FadeBlackOutSquare(fadeToBlack, fadeSpeed));
    }

    public void StartFadeIn(bool fadeToBlack = true, int fadeSpeed = 3)
    {
	    StartCoroutine(FadeBlackOutSquare(fadeToBlack, fadeSpeed));
    }
}
