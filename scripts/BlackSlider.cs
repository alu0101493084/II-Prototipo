using UnityEngine;

public class BlackSlider : MonoBehaviour
{
    public float slider_speed = 10;

    private static RectTransform rect_transform;
    private static bool sliding_in = true;

    void Start() {
        rect_transform = GetComponent<RectTransform>();
    }

    public static void SlideIn() {
        rect_transform.pivot = new Vector2(0.5f, 0);
        sliding_in = true;
    }

    public static void SlideOut() {
        rect_transform.pivot = new Vector2(0.5f, 1);
        sliding_in = false;
    }

    void Update() {
        if (sliding_in) {
            rect_transform.localScale = new Vector3(1, Mathf.Lerp(rect_transform.localScale.y, 1, Time.deltaTime * slider_speed), 1);
        } else {
            rect_transform.localScale = new Vector3(1, Mathf.Lerp(rect_transform.localScale.y, 0, Time.deltaTime * slider_speed), 1);
        }
    }
}
