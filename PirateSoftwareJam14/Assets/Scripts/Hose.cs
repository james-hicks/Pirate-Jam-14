using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
using static UnityEngine.Rendering.HableCurve;

public class Hose : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject hose_Start;
    [SerializeField] private GameObject hose_End;
    [SerializeField] private GameObject hose_Segment;

    [SerializeField] public int segments = 0;

    [SerializeField] private List<GameObject> hose_Segments_Objects = new List<GameObject>();

    [SerializeField] public GameObject playerColliderSim;

    [SerializeField] private GameObject backpackHose;

    [SerializeField] private GameObject hand_Snap;

    [SerializeField] public GameObject water_Snap;

    [SerializeField] public GameObject playerController;

    private bool attached = false;

    [SerializeField] public UpgradeCards HoseLength;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(segments / 2 + 1);
        Debug.Log(Vector3.Distance(hose_Start.transform.position, hose_End.transform.position));

        water_Snap = backpackHose;

        StartCoroutine(CheckHoseLength());

        ResetHose();
    }

    // Update is called once per frame
    void Update()
    {
        hose_Start.transform.position = water_Snap.transform.position;

        hose_End.transform.position = hand_Snap.transform.position;

        playerColliderSim.transform.position = playerController.transform.position;

        if (attached && Vector3.Distance(hose_Start.transform.position, hose_End.transform.position) > segments / 1.5)
        {
            Debug.Log("Too Far");
            water_Snap = backpackHose;
            attached = false;
        }
        else if (attached)
        {
            PlayerController.PlayerInstance.regainWater(2 * Time.deltaTime);
        }
    }

    [Button("Reset Hose")]
    public void ResetHose()
    {
        hose_Start.transform.position = Vector3.zero;

        for (int i = hose_Segments_Objects.Count; i > 0; i--)
        {
            DestroyImmediate(hose_Segments_Objects[i - 1].gameObject);
        }
        hose_Segments_Objects.Clear();

        if (segments <= 0)
        {
            hose_Start.GetComponent<LookAtTarget>().target = hose_End.transform;

            hose_End.transform.position = new Vector3(0.5f, 0, 0);
        }
        else
        {

            for (int i = 0; i < segments; i++)
            {
                GameObject Hose_seg = Instantiate(hose_Segment);
                Hose_seg.transform.parent = gameObject.transform;
                hose_Segments_Objects.Add(Hose_seg);
                hose_Segments_Objects[i].transform.position = new Vector3(0.5f + (0.5f * i), 0, 0);
                hose_Segments_Objects[i].GetComponent<LookAtTarget>().target = hose_End.transform;

                if (i == 0)
                {
                    hose_Start.GetComponent<LookAtTarget>().target = hose_Segments_Objects[i].transform;

                    hose_Segments_Objects[i].GetComponent<SpringJoint>().connectedBody = hose_Start.GetComponent<Rigidbody>();
                }
                else
                {
                    hose_Segments_Objects[i - 1].GetComponent<LookAtTarget>().target = hose_Segments_Objects[i].transform;

                    hose_Segments_Objects[i].GetComponent<SpringJoint>().connectedBody = hose_Segments_Objects[i - 1].GetComponent<Rigidbody>();
                }
            }

            hose_Segments_Objects[hose_Segments_Objects.Count - 1].GetComponent<LookAtTarget>().target = hose_End.transform;

            hose_End.GetComponent<SpringJoint>().connectedBody = hose_Segments_Objects[hose_Segments_Objects.Count - 1].GetComponent<Rigidbody>();

            hose_End.transform.position = new Vector3(0.5f * (hose_Segments_Objects.Count + 1), 0, 0);
        }
    }

    [Button("Add Segment")]
    private void AddSegment()
    {
        segments++;

        ResetHose();
    }

    [Button("Subtract Segment")]
    private void SubtractSegment()
    {
        segments--;

        ResetHose();
    }

    public void Attach(GameObject attach)
    {
        water_Snap = attach;
        attached = true;
    }

    IEnumerator CheckHoseLength()
    {
        bool t = true;

        while (t)
        {
            foreach (UpgradeCards c in CardList.instance.ActiveCards)
            {
                yield return null;
                if (c == HoseLength)
                {
                    Debug.Log("YOU GOT HOSED");
                    segments += 25;
                    ResetHose();
                    t = false; break;
                }

            }
            yield return null;
        }
    }
}
