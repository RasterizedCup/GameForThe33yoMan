using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class FilHealth : HealthLogicBase
{
    // Start is called before the first frame update
    public BoxCollider2D FilHitBox;
    public GameObject DeathAnimator;
    public SpriteRenderer FilSprite;
    public float deathDuration;
    public static bool isDead;
    float currTime;
    float xSize, ySize, xOffset, yOffset;
    void Start()
    {
        xSize = FilHitBox.size.x + .1f;
        ySize = FilHitBox.size.y + .1f;
        xOffset = FilHitBox.offset.x;
        yOffset = FilHitBox.offset.y;
        CurrentHealth = MaxHealth;
        handleDeath = false;
    }

    // Update is called once per frame
    void Update()
    {
        DEBUG_DEATH();
        checkForCharacterDeath();
    }
    void DEBUG_DEATH()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CurrentHealth = 0;
        }
    }
    void checkForCharacterDeath()
    {
        if (CurrentHealth <= 0 && !handleDeath)
        {
            currTime = Time.time;
            handleDeath = true;
            isDead = true;
        }

        if (handleDeath)
        {
            FilSprite.color = new Color(FilSprite.color.r, FilSprite.color.g, FilSprite.color.b, 0);
            if (!DeathAnimator.active)
            {
                DeathAnimator.active = true;
                DeathCounterIncrement.deathCount++;
            }
           
            // disable motion and everything else
            if (currTime + deathDuration < Time.time) // separate death time?
            {
                FilSprite.color = new Color(FilSprite.color.r, FilSprite.color.g, FilSprite.color.b, 1);
                DeathAnimator.active = false;
                handleDeath = false;
                // teleport to nearest checkpoint, rest health
                CurrentHealth = MaxHealth;

                GameObject.Find("Bubble Player").transform.position = CheckpointTracker.currentCheckpoint;

                // handle CM cam relocation for tele scene
                if (SceneManager.GetActiveScene().name == "Level 1-1")
                {
                    GameObject.Find("ObjectOmniController").GetComponent<EnvToggle>().handleBoundarySwap(CheckpointTracker.currentCheckpointName);
                }
                isDead = false;
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SnailRegion"))
            Snailian.isSnailianAllowed = true;

        // TODO: get tag dictionary for enemy weapons
        if (collision.CompareTag("Missile") && !this.gameObject.CompareTag("PlayerInvis"))
        {
            // all colliders are child objects tied to sprite angle for weapons
            // we must get the parent component to get Weapon logic -> damage
            CurrentHealth -= collision.gameObject.GetComponentInParent<WeaponLogicBase>().damage;
            if (CurrentHealth <= 0)
            {
                currTime = Time.time;
                handleDeath = true; // set this true for PRODUCTION
            }
        }

        if (collision.CompareTag("Tapioca"))
        {
            if (CurrentHealth < MaxHealth) {
                CurrentHealth = CurrentHealth + 10 > MaxHealth ? MaxHealth : CurrentHealth + 10;
            }
        }

        if (collision.CompareTag("DeathRegion"))
        {
            CurrentHealth = 0;
        }
    }

    // set statuses on character
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("SnailRegion"))
            Snailian.isSnailianAllowed = true;
    }

    // unset statuses upon character exit
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SnailRegion"))
            Snailian.isSnailianAllowed = false;
    }
}
