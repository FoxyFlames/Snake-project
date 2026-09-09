using Unity.VisualScripting;
using UnityEngine;

/** handles */
public class LevelGrid 
{
    private Vector2Int food_grid_position;
    private GameObject food_game_object;
    private int width;
    private int height;
    private Snake snake;

    public LevelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    /** passes snake reference to grid */
    public void Setup(Snake snake)
    {
        this.snake = snake;

        SpawnFood();
    }

    /** randomises grid position then creates new food object with sprite while avoiding snake's body */
    private void SpawnFood()
    {
        do
        {
            food_grid_position = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (snake.GetFullSnakeBody().IndexOf(food_grid_position) != -1);

        food_game_object = new GameObject("Food", typeof(SpriteRenderer));
        food_game_object.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.food_sprite;
        food_game_object.transform.position = new Vector3(food_grid_position.x, food_grid_position.y);
    }

    /** checks if snake ate food and generates new food */
    public bool TrySnakeEatFood(Vector2Int snake_grid_position)
    {
        if (snake_grid_position == food_grid_position)
        {
            Object.Destroy(food_game_object);
            SpawnFood();
            GameHandler.AddScore();
            return true;
        }
        else
            return false;
    }

    /** checks if snake left the map */
    public void CheckGridPosition(Vector2Int grid_position)
    {
        if (grid_position.x < 0 || grid_position.x > 19 || grid_position.y < 0 || grid_position.y >19)
            snake.state = Snake.State.Dead;
    }
}
