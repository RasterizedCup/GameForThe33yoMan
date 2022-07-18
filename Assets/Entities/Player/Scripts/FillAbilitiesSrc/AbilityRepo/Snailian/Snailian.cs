using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snailian : MonoBehaviour
{
    public static bool isSnailianActive;
    public float maxVelocity;
    public float translateRate;
    public float velRedirect;
    public float velIncr, velDecr;
    public LayerMask SnailCrawlLayers;
    float TravelDistanceToRenableFeelers;
    float TravelDelta;
    Rigidbody2D rb2d;
    Transform player;
    SpriteRenderer filSprite;
    public Animator Snailimator;
    public SpriteRenderer SnailSprite;

    public GameObject RightBox;
    public GameObject LeftBox;
    Vector2 BoxSize = new Vector2(.2f, .2f);

    Vector3 PriorFramePosition; // we use this to add to the deltaOffset to TravelDistanceFeelers to renable collision

    bool feelersEnabled;
    private void Start()
    {
        filSprite = GameObject.FindGameObjectWithTag("FilSprite").GetComponent<SpriteRenderer>();
        rb2d = GameObject.Find("Bubble Player").GetComponent<Rigidbody2D>();
        player = GameObject.Find("Bubble Player").GetComponent<Transform>();
        isSnailianActive = false;

        feelersEnabled = true; // for now

        TravelDistanceToRenableFeelers = RightBox.transform.position.x;

        PriorFramePosition = player.position;
    }
    public bool handleSnailianMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSnailianActive = !isSnailianActive;
            if (isSnailianActive)
            {
                Debug.Log($"Toggle Snailan {isSnailianActive}");
                // we want to handle snail movement on translate, not physics
                rb2d.velocity = Vector2.zero;
                rb2d.bodyType = RigidbodyType2D.Static;
                filSprite.color = new Color(filSprite.color.r, filSprite.color.g, filSprite.color.b, 0);
                SnailSprite.color = new Color(filSprite.color.r, filSprite.color.g, filSprite.color.b, 1);
                Snailimator.enabled = true;
            }
            else
            {
                Debug.Log($"Toggle Snailan {isSnailianActive}");
                rb2d.bodyType = RigidbodyType2D.Dynamic;
                player.rotation = Quaternion.Euler(0, 0, 0);
                filSprite.color = new Color(filSprite.color.r, filSprite.color.g, filSprite.color.b, 1);
                SnailSprite.color = new Color(filSprite.color.r, filSprite.color.g, filSprite.color.b, 0);
                Snailimator.enabled = false;
                return false;
            }
        }

        // cut short on no run
        if (!isSnailianActive)
        {
            Snailimator.enabled = false;
            return false;
        }

        Vector2 position = player.position;

        // get modulo angle
        int absoluteAngle = (int)Mathf.Round(player.rotation.eulerAngles.z % 360);
        if (Input.GetKey(KeyCode.D))
        {
            Debug.Log(absoluteAngle);
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
            Debug.Log(absoluteAngle);
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
        if(feelersEnabled)
            setSnailRotation();
        player.position = position;

        Snailimator.SetBool("isMoving", PriorFramePosition != player.position);

        return true;
    }


    // upon rotating, disable all rotations until a minimum distance of feeler.pos.x has been travelled
    private void setSnailRotation()
    {
        // set right box
        Collider2D[] overlap = Physics2D.OverlapBoxAll(RightBox.transform.position, BoxSize, 0, SnailCrawlLayers);
        if (overlap.Length > 0)
        {
            Debug.Log("Impact Detected");
            player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z + 90);
            return;
        }

      /*  overlap = Physics2D.OverlapBoxAll(LeftBox.transform.position, BoxSize, 0, SnailCrawlLayers);
        if (overlap.Length > 0)
        {
            Debug.Log("Impact Detected");
            player.rotation = Quaternion.Euler(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y, player.rotation.eulerAngles.z - 90);

            return;
        }*/

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(RightBox.transform.position, BoxSize);
        Gizmos.DrawWireCube(LeftBox.transform.position, BoxSize);
    }

    // get box overlay on left, right, top bottom of character
    // logic discern how to rotate transform based on enters and exits
}
