using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snailian : MonoBehaviour
{
    public static bool isSnailianAllowed;
    public static bool isSnailianActive;
    public bool SnailModeEnabled;
    public static bool isSnailianPrimed;
    public float maxVelocity;
    public float translateRate;
    public float velRedirect;
    public float velIncr, velDecr;
    public LayerMask SnailCrawlLayers;
    public Rigidbody2D rb2d;
    Transform player;
    public SpriteRenderer filSprite;
    public Animator Snailimator;
    public SpriteRenderer SnailSprite;
    Vector3 PrevRotationStartPos;
    public GameObject RightBox;
    public GameObject LeftBox;
    public GameObject BottomBox;
    public GameObject BottomRightBox;
    public GameObject BottomLeftBox;

    public AudioClip BecomeSnail;
    public AudioClip BecomeFil;

    public AudioSource SnailStick;
    AudioSource SnailStuff;
    Vector2 BoxSize = new Vector2(.35f, .2f);
    Vector2 SectionBottomSize = new Vector2(1, 1);
    int prevAbsAngle;
    Vector3 PriorFramePosition; // we use this to add to the deltaOffset to TravelDistanceFeelers to renable collision
    bool rotationTrigger;

    private void Start()
    {
        SnailStuff = GetComponent<AudioSource>();
        SnailStuff.clip = BecomeSnail;
        filSprite = GameObject.FindGameObjectWithTag("FilSprite").GetComponent<SpriteRenderer>();
        rb2d = GameObject.Find("Bubble Player").GetComponent<Rigidbody2D>();
        player = GameObject.Find("Bubble Player").GetComponent<Transform>();
        isSnailianActive = false;

        PriorFramePosition = player.position;

        SnailModeEnabled = false;
        isSnailianPrimed = false;

        rotationTrigger = false;
    }
    public bool handleSnailianMovement()
    {
        // don't allow initial proccing if overlap is too close
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SnailModeEnabled = !SnailModeEnabled;
            // enable base snail mode check for land
            if (SnailModeEnabled)
            {
                BoxSize = new Vector2(.35f, .2f);
                // can't switch to snail if too close to a wall
                if (isAlreadyOverlapping())
                {
                    SnailModeEnabled = false;
                    return false;
                }

                SnailStuff.volume = .1f;
                SnailStuff.clip = BecomeSnail;
                SnailStuff.Play();
                isSnailianPrimed = true;
                filSprite.color = new Color(filSprite.color.r, filSprite.color.g, filSprite.color.b, 0);
                SnailSprite.color = new Color(filSprite.color.r, filSprite.color.g, filSprite.color.b, 1);
                Snailimator.enabled = true;
            }
            else // full disable, we return to normal filian
            {
                // reset double jump if we were clung to a wall on switch
                if (isSnailianActive)
                    Jumping.canDoubleJump = true;
                SnailStuff.volume = .3f;
                SnailStuff.clip = BecomeFil;
                SnailStuff.Play();
                rb2d.bodyType = RigidbodyType2D.Dynamic;
                player.rotation = Quaternion.Euler(0, 0, 0);
                filSprite.color = new Color(filSprite.color.r, filSprite.color.g, filSprite.color.b, 1);
                SnailSprite.color = new Color(filSprite.color.r, filSprite.color.g, filSprite.color.b, 0);
                Snailimator.enabled = false;
                isSnailianPrimed = false;
                isSnailianActive = false;
                return false;
            }
        }

        // if we encounter any collision with a surface from setSnailRotation
        // set the rb2d to static and engage snailian active
        if (isSnailianPrimed)
        {
            // don't allow initial proccing if overlap is too close
            if (setSnailRotation())
            {
                // play some kind of snail sticking sound
                SnailStick.Play();
                isSnailianActive = true;
                isSnailianPrimed = false;
                rb2d.bodyType = RigidbodyType2D.Kinematic;
                rb2d.interpolation = RigidbodyInterpolation2D.Extrapolate;
                rb2d.velocity = Vector2.zero;
            }
            else
                return true;
        }

        Vector2 position = player.position;

        // get modulo angle
        int absoluteAngle = (int)Mathf.Round(player.rotation.eulerAngles.z % 360);
        if (absoluteAngle < 180)
        {
            if (player.position.x > PriorFramePosition.x || player.position.y > PriorFramePosition.y)
            {
                SnailSprite.flipX = false;
            }
            else if (player.position.x < PriorFramePosition.x || player.position.y < PriorFramePosition.y)
            {
                SnailSprite.flipX = true;
            }
        }
        else
        {
            if (player.position.x < PriorFramePosition.x || player.position.y < PriorFramePosition.y)
            {
                SnailSprite.flipX = false;
            }
            else if (player.position.x > PriorFramePosition.x || player.position.y > PriorFramePosition.y)
            {
                SnailSprite.flipX = true;
            }
        }

        // cut short on no run
        if (!isSnailianActive)
        {
            Snailimator.enabled = false;
            return false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            // discern how to move based on rotation
            if(absoluteAngle == 0) // going right
                position.x += (translateRate * Time.deltaTime);
            if (absoluteAngle == 90) // going up
                position.y += (translateRate * Time.deltaTime);
            if (absoluteAngle == 180) // going up
                position.x -= (translateRate * Time.deltaTime);
            if (absoluteAngle == 270) // going up
                position.y -= (translateRate * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            // discern how to move based on rotation
            if (absoluteAngle == 0) // going right
                position.x -= (translateRate * Time.deltaTime);
            if (absoluteAngle == 90) // going up
                position.y -= (translateRate * Time.deltaTime);
            if (absoluteAngle == 180) // going up
                position.x += (translateRate * Time.deltaTime);
            if (absoluteAngle == 270) // going up
                position.y += (translateRate * Time.deltaTime);
        }

        PriorFramePosition = player.position;
        player.position = position;
        setSnailRotation();

        Snailimator.SetBool("isMoving", PriorFramePosition != player.position);
      
        return true;
    }

    private bool isAlreadyOverlapping()
    {
        Collider2D[] overlapR = Physics2D.OverlapBoxAll(RightBox.transform.position, BoxSize, 0, SnailCrawlLayers);
        Collider2D[] overlapL = Physics2D.OverlapBoxAll(LeftBox.transform.position, BoxSize, 0, SnailCrawlLayers);
        if (overlapR.Length > 0 || overlapL.Length > 0)
            return true;
        return false;
    }

    private bool setSnailRotation()
    {
        int absoluteAngle = (int)Mathf.Round(player.rotation.eulerAngles.z % 360);
        // with a rotation, do not permit another rotation until all feelers are free
        if (rotationTrigger)
        {
            Collider2D[] overlapR = Physics2D.OverlapBoxAll(RightBox.transform.position, BoxSize, 0, SnailCrawlLayers);
            Collider2D[] overlapL = Physics2D.OverlapBoxAll(LeftBox.transform.position, BoxSize, 0, SnailCrawlLayers);
            if (overlapR.Length == 0 && overlapL.Length == 0)
                rotationTrigger = false;
        }

        // set right box
        if (!rotationTrigger)
        {
            Collider2D[] overlap = Physics2D.OverlapBoxAll(RightBox.transform.position, BoxSize, 0, SnailCrawlLayers);
            if (overlap.Length > 0)
            {
                PrevRotationStartPos = player.position;
                prevAbsAngle = absoluteAngle;
                player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z + 90);
                int newAbsoluteAngle = (int)Mathf.Round(player.rotation.eulerAngles.z % 360);
                AngleQuickOverlapFix(newAbsoluteAngle);
                rotationTrigger = true;
                return true;
            }
        }
        if (!rotationTrigger)
        {
            Collider2D[] overlap = Physics2D.OverlapBoxAll(LeftBox.transform.position, BoxSize, 0, SnailCrawlLayers);
            if (overlap.Length > 0)
            {
                PrevRotationStartPos = player.position;
                prevAbsAngle = absoluteAngle;
                player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z - 90);
                int newAbsoluteAngle = (int)Mathf.Round(player.rotation.eulerAngles.z % 360);
                AngleQuickOverlapFix(newAbsoluteAngle);
                rotationTrigger = true;
                return true;
            }
        }

        // handle all external cases (find a better way to implement this holy guacamole)
        if (isSnailianActive)
        {
            Collider2D[] vertOverlapExtern = Physics2D.OverlapBoxAll(BottomRightBox.transform.position, SectionBottomSize, 0, SnailCrawlLayers);
            if (vertOverlapExtern.Length == 0)
            {
                if (absoluteAngle == 0 && player.position.x > PriorFramePosition.x)
                {
                    player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z - 90);
                    int newAbsoluteAngle = (int)Mathf.Round(player.rotation.eulerAngles.z % 360);
                    prevAbsAngle = absoluteAngle;
                    AngleQuickOverlapFixExternal(newAbsoluteAngle);
                }
                else if(absoluteAngle == 0 && player.position.x < PriorFramePosition.x)
                {
                    player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z + 90);
                    int newAbsoluteAngle = (int)Mathf.Round(player.rotation.eulerAngles.z % 360);
                    prevAbsAngle = absoluteAngle;
                    AngleQuickOverlapFixExternal(newAbsoluteAngle);
                }
                else if (absoluteAngle == 90 && player.position.y < PriorFramePosition.y)
                {
                    player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z + 90);
                    int newAbsoluteAngle = (int)Mathf.Round(player.rotation.eulerAngles.z % 360);
                    prevAbsAngle = absoluteAngle;
                    AngleQuickOverlapFixExternal(newAbsoluteAngle);
                }
                else if (absoluteAngle == 90 && player.position.y > PriorFramePosition.y)
                {
                    player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z - 90);
                    int newAbsoluteAngle = (int)Mathf.Round(player.rotation.eulerAngles.z % 360);
                    prevAbsAngle = absoluteAngle;
                    AngleQuickOverlapFixExternal(newAbsoluteAngle);
                }
                else if (absoluteAngle == 180 && player.position.x < PriorFramePosition.x)
                {
                    player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z - 90);
                    int newAbsoluteAngle = (int)Mathf.Round(player.rotation.eulerAngles.z % 360);
                    prevAbsAngle = absoluteAngle;
                    AngleQuickOverlapFixExternal(newAbsoluteAngle);
                }
                else if (absoluteAngle == 180 && player.position.x > PriorFramePosition.x)
                {
                    player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z + 90);
                    int newAbsoluteAngle = (int)Mathf.Round(player.rotation.eulerAngles.z % 360);
                    prevAbsAngle = absoluteAngle;
                    AngleQuickOverlapFixExternal(newAbsoluteAngle);
                }
                else if (absoluteAngle == 270 && player.position.y < PriorFramePosition.y)
                {
                    player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z - 90);
                    int newAbsoluteAngle = (int)Mathf.Round(player.rotation.eulerAngles.z % 360);
                    prevAbsAngle = absoluteAngle;
                    AngleQuickOverlapFixExternal(newAbsoluteAngle);
                }
                else if (absoluteAngle == 270 && player.position.y > PriorFramePosition.y)
                {
                    player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z + 90);
                    int newAbsoluteAngle = (int)Mathf.Round(player.rotation.eulerAngles.z % 360);
                    prevAbsAngle = absoluteAngle;
                    AngleQuickOverlapFixExternal(newAbsoluteAngle);
                }
            }
        }
        Collider2D[] vertOverlap = Physics2D.OverlapBoxAll(BottomBox.transform.position, BoxSize, 0, SnailCrawlLayers);
        if (vertOverlap.Length > 0)
            return true;

        return false;
    }

    // if a user quickly backtracks after a rotation, this is in place to prevent wall clipping, placing them slightly forward on th path
    void AngleQuickOverlapFix(int currentAbsAngle)
    {
        if(currentAbsAngle == 90 || currentAbsAngle == 270)
        {
            BoxSize = new Vector2(.2f, .4f);
        }
        if(currentAbsAngle == 0 || currentAbsAngle == 180)
        {
            BoxSize = new Vector2(.4f, .2f);
        }

        if((currentAbsAngle == 90 || currentAbsAngle == 270) && prevAbsAngle == 0)
        {
            player.position = new Vector3(player.position.x, player.position.y + .05f, 0);
        }
        if ((currentAbsAngle == 90 || currentAbsAngle == 270) && prevAbsAngle == 180)
        {
            player.position = new Vector3(player.position.x, player.position.y - .05f, 0);
        }
        if ((currentAbsAngle == 0 || currentAbsAngle == 180) && prevAbsAngle == 90)
        {
            player.position = new Vector3(player.position.x - .05f, player.position.y, 0);
        }
        if ((currentAbsAngle == 0 || currentAbsAngle == 180) && prevAbsAngle == 270)
        {
            player.position = new Vector3(player.position.x + .05f, player.position.y, 0);
        }
    }

    void AngleQuickOverlapFixExternal(int currentAbsAngle)
    {
        if (currentAbsAngle == 90 || currentAbsAngle == 270)
        {
            BoxSize = new Vector2(.2f, .4f);
        }
        if (currentAbsAngle == 0 || currentAbsAngle == 180)
        {
            BoxSize = new Vector2(.4f, .2f);
        }

        Debug.Log(currentAbsAngle + " " + prevAbsAngle);
        if ((currentAbsAngle == 90 || currentAbsAngle == 270) && prevAbsAngle == 0)
        {
            player.position = new Vector3(player.position.x, player.position.y - .15f, 0);
        }
        if ((currentAbsAngle == 90 || currentAbsAngle == 270) && prevAbsAngle == 180)
        {
            player.position = new Vector3(player.position.x, player.position.y + .15f, 0);
        }
        if ((currentAbsAngle == 0 || currentAbsAngle == 180) && prevAbsAngle == 90)
        {
            player.position = new Vector3(player.position.x + .15f, player.position.y, 0);
        }
        if ((currentAbsAngle == 0 || currentAbsAngle == 180) && prevAbsAngle == 270)
        {
            player.position = new Vector3(player.position.x - .15f, player.position.y, 0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(RightBox.transform.position, BoxSize);
        Gizmos.DrawWireCube(LeftBox.transform.position, BoxSize);
        Gizmos.DrawWireCube(BottomRightBox.transform.position, SectionBottomSize);
        Gizmos.DrawWireCube(BottomLeftBox.transform.position, SectionBottomSize);
    }
}
