using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
    void OnEnable()
    {
        foreach (PlayerComponent playerComponent in GetComponents<PlayerComponent>())
        {
            playerComponent.EnableComponent(this);
        }

    }

    void OnDisable()
    {
        foreach (PlayerComponent playerComponent in GetComponents<PlayerComponent>())
        {
            playerComponent.DisableComponent();
        }
    }
}




