using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ComputerNoteBlock : MonoBehaviour
{
    public AudioSource soundObj;
    public SpriteRenderer spriteObj;
    public Light2D lightObj;
    float currTime;
    float DisableBlockDelay;

    public ComputerNoteBlock(AudioSource sound, SpriteRenderer sprite, Light2D light)
    {
        DisableBlockDelay = 1;
        soundObj = sound;
        spriteObj = sprite;
        lightObj = light;
        spriteObj.color = new Color(255, 255, 255, 0);
        lightObj.intensity = 0;
    }

    public void EnableBlock()
    {
        currTime = Time.time;
        soundObj.Play();
        spriteObj.color = new Color(255, 255, 255, 1);
        lightObj.intensity = 2;
    }

    public bool DisableBlock()
    {
        if (currTime + DisableBlockDelay < Time.time)
        {
            spriteObj.color = new Color(255, 255, 255, 0);
            lightObj.intensity = 0;
            return true;
        }
        return false;
    }
}



public class ColorSequenceController : MonoBehaviour
{
    // various counts
    const int NumBlocks = 4;
    public float interSequenceDisplayDelay; // delay between displaying blocks in a sequence
    public float betweenSequenceDisplayDelay; // delay between displaying sequences
    public int numSequenceDisplays; // how many times a sequence is displayed
    int numInSequence;              // current number in current sequence
    int numInOverallSequence;       // current number across all sequences

    // logic flow delays
    public float DelayFromStartupToSequenceDisplay;

    // block startup sequence
    bool isAscending; // go back and forth on block pad
    public float startupBlockDisplayDelay;

    // block input sequencing
    public List<int> sequenceLength;
    List<int> currentSequence;
    public bool sequenceIsValid, sequenceComplete, totalSuccess;

    // computer display block objects
    List<ComputerNoteBlock> noteBlocks;
    public List<GameObject> BaseBlockInputList;
    Queue<ComputerNoteBlock> noteBlockSequencer;
    public List<GameObject> incompleteObjs, successObjs;

    // actual display block references
    public List<ColorBlockHandler> BlockList;

    // internal controls
    public bool performStartup;
    bool blockIsEnabled;
    float currTime;
    public bool shouldDisplay;
    bool playerNearby;
    bool triggerComputerSuccess;

    public float triggerComputerSuccessDelay;
    int currLightToggle;

    // Start is called before the first frame update
    void Start()
    {
        totalSuccess = false;
        currLightToggle = 0;
        triggerComputerSuccess = false;
        noteBlockSequencer = new Queue<ComputerNoteBlock>();
        isAscending = true;
        blockIsEnabled = false;
        sequenceIsValid = true;
        numInOverallSequence = 1; // iterate over first since it is init in start;
        numInSequence = 0;
        currentSequence = new List<int>();
        for(int i=0; i<sequenceLength[0]; i++)
        {
            currentSequence.Add(Random.Range(0, NumBlocks));
        }

        noteBlocks = new List<ComputerNoteBlock>();
        for (int i=0; i<NumBlocks; i++)
        {
            noteBlocks.Add(new ComputerNoteBlock(
                BaseBlockInputList[i].GetComponent<AudioSource>(),
                BaseBlockInputList[i].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>(),
                BaseBlockInputList[i].transform.GetChild(1).gameObject.GetComponent<Light2D>()));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!totalSuccess)
        {
            HandleManualStartup();
            HandleDisplayStartup();
            HandleSequenceReadout();
        }
        if(triggerComputerSuccess)
            HandleSuccessComputerIndicator();

        HandleComputerNoteBlockCleanup();
    }

    public bool ReceiveAndCheckBlockInput(int value)
    {
        if(value == currentSequence[numInSequence])
        {
            numInSequence++;
            if (numInSequence > currentSequence.Count - 1)
            {
                for(int i=0; i< BlockList.Count; i++)
                {
                    BlockList[i].SetFireOffSuccessTimer();
                }
                currTime = Time.time;
                triggerComputerSuccess = true;
                SetNewSequence(); // can set 'total success' value which is a completion condition
                sequenceComplete = true; // color blocks will read this value
            }
            return true;
        }
        numInSequence = 0; // failure, reset sequence
        sequenceIsValid = false;
        return false;
    }

