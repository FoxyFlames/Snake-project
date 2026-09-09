using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public enum State
    {
        Alive,
        Dead
    }

    public State state;
    private Vector2Int grid_move_direction;
    private Vector2Int grid_position;
    private float grid_move_timer;
    private float grid_move_timer_max;
    private LevelGrid level_grid;
    private int snake_body_size;
    private List<Vector2Int> snake_move_position_list;
    private List<SnakeBodyPart> snake_body_part_list;


    /** passes grid reference to snake */
    public void Setup(LevelGrid level_grid)
    {
        this.level_grid = level_grid;
    }

    private void Awake()
    {
        grid_position = new Vector2Int(10, 10);
        grid_move_timer_max = .4f;
        grid_move_timer = grid_move_timer_max;
        grid_move_direction = new Vector2Int(1, 0);

        snake_move_position_list = new List<Vector2Int>();
        snake_body_size = 0;

        snake_body_part_list = new List<SnakeBodyPart>();
        state = State.Alive;
    }


    private void Update()
    {
        switch (state)
        {
            case State.Alive:
                HandleInput();
                HandleGridMovement();
                break;
            case State.Dead:
                break;
        }
    }

    /** intakes input to change snake direction */
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (grid_move_direction.y != -1)
            {
                grid_move_direction.x = 0;
                grid_move_direction.y = 1;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (grid_move_direction.y != 1)
            {
                grid_move_direction.x = 0;
                grid_move_direction.y = -1;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (grid_move_direction.x != -1)
            {
                grid_move_direction.x = 1;
                grid_move_direction.y = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (grid_move_direction.x != 1)
            {
                grid_move_direction.x = -1;
                grid_move_direction.y = 0;
            }
        }
    }
    
    /** moves snake in current direction, rotates sprite and sends snake position to grid */
    private void HandleGridMovement()
    {
        grid_move_timer += Time.deltaTime;
        if (grid_move_timer >= grid_move_timer_max)
        {
            grid_move_timer -= grid_move_timer_max;

            snake_move_position_list.Insert(0, grid_position);
            grid_position += grid_move_direction;

            level_grid.CheckGridPosition(grid_position);

            bool snake_ate_food = level_grid.TrySnakeEatFood(grid_position);
            if (snake_ate_food)
            {
                /* if snake ate food, grow body */
                snake_body_size++;
                CreateSnakeBodyPart();
            }

            if (snake_move_position_list.Count >= snake_body_size + 1)
            {
                snake_move_position_list.RemoveAt(snake_move_position_list.Count - 1);
            }

            foreach (SnakeBodyPart snake_body_part in snake_body_part_list)
            {
                Vector2Int snake_body_part_grid_position = snake_body_part.GetGridPosition();
                if (grid_position == snake_body_part_grid_position)
                {
                    //game over
                    state = State.Dead;
                }
            }

            transform.position = new Vector3(grid_position.x, grid_position.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(grid_move_direction) - 90);

            UpdateSnakeBodyParts();
        }
    }

    /** creates snake body part */
    private void CreateSnakeBodyPart()
    {
        snake_body_part_list.Add(new SnakeBodyPart(snake_body_part_list.Count));
    }

    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snake_body_part_list.Count; i++)
        {
            snake_body_part_list[i].SetGridPosition(snake_move_position_list[i]);
        }
    }

    /** changes movement vector into angle to rotate sprite */
    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2Int GetGridPosition()
    {
        return grid_position;
    }

    /** returns full list of positions of the snake */
    public List<Vector2Int> GetFullSnakeBody()
    {
        List<Vector2Int> grid_postition_list = new List<Vector2Int>() { grid_position };
        grid_postition_list.AddRange(snake_move_position_list);
        return grid_postition_list;
    }

    /** handles the creation of body parts and sprites */
    private class SnakeBodyPart
    {
        private Vector2Int grid_position;
        private Transform transform;

        public SnakeBodyPart(int body_index)
        {
            GameObject snake_body_game_object = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snake_body_game_object.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snake_body_sprite;
            snake_body_game_object.GetComponent<SpriteRenderer>().sortingOrder = -body_index;
            transform = snake_body_game_object.transform;
        }

        /** sets position of body part*/
        public void SetGridPosition(Vector2Int grid_position)
        {
            this.grid_position = grid_position;
            transform.position = new Vector3(grid_position.x, grid_position.y);
        }

        public Vector2Int GetGridPosition()
        {
            return grid_position;
        }
    }
}
