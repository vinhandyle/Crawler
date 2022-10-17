using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player.
/// </summary>
public class PlayerController : Character
{
    [SerializeField] private EquipmentMenu em;
    [SerializeField] private InventoryMenu im;

    [Header("Player Stats")]
    [SerializeField] private float baseSpeed = 1;

    protected override void SetBaseInventory()
    {
        base.SetBaseInventory();

        inventory.AddItem(new GoldCoin(1000));
        inventory.AddItem(new LevelItem(200));
        inventory.AddItem(new GodSword_0());
        inventory.AddItem(new GodSword_1());
        inventory.AddItem(new GodSword_2());
        inventory.AddItem(new Fist());
        inventory.AddItem(new Arrow(100));
        inventory.AddItem(new Fist());
        inventory.AddItem(new Potion(25));
        inventory.AddItem(new FistTech_0());
        inventory.AddItem(new SlashTech_0());
        inventory.AddItem(new Fireball());
        inventory.AddItem(new Heal());
        inventory.AddItem(new GreatHeal());
        inventory.AddItem(new CosmicHelmet());
        inventory.AddItem(new CosmicChestplate());
        inventory.AddItem(new TankRing());
        inventory.AddItem(new DragonMaterial(33));
        //inventory.AddItem(new AccessoryBag(1));       
    }

    protected override void SetBaseEquipment()
    {
        equipment.FullEquipWeapon<Fist>();
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
        float playerSpeed = baseSpeed * profile.speedMult;

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
