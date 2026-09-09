using UnityEngine;




public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;

    private static int score;
    [SerializeField] private Snake snake;
    private LevelGrid level_grid;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        level_grid = new LevelGrid(20, 20);
        score = 0;

        snake.Setup(level_grid);
        level_grid.Setup(snake);
    }

    public static int GetScore()
    {
        return score;
    }

    public static void AddScore()
    {
        score += 100;
    }
}
