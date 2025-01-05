using UnityEngine;

[RequireComponent(typeof(Player))]
public abstract class PlayerComponent : MonoBehaviour
{
    public Player player;
    public virtual void EnableComponent(Player player)
    {
        this.player = player;
    }
    public virtual void DisableComponent()
    {
        player = null;
    }
}