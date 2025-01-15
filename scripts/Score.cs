using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static Score singleton;

    public delegate void ScoreMessage(int value);
    public static event ScoreMessage scoreChanged;
    public static event ScoreMessage scoreHundredMark;

    public int initial_score = 0;
    public int[] score_event_marks;
    public UnityEvent[] score_event_events;
    private int current_score_event_index = 0;

    private static int score_ = 0;
    public static int score {
        get => score_;
        set {
            if (scoreChanged != null) scoreChanged(value);
            if (score_ / 100 != value / 100) scoreHundredMark(value);
            score_ = value;
            text.text = "Score: " + value.ToString();

            if (singleton.score_event_marks.Length <= singleton.current_score_event_index) return;
            if (value == singleton.score_event_marks[singleton.current_score_event_index]) {
                singleton.score_event_events[singleton.current_score_event_index].Invoke();
                singleton.current_score_event_index++;
            }
        }
    }

    private static Text text;

    void Start() {
        score_ = initial_score;
        singleton = this;
        text = GetComponent<Text>();
    }
}
