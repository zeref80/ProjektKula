using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// It is only for UI objects like Text or Image
/// </summary>
public class PopupAnim : MonoBehaviour
{
    public float time = 0;
    float timeToEnd = 0;
    public bool onEndDissapear = true;

    //Fade:
    public bool fade = false;
    [ConditionalField("fade", compareValues: true)]
    public float fadeInTime = 0;
    [ConditionalField("fade", compareValues: true)]
    public float fadeOutTime = 0;

    //Scale:
    public bool scale = false;
    [ConditionalField("scale", compareValues: true)]
    public float scaleInTime = 0;
    [ConditionalField("scale", compareValues: true)]
    public Vector3 startScale;
    [ConditionalField("scale", compareValues: true)]
    public Vector3 endScale;
    [ConditionalField("scale", compareValues: true)]
    public float scaleOutTime = 0;

    private void Update()
    {
        timeToEnd -= Time.deltaTime;
        if(timeToEnd <= 0 && onEndDissapear)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        timeToEnd = time;
        if (fade)
        {
            if(fadeInTime + fadeOutTime > time)
            {
                Debug.LogError(name + ": 'fadeInTime + fadeOutTime' can't be greater than time");
            }
            else
            {
                Image image;
                Text text;
                if(TryGetComponent<Image>(out image))
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
                    LeanTween.imageColor(image.rectTransform, new Color(image.color.r, image.color.g, image.color.b, 1), fadeInTime).setOnComplete(() => AfterImageFade(image));
                }
                else if (TryGetComponent<Text>(out text))
                {
                    text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
                    LeanTween.textColor(text.rectTransform, new Color(text.color.r, text.color.g, text.color.b, 1), fadeInTime).setOnComplete(() => AfterTextFade(text));
                }
                else
                {
                    Debug.LogError("You can't use fade on this object: '" + name + "'");
                }
            }
        }

        if (scale)
        {
            if (scaleInTime + scaleOutTime > time)
            {
                Debug.LogError(name + ": 'scaleInTime + scaleOutTime' can't be greater than time");
            }
            else
            {
                transform.localScale = startScale;
                LeanTween.scale(gameObject, endScale, scaleInTime).setOnComplete(AfterScale);
            }
        }
    }

    void AfterTextFade(Text text)
    {   
        if(fadeOutTime > 0)
            LeanTween.textColor(text.rectTransform, new Color(text.color.r, text.color.g, text.color.b, 0), fadeOutTime).setDelay(time - fadeOutTime - fadeInTime);
    }

    void AfterImageFade(Image image)
    {
        if(fadeOutTime>0)
            LeanTween.imageColor(image.rectTransform, new Color(image.color.r, image.color.g, image.color.b, 0), fadeOutTime).setDelay(time - fadeOutTime - fadeInTime);
    }

    void AfterScale()
    {
        if(scaleOutTime > 0)
            LeanTween.scale(gameObject, startScale, scaleOutTime).setDelay(time - scaleInTime - scaleOutTime);
    }
}
