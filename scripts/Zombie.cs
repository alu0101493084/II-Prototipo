using UnityEngine;

public class Zombie : MonoBehaviour, IShootable
{
    public bool chasing_player = false;

    private Rigidbody rb;
    public float walk_force = 2000;
    public float run_force = 7000;
    public bool running = false;
    private bool attacking = false;
    public float attack_range = 1f;

    private Animator animator;
    private bool dead = false;

    public float attack_cd = 1.5f;
    private float attack_timer = 0f;

    private Transform player_transform;
    private Player player;

    void Start() {
        rb = GetComponent<Rigidbody>();

        GameObject player_go = GameObject.FindWithTag("Player");
        player = player_go.GetComponent<Player>();
        player_transform = player_go.transform;
        animator = GetComponent<Animator>();

        if (chasing_player) {
            string walk_type = Random.Range(0, 1) > 0.5 ? "" : "1";
            animator.Play("Base Layer.Z_Walk" + walk_type + "_InPlace");
        }
    }

    public void ChasePlayer() {
        chasing_player = true;
        string walk_type = Random.Range(0, 1) > 0.5 ? "" : "1";
        animator.Play("Base Layer.Z_Walk" + walk_type + "_InPlace");
    }

    public void Run() {
        if (dead) return;
        running = true;
        animator.Play("Base Layer.Z_Run_InPlace");
    }

    void FixedUpdate() {
        if (dead) return;
        
        if (chasing_player) {
            Vector3 dir_to_player = player_transform.position - transform.position;
            dir_to_player.y = 0;
            dir_to_player = dir_to_player.normalized;

            rb.AddForce(dir_to_player * (running ? run_force : walk_force) * Time.fixedDeltaTime);
            transform.LookAt(transform.position + dir_to_player);
        }

        if (attacking && attack_timer <= 0f) {
            animator.Play("Base Layer.Z_Attack");
            player.Damage();
            attack_timer = attack_cd;
        }

        if (attack_timer > 0) {
            attack_timer -= Time.fixedDeltaTime;
        }

        if (Vector3.Distance(player_transform.position, transform.position) < attack_range) {
            chasing_player = false;
            attacking = true;
        }
    }

    public void Shoot() {
        GetComponent<Collider>().enabled = false;
        chasing_player = false;
        rb.isKinematic = true;
        animator.Play("Base Layer.Z_FallingBack");
        dead = true;
        Score.score += 1;
        GetComponent<AudioSource>().Play();
    }
}
