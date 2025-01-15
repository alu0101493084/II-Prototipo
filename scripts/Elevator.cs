using UnityEngine;

public class Elevator : MonoBehaviour
{
    public int[] raise_steps;
    private int current_step = 0;
    private float slide_out_timer = 0;

    void Start() {
        Score.scoreChanged += ScoreChanged;
        slide_out_timer = 1;
        transform.position -= Vector3.up * 30;

    }

    void ScoreChanged(int value) {
        if (raise_steps.Length <= current_step) return;
        if (value == raise_steps[current_step]) {
            current_step++;
            BlackSlider.SlideIn();
            slide_out_timer = 1;
        }
    }

    void Update() {
        if (slide_out_timer > 0) {
            slide_out_timer -= Time.deltaTime;
            if (slide_out_timer <= 0) {
                transform.position += Vector3.up * 30;
                BlackSlider.SlideOut();
            }
        }
    }
}
