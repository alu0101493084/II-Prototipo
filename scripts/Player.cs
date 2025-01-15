using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public AudioSource audio_source;

    public int max_health = 4;
    private int health;
    private float slide_timer = 0;
    public Text health_text;

    void Start() {
        health = max_health;
        health_text.text = "Health: " + health.ToString();
    }

    public void Damage() {
        health -= 1;
        health_text.text = "Health: " + health.ToString();
        if (health == 0) {
            BlackSlider.SlideIn();
            slide_timer = 2;
        }
        audio_source.Play();
    }

    void Update() {
        if (slide_timer > 0) {
            slide_timer -= Time.deltaTime;
            if (slide_timer <= 0) {
                SceneManager.LoadScene(0);
            }
        }
    }
}
