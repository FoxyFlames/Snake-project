using UnityEngine;

/** loads sprites for use when creating objects */
public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;

    private void Awake()
    {
        instance = this;
    }

    public Sprite food_sprite;
    public Sprite snake_head_sprite;
    public Sprite snake_body_sprite;
}
