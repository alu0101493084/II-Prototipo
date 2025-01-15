using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class Gun : MonoBehaviour
{
    public AudioSource audio_source;

    public Transform camera_transform;
    public float shoot_distance;
    public Transform reticle;
    public float reticle_speed = 10;
    public GameObject bullet_prefab;

    private IShootable current_target = null;
    private Vector3 reticle_target_pos = Vector3.zero;
    private bool touching = false;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        EnhancedTouchSupport.Enable();
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += OnTouch;
    }

    void FixedUpdate() {
        RaycastHit hit_info;
        LayerMask mask = LayerMask.GetMask("shootable");
        if (!Physics.Raycast(camera_transform.position, camera_transform.forward, out hit_info, shoot_distance, mask)) {
            //reticle.gameObject.SetActive(false);
            current_target = null;
            return;
        }
        //reticle.gameObject.SetActive(true);
        reticle_target_pos = hit_info.point;

        current_target = hit_info.collider.GetComponent<IShootable>();
    }

    void Update() {
        //reticle.transform.position = Vector3.Lerp(reticle.transform.position, reticle_target_pos, Time.deltaTime * reticle_speed);
        if (Mouse.current.leftButton.isPressed && !touching) {
            OnTouch();
        }
        touching = Mouse.current.leftButton.isPressed;
    }

    void OnTouch(Finger action = null) {
        current_target?.Shoot();
        GameObject bullet_go = Instantiate(bullet_prefab);
        bullet_go.transform.position = reticle_target_pos;
        audio_source.Play();
    }
}
