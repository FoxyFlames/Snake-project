using UnityEngine;
using UnityEngine.UI;

/** handles the score screen */
public class ScoreWindow : MonoBehaviour
{
    private Text score_text;

    private void Awake()
    {
        score_text = transform.Find("ScoreText").GetComponent<Text>();
    }

    private void Update()
    {
        score_text.text = GameHandler.GetScore().ToString();
    }
}
