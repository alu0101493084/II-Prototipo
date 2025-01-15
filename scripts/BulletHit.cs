using UnityEngine;

public class BulletHit : MonoBehaviour
{
    private Material material;

    private float fade_time = 0.5f;
    private float timer = 0f;
    private float target_scale = 1f;

    void Start() {
        timer = fade_time;
        material = GetComponent<Renderer>().material;
        transform.localScale = Vector3.one * 0.03f;
    }

    void Update() {
        material.color = Color.Lerp(new Color(1f, 1f, 0f, 1f), new Color(1f, 1f, 0f, 0f), 1-timer/fade_time);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * target_scale, Time.deltaTime * 20);

        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }
    }
}
