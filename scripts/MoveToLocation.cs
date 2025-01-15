using UnityEngine;
using UnityEngine.Events;

public class MoveToLocation : MonoBehaviour
{
    private bool moving = false;
    private Vector3 initial_position = Vector3.zero;
    public Vector3 target_position = Vector3.zero;
    public float current_speed = 2;
    private float current_weight = 0;
    private Vector3 current_target_position;
    public UnityEvent finished;

    void Start() {
        initial_position = transform.position;
    }

    public void MoveTo() {
        moving = true;
        current_weight = 0;
        initial_position = transform.position;
        current_target_position = initial_position + target_position;
    }

    void FixedUpdate() {
        if (!moving) return;
        current_weight = Mathf.MoveTowards(current_weight, 1, current_speed * Time.fixedDeltaTime);
        transform.position = Vector3.Lerp(initial_position, current_target_position, current_weight);
        if (transform.position == current_target_position) {
            moving = false;
            finished.Invoke();
        };
    }
}
