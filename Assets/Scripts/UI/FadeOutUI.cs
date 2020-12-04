using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutUI : MonoBehaviour
{
    [SerializeField]
    public GameObject blackOutSquare;

    /// <summary>
    /// Fades the screen to black when called. FadeSpeed tells how quickly it happens.
    /// </summary>
    public IEnumerator FadeBlackOutSquare(float fadeSpeed = 1)
    {
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;

        while (blackOutSquare.GetComponent<Image>().color.a < 1)
        {
            fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackOutSquare.GetComponent<Image>().color = objectColor;
            yield return null;
        }
        
    }

    /// <summary>
    /// Fades away from the black when called. FadeSpeed tells how quickly it happens.
    /// </summary>
    public IEnumerator ShowBlackOutSquare(float fadeSpeed = 1)
    {
        Color objectColor = blackOutSquare.GetComponent<Image>().color;
        float fadeAmount;

        while (blackOutSquare.GetComponent<Image>().color.a > 0)
        {
            fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackOutSquare.GetComponent<Image>().color = objectColor;
            yield return null;
        }
        
    }

    public void FadeToBlack(float fadeSpeed = 1)
    {
	    StartCoroutine(FadeBlackOutSquare(fadeSpeed));
    }

    public void FadeFromBlack(float fadeSpeed = 1)
    {
	    StartCoroutine(ShowBlackOutSquare(fadeSpeed));
    }
}
