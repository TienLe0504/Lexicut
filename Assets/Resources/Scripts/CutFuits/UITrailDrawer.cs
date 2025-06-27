using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UITrailDrawer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public enum TrailColor
    {
        Red,
        Yellow,
        Orange,
        Green
    }

    [Header("Trail Settings")]
    public TrailColor trailColor = TrailColor.Red;
    public GameObject trailPrefab;
    public RectTransform trailParent;
    public float spawnInterval = 0.05f;
    public float trailLifetime = 0.4f;

    private bool isDrawing = false;
    private float timer = 0f;
    private Color startColor;
    private Color endColor = Color.white;

    void Start()
    {
        startColor = GetColorFromEnum(trailColor);
    }

    void Update()
    {
        if (!isDrawing) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnTrail(Input.mousePosition);
            timer = 0f;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDrawing = true;
        timer = spawnInterval; // tạo dấu đầu tiên ngay
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDrawing = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Không cần xử lý gì ở đây vì Update sẽ tự spawn theo khoảng thời gian
    }

    void SpawnTrail(Vector3 position)
    {
        GameObject dot = Instantiate(trailPrefab, trailParent);
        dot.transform.position = position;

        Image img = dot.GetComponent<Image>();
        if (img != null)
        {
            img.color = startColor;
            StartCoroutine(FadeToColor(img, startColor, endColor));
        }
    }

    IEnumerator FadeToColor(Image img, Color fromColor, Color toColor)
    {
        float time = 0;
        while (time < trailLifetime)
        {
            float t = time / trailLifetime;
            img.color = Color.Lerp(fromColor, toColor, t);
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(img.gameObject);
    }

    Color GetColorFromEnum(TrailColor color)
    {
        switch (color)
        {
            case TrailColor.Red: return Color.red;
            case TrailColor.Yellow: return Color.yellow;
            case TrailColor.Orange: return new Color(1f, 0.5f, 0f);
            case TrailColor.Green: return Color.green;
            default: return Color.white;
        }
    }
}
