using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class ZombieManager : MonoBehaviour
{
    public int initial_score_on_room;
    private List<int> send_horde_score_milestones = new List<int>();
    private int current_horde_index = 0;

    private List<Transform> hordes = new List<Transform>();
    

    void Start() {
        int current_base_score = initial_score_on_room + 1;
        foreach (Transform child in transform) {
            hordes.Add(child);
            send_horde_score_milestones.Add(current_base_score);
            current_base_score += child.childCount - 2;
        }

        Score.scoreChanged += ScoreChanged;
    }

    public void StartWalking() {
        foreach (Transform horde in hordes) {
            foreach (Transform zombie_tr in horde) {
                zombie_tr.GetComponent<Zombie>().ChasePlayer();
            }
        }
    }

    public void ScoreChanged(int value) {
        if (current_horde_index >= send_horde_score_milestones.Count) return;
        if (send_horde_score_milestones[current_horde_index] ==  value) {
            if (hordes[current_horde_index] == null) {
                current_horde_index++;
                return;
            }
            foreach (Transform zombie_tr in hordes[current_horde_index]) {
                if (zombie_tr == null) return;
                zombie_tr.GetComponent<Zombie>().Run();
            }
            current_horde_index++;
        }
    }
}
