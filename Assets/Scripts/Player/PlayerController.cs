using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private EquipmentMenu em;
    [SerializeField] private InventoryMenu im;

    private Rigidbody2D rb;

    [Header("Player Stats")]
    [SerializeField] private float baseSpeed = 1;
    [SerializeField] private float playerSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSpeed = baseSpeed;
    }

    void Update()
    {
        #region Player Movement

        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);
        bool up = Input.GetKey(KeyCode.W);
        bool down = Input.GetKey(KeyCode.S);

        float hSpeed = 0;
        float vSpeed = 0;

        // If there is no input for an axis, there is no movement along that axis
        // If there is simultaneous input of opposite directions, there is no movement along that axis
        // If there is simultaneous input of different axises, movement direction is combined

        if (!(left && right))
        {
            if (left)
                hSpeed = -playerSpeed;
            else if (right)
                hSpeed = playerSpeed;
            else
                hSpeed = 0;
        }

        if (!(up && down))
        {
            if (up)
                vSpeed = playerSpeed;
            else if (down)
                vSpeed = -playerSpeed;
            else vSpeed = 0;
        }

        rb.velocity = new Vector2(hSpeed, vSpeed);

        #endregion

        #region Player Menu

        if (Input.GetKeyDown(KeyCode.E))
        {
            em.Toggle();
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            im.Toggle();
        }       

        #endregion
    }
}
