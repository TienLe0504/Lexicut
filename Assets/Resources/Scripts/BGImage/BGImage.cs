using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGImage : MonoBehaviour
{
    // Start is called before the first frame update
    private Image bgImage;
    public Canvas canvas;
    void Start()
    {
        bgImage = GetComponent<Image>();
        float width = canvas.GetComponent<RectTransform>().rect.width;
        float height = canvas.GetComponent<RectTransform>().rect.height;
        RectTransform rt = bgImage.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(width, height);
    }
    public void ShowImage(Sprite sprite)
    {
        if (bgImage != null && sprite != null)
        {
            bgImage.sprite = sprite;
            bgImage.enabled = true;
        }
        
    }
}
