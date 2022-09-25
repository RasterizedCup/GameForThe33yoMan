using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CMrefocus : MonoBehaviour
{
    GameObject player;
    CinemachineVirtualCamera cmCam;
    Dictionary<string, GameObject> refocusMap;
    public List<string> nameKey;
    public List<GameObject> followVal;
    public static string currentTrigger = "";
    // Start is called before the first frame update
    void Start()
    {
        refocusMap = new Dictionary<string, GameObject>();
        for (var i=0; i<nameKey.Count; i++)
        {
            refocusMap.Add(nameKey[i], followVal[i]);
        }
        player = GameObject.Find("Bubble Player");
        cmCam = GameObject.Find("CMcam").GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        handleRefocus();
    }

    void handleRefocus()
    {
        Debug.Log(currentTrigger);
        if (refocusMap.ContainsKey(currentTrigger))
        {
            cmCam.Follow = refocusMap[currentTrigger].transform;
        }
        else
        {
            cmCam.Follow = player.transform;
        }
    }
}
