using UnityEngine;
public class Target : MonoBehaviour, IShootable
{
    private bool hit = false;
    private Vector3 velocity = Vector3.up * 3;
    private MeshCollider mesh_collider;
    private AudioSource audio_source;

    void Start() {
        audio_source = GetComponent<AudioSource>();
        mesh_collider = GetComponent<MeshCollider>();
    }

    void Update() {
        if (!hit) return;
        velocity.y -= Time.deltaTime * 8;
        transform.position += velocity * Time.deltaTime;
        if (velocity.y > 100) Destroy(gameObject);
    }

    public void Shoot()
    {
        if (hit) return;
        hit = true;
        Score.score = Score.score + 1;
        mesh_collider.enabled = false;
        audio_source.Play();
    }
}
