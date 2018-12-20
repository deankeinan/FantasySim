using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character is all about what each GameObject 'Characterish' entity has or is shared accross everyone.
/// </summary>
public abstract class Character : MonoBehaviour
{

    [Header("Character")]
    public GameObject characterEntity;

    [Header("Status")]
    public bool canMove = true;
    public float alterSpeed = 1f;
    public bool canAttack = true;
    public bool isInteractable = true;

    [Header("Inverts")]
    // The invert movement for the X and Y. 1 for normal and -1 for opposite.
    public int playerInvertX = 1;
    public int playerInvertY = 1;

    [Header("Components")]
    public Light lightSource;
    public Collider2D characterCollider;
    public Animator characterAnimator;
    public Rigidbody2D characterRigidBody2D;

    [Header("Interaction")]
    public bool isInteracting = false;
    public GameObject interactionFocusTarget;

    [Header("Position")]
    public Transform characterTransform;
    public Vector2 characterPrevLocation;
    public Vector2 characterAmountMoved;

    [Header("Stats")]
    public float DefaultDamage = 0f;
    public float DefaultMoveSpeed = 1f;
    public float DefaultHealth = 3f;
    public float DefaultMaxHealth = 5f;
    public float DefaultMana = 20f;
    public float DefaultMaxMana = 20f;


    public float CurrentDamage;
    public float BonusDamage;

    public float CurrentMoveSpeed;
    public float BonusMoveSpeed;

    public float MaxHealth;
    public float CurrentHealth;

    public float MaxMana;
    public float CurrentMana;



    /// <summary>
    /// Increase the base damage.
    /// </summary>
    public void IncreaseBaseDamage(float amount)
    {
        // We add to our bonus variable to keep track of how much bonus we have.
        BonusDamage += amount;
    }

    /// <summary>
    /// Increase the base movement speed.
    /// </summary>
    public void IncreaseBaseMoveSpeed(float amount)
    {
        // We add to our bonus variable to keep track of how much bonus we have.
        BonusMoveSpeed += amount;
    }



    /// <summary>
    /// Add to our current health pool.  If we happen to go above our max health we set our current health
    /// at max health.
    /// </summary>
    public void AddHealth(float amount)
    {
        // IF we go above our max health.
        if (CurrentHealth + amount >= MaxHealth)
        {
            // Set our current health to our max health.
            CurrentHealth = MaxHealth;
        }
        else
        {
            // Just add to our current health pool.
            CurrentHealth += amount;
        }
    }

    /// <summary>
    /// Subtract from our current health pool.  If we happen to go below 0 we just set our current heaht to 0.
    /// </summary>
    public void SubtractHealth(float amount)
    {
        // IF this damaging attack reduces our HP to 0 or below which kills us,
        // ELSE this damaging attack didnt kill us so we just reduce the HP.
        if (CurrentHealth - amount <= 0f)
        {
            // We set our current health to 0.
            CurrentHealth = 0;
        }
        else
        {
            // Reduce our health.
            CurrentHealth -= amount;
        }
    }



    /// <summary>
    /// Get the default health.
    /// </summary>
    public float GetDefaultHealth()
    {
        return DefaultHealth;
    }

    /// <summary>
    /// Get the default max health.
    /// </summary>
    public float GetDefaultMaxHealth()
    {
        return DefaultMaxHealth;
    }

    /// <summary>
    /// Get the current.
    /// </summary>
    public float GetCurrentHealth()
    {
        return CurrentHealth;
    }

    /// <summary>
    /// Get the current max health.
    /// </summary>
    public float GetMaxHealth()
    {
        return MaxHealth;
    }

    /// <summary>
    /// Set the max health.
    /// </summary>
    public void SetMaxHealth(float newMaxHealth)
    {
        MaxHealth = newMaxHealth;
    }




    /// <summary>
    /// Get the default mana.
    /// </summary>
    public float GetDefaultMana()
    {
        return DefaultMana;
    }

    /// <summary>
    /// Get the default max mana.
    /// </summary>
    public float GetDefaultMaxMana()
    {
        return DefaultMaxMana;
    }


    /// <summary>
    /// Get the default damage.
    /// </summary>
    public float GetDefaultDamage()
    {
        return DefaultDamage;
    }

    /// <summary>
    /// Get the current damage.
    /// </summary>
    public float GetCurrentDamage()
    {
        return CurrentDamage;
    }

    /// <summary>
    /// Get the bonus damage.
    /// </summary>
    public float GetBonusDamage()
    {
        return BonusDamage;
    }

    /// <summary>
    /// Set the current damage.
    /// </summary>
    public void SetCurrentDamage(float newCurrentDamage)
    {
        CurrentDamage = newCurrentDamage;
    }

    /// <summary>
    /// Gets the default bonus damage.
    /// </summary>
    /// <returns>The default bonus damage.</returns>
    public float GetDefaultBonusDamage()
    {
        return DefaultDamage + BonusDamage;
    }




    /// <summary>
    /// Get the default movement speed.
    /// </summary>
    public float GetDefaultMoveSpeed()
    {
        return DefaultMoveSpeed;
    }

    /// <summary>
    /// Get the current movement speed.
    /// </summary>
    public float GetCurrentMoveSpeed()
    {
        return CurrentMoveSpeed;
    }

    /// <summary>
    /// Get the bonus movement speed.
    /// </summary>
    public float GetBonusMoveSpeed()
    {
        return BonusMoveSpeed;
    }

    /// <summary>
    /// Set the current movementspeed.
    /// </summary>
    public void SetCurrentMoveSpeed(float newMoveSpeed)
    {
        CurrentMoveSpeed = newMoveSpeed;
    }


    public float GetDefaultBonusMovementSpeed()
    {
        return DefaultMoveSpeed + BonusMoveSpeed;
    }

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }
}