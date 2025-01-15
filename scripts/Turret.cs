using UnityEngine;

public class Turret : MonoBehaviour, IShootable
{
    public bool following_player = false;
    public Transform player_transform;
    public Player player;
    public Transform gun_transform;

    public GameObject charge_ball;
    private Transform charge_ball_transform;
    private Material charge_ball_material;

    public GameObject shoot_line;
    private LineRenderer shoot_line_renderer;
    private Material shoot_line_material;
    public float shoot_linger_duration = 0.5f;
    private float shoot_linger_timer = 0;

    public float shoot_cooldown = 1f;
    public float shoot_charge_time = 2f;
    private float shoot_charging = 0f;
    public float shoot_reloading = 0f;

    private bool dead = false;
    public int max_health = 3;
    private int health = 0;

    public AudioSource audio_source_shoot;
    public AudioClip charge_audio;
    public AudioClip shoot_audio;

    public AudioSource audio_source_damage;
    public AudioClip[] damage_clips;

    void Start() {
        health = max_health;

        charge_ball_transform = charge_ball.transform;
        charge_ball_material = charge_ball.GetComponent<Renderer>().material;
        shoot_line_renderer = shoot_line.GetComponent<LineRenderer>();
        shoot_line_material = shoot_line_renderer.material;
    }

    public void FollowPlayer() {
        following_player = true;
        shoot_reloading = Random.Range(0f, 0.5f);
    }

    void Update() {
        if (shoot_linger_timer > 0) {
            shoot_linger_timer -= Time.deltaTime;
        }
        shoot_line_material.color = Color.Lerp(new Color(1f, 0, 0, 0f), new Color(1f, 0, 0, 1f), shoot_linger_timer/shoot_linger_duration);

        if (following_player) {
            if (dead) {
                Quaternion targetRotation = Quaternion.Euler(80, 90, 0);
                gun_transform.rotation = Quaternion.Slerp(gun_transform.rotation, targetRotation, 1 * Time.deltaTime);
                return;
            } else {
                Quaternion targetRotation = Quaternion.LookRotation(player_transform.position - gun_transform.position);
                targetRotation *= Quaternion.Euler(Vector3.right * -90);
                gun_transform.rotation = Quaternion.Slerp(gun_transform.rotation, targetRotation, 1 * Time.deltaTime);
            }

            if (shoot_charging > 0 && shoot_reloading <= 0) {
                shoot_charging -= Time.deltaTime;
                if (shoot_charging <= 0) {
                    shoot_charging = shoot_charge_time;
                    shoot_reloading = shoot_cooldown;
                    Fire();
                }
            }
        } else {
            shoot_charging = shoot_charge_time;
        }

        if (shoot_reloading > 0) {
            shoot_reloading -= Time.deltaTime;
            if (shoot_reloading <= 0) {
                audio_source_shoot.clip = charge_audio;
                audio_source_shoot.Play();
                shoot_reloading = 0;
                shoot_charging = shoot_charge_time;
            }
        }


        charge_ball_material.color = Color.Lerp(new Color(1f, 0, 0, 1f), new Color(1f, 0, 0, 0f), shoot_charging/shoot_charge_time);
        charge_ball_transform.localScale = Vector3.Lerp(Vector3.one*30, Vector3.one*10, shoot_charging/shoot_charge_time);

    }

    public void Shoot() {
        if (dead) return;
        health -= 1;
        audio_source_damage.clip = damage_clips[health];
        audio_source_damage.Play();
        
        if (health <= 0) {
            dead = true;
            if (audio_source_shoot.clip == charge_audio) audio_source_shoot.Stop();
            charge_ball.SetActive(false);
            Score.score += 1;
        }
    }

    public void Fire() {
        audio_source_shoot.clip = shoot_audio;
        audio_source_shoot.Play();
        shoot_linger_timer = shoot_linger_duration;
        shoot_line_renderer.SetPosition(0, charge_ball_transform.position);
        shoot_line_renderer.SetPosition(1, player_transform.position);
        player.Damage();
    }
}
