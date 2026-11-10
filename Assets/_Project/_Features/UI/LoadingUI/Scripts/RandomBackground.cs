using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AspectRatioFitter), typeof(Image))]
public class RandomBackground : MonoBehaviour
{
    public Backgrounds database;

    void Start()
    {
        var image = GetComponent<Image>();
        var fitter = GetComponent<AspectRatioFitter>();

        int randomIndex  = Random.Range(0, database.backgrounds.Length);

        image.sprite = database.backgrounds[randomIndex].image;

        if (image.sprite != null)
        {
            float ratio = (float)image.sprite.rect.width / image.sprite.rect.height;
            fitter.aspectRatio = ratio;
        }

        fitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
    }
}