    // previous sequence was successful,
    // set new sequence or set totalSuccess if done
    private void SetNewSequence()
    {
        if (numInOverallSequence >= sequenceLength.Count)
        {
            totalSuccess = true;
            return;
        }
        numInSequence = 0;
        currentSequence = new List<int>();
        for (int i = 0; i < sequenceLength[numInOverallSequence]; i++)
        {
            currentSequence.Add(Random.Range(0, NumBlocks));
        }
        numInOverallSequence++;
    }

    private void HandleManualStartup()
    {
        if (Input.GetKeyDown(ControlMapping.KeyMap["Interact"]) && playerNearby)
        {
            if (performStartup || shouldDisplay)
                return;

            performStartup = true;
            InteractTextUpdate.UpdateInteractionText($"");
            currTime = Time.time - interSequenceDisplayDelay;
        }
    }

    // whenever player interacts with the computer
    // display all blocks going back and forth
    private void HandleDisplayStartup()
    {
        if(performStartup && currTime + startupBlockDisplayDelay <= Time.time)
        {
            BlockList[numInSequence].FireOffBlockDisplay();
            currTime = Time.time;

            numInSequence = isAscending ? numInSequence + 1 : numInSequence - 1;

            if (numInSequence == NumBlocks - 1)
                isAscending = false;

            if (numInSequence == -1)
            {
                numInSequence = 0;
                isAscending = true; // reset for next passthrough
                performStartup = false;
                shouldDisplay = true;
            }
        }
    }

    // play sound and show color of sequence
    // with delay
    // show twice
    public void HandleSequenceReadout()
    {
        // handle numInSequence exceeds currentSequence Count
        if (shouldDisplay && currTime + interSequenceDisplayDelay < Time.time)
        {
            currTime = Time.time;
            noteBlocks[currentSequence[numInSequence]].EnableBlock();
            noteBlockSequencer.Enqueue(noteBlocks[currentSequence[numInSequence]]);
            numInSequence++;
            if(numInSequence >= currentSequence.Count)
            {
                shouldDisplay = false;
                numInSequence = 0;
            }
        }
    }

    private void HandleComputerNoteBlockCleanup()
    {
        if (noteBlockSequencer.Count > 0)
        {
            if (noteBlockSequencer.Peek().DisableBlock())
            {
                noteBlockSequencer.Dequeue();
            }
        }
    }

    private void HandleSuccessComputerIndicator()
    {
        if(currTime + triggerComputerSuccessDelay < Time.time)
        {
            incompleteObjs[currLightToggle].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            incompleteObjs[currLightToggle].transform.GetChild(1).GetComponent<Light2D>().intensity = 0;
            successObjs[currLightToggle].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            successObjs[currLightToggle].transform.GetChild(1).GetComponent<Light2D>().intensity = 2;
            currLightToggle++;
            triggerComputerSuccess = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger entered");
        if ((collision.CompareTag("Player") || collision.CompareTag("PlayerInvis") || collision.CompareTag("PlayerFlashbang")) && !totalSuccess)
        {
            Debug.Log("Correct Tag");
            playerNearby = true;
            if (!performStartup && !shouldDisplay)
            {
                Debug.Log("Correct state");
                InteractTextUpdate.UpdateInteractionText($"Press {ControlMapping.KeyMap["Interact"].ToString()} to use computer");
            }
        }
    }

    //used to renenable InteractionText if the player doesnt move
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player") || collision.CompareTag("PlayerInvis") || collision.CompareTag("PlayerFlashbang")) && !totalSuccess)
        {
            if(!performStartup && !shouldDisplay)
                InteractTextUpdate.UpdateInteractionText($"Press {ControlMapping.KeyMap["Interact"].ToString()} to use computer");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player") || collision.CompareTag("PlayerInvis") || collision.CompareTag("PlayerFlashbang")) && !totalSuccess)
        {
            playerNearby = false;
            InteractTextUpdate.UpdateInteractionText($"");
        }
    }
}
