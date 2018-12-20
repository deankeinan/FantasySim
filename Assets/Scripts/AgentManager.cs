using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class AgentManager : Character
    {

        [Header("Misc")]
        public float MainAttackCooldown = 0.2f;
        public bool canBeJolted;
        public bool currentlyJolted = false;
        public float hitAnimationTime = 0.2f;


        void Awake()
        {
            // The transform of the Character Entity for referencing.
            characterTransform = characterEntity.GetComponent<Transform>();
            // Get the Character Collider2D.
            characterCollider = characterEntity.GetComponent<Collider2D>();
            // Get the Animator Component.
            characterAnimator = characterEntity.GetComponent<Animator>();
            // Get the Rigidbody2D Component.
            characterRigidBody2D = characterEntity.GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            // IF there is a animator on the Character.
            if (characterAnimator != null)
            {

            }
        }

        void Update()
        {
            // Find out how much the character has moved.
            characterAmountMoved = (Vector2)characterTransform.position - characterPrevLocation;

            // IF the character has an animation set and ready to go.
            if (characterAnimator != null)
            {
                // IF there is an Action Key Dialogue currently running,
                // ELSE IF the character can move.
                if (isInteracting)
                {
                    // Handle the direction the NPC is looking.
                    //NPCLookDirection ();
                }
                else if (canMove)
                {
                    // Play the animation.
                    PlayAnimation(characterAmountMoved.x, characterAmountMoved.y);
                }
            }

            // Set where we were.
            characterPrevLocation = characterTransform.position;
        }



        /// <summary>
        /// Method to decide which way this NPC should be facing for our animation purposes.
        /// </summary>
        private void NPCLookDirection()
        {
            Transform focTransform = interactionFocusTarget.transform;
            FourDirectionAnimation(focTransform.position.x - characterTransform.position.x, focTransform.position.y - characterTransform.position.y, characterAnimator);

        }

        /// <summary>
        /// Method that sets our animation variables based on our 2 parameters "hor" and "vert".
        /// </summary>
        private void PlayAnimation(float hor, float vert)
        {

            FourDirectionAnimation(hor, vert, characterAnimator);

        }

        /// <summary>
        /// Everything you want to happen when the character dies.
        /// </summary>
        private void Death()
        {
            // Set our animation variables.
            //SetAnimationsDead(characterAnimator);
        }


        /// <summary>
        /// Method that handles our knockbacks.  characterEntity will be the entity that will be getting the knockback.
        /// </summary>
        public void Knockback(Transform otherTransform, float joltAmount)
        {
            // Get the relative position.
            Vector2 relativePos = characterEntity.transform.position - otherTransform.position;
            // Get the rigidbody2D
            Rigidbody2D charRigid = characterEntity.GetComponent<Rigidbody2D>();
            // Stop the colliding objects velocity.
            charRigid.velocity = Vector3.zero;
            // Apply knockback.
            charRigid.AddForce(relativePos.normalized * joltAmount, ForceMode2D.Impulse);
        }

        /// <summary>
        /// Method that handles everything we want when we want the character to not be in control.  Based on our hitAnimationTime
        /// dictates how long we will not be in control for when we call this method.
        /// </summary>
        private IEnumerator NoCharacterControl()
        {
            // Make the player not be able to control the character while the knockback is happening.
            canMove = false;
            // We are currently being knockbacked.
            currentlyJolted = true;
            // Wait for 'HitAnimationTime' before being able to control the character again.
            yield return new WaitForSeconds(hitAnimationTime);
            // Stop the knockback.
            characterEntity.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            // We can now move the character.
            canMove = true;
            // We are not being jolted anymore.
            currentlyJolted = false;
        }


        public void ChangeSortingLayer(string sortLayer, int sortLayerOrder)
        {
            // Change the Sort Layer and Order Number.
            GetComponent<SpriteRenderer>().sortingLayerName = sortLayer;
            GetComponent<SpriteRenderer>().sortingOrder = sortLayerOrder;
        }

    public void SetAnimationsIdle(Animator anim)
    {
        anim.SetBool("IsIdle", true);
        anim.SetBool("IsMoving", false);
    }

    /// <summary>
    /// Assign our animation variables to handle our Moving state.
    /// </summary>
    public void SetAnimationsWalk(Animator anim)
    {
        // We are moving.
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsMoving", true);
        // We cannot craft while moving.
        anim.SetBool("IsCrafting", false);
    }

    public void FourDirectionAnimation(float moveHorizontal, float moveVertical, Animator anim)
    {
        // IF we are moving we set the animation IsMoving to true,
        // ELSE we are not moving.
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            // Set walk animations.
            SetAnimationsWalk(anim);
        }
        else
        {
            // Set idle animations.
            SetAnimationsIdle(anim);
            // We leave if our player is not moving as we don't want to give access to looking in other directions.
            return;
        }

        // IF we are wanting to go in the positive X direction,
        // ELSE IF we are wanting to move in the negative X direction,
        // ELSE IF we are wanting to move in the negative Y direction,
        // ELSE IF we are wanting to move in the positive Y direction.
        if (moveHorizontal > 0 && Mathf.Abs(moveVertical) <= Mathf.Abs(moveHorizontal))
        {
            anim.SetInteger("Direction", 4);

        }
        else if (moveHorizontal < 0 && Mathf.Abs(moveVertical) <= Mathf.Abs(moveHorizontal))
        {
            anim.SetInteger("Direction", 2);

        }
        else if (moveVertical < 0 && Mathf.Abs(moveVertical) > Mathf.Abs(moveHorizontal))
        {
            anim.SetInteger("Direction", 3);

        }
        else if (moveVertical > 0 && Mathf.Abs(moveVertical) > Mathf.Abs(moveHorizontal))
        {
            anim.SetInteger("Direction", 1);
        }
    }


}
