using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WormSegments : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float _distanceBetween = .2f;
    [SerializeField]
    private float _minScaleFalloff = 0.5f;

    [Header("References")]
    [SerializeField]
    private WormSegment _head;
    [SerializeField]
    private List<WormSegment> _bodyParts = new List<WormSegment>();

    List<WormSegment> bodyParts = new List<WormSegment>();
    List<WormSegment> wormBody = new List<WormSegment>();

    private int currentIndex;
    private int initialLength;

    private float distanceTimer;
    private bool initalized;

    private void Setup()
    {
        currentIndex = 0;
        initialLength = _bodyParts.Count;

        for(int i = 0; i < _bodyParts.Count; i++)
        {
            bodyParts.Add(_bodyParts[i]);
        }
    }

    private void Start()
    {
        Setup();
        CreateBodyParts();
        initalized = true;
    }

    private void OnEnable()
    {
        if (!initalized) return;

        Debug.Log("Readying up!");
        Setup();
        CreateBodyParts();
    }

    public void Clear()
    {
        Debug.Log("I'm clearing!");
        for (int i = 1; i < wormBody.Count; i++)
        {
            Destroy(wormBody[i].gameObject);
        }

        bodyParts.Clear();
        wormBody.Clear();
    }

    public void Pulse(float inTime, float strength, float outTime, Ease inEase, Ease outEase)
    {
        StartCoroutine(IPulse(inTime, strength, outTime, inEase, outEase));
    }

    private IEnumerator IPulse(float inTime, float strength, float outTime, Ease inEase, Ease outEase)
    {
        for (int i = 1; i < wormBody.Count; i++) 
        {
            wormBody[i].Pulse(inTime, strength, outTime, inEase, outEase);
            yield return new WaitForSeconds(0.08f);
        }
    }

    private void FixedUpdate()
    {
        if (bodyParts.Count > 0)
        {
            CreateBodyParts();
        }

        if(wormBody.Count > 1)
        {
            for(int i = 1; i < wormBody.Count; i++)
            {
                MarkerManager marker = wormBody[i - 1].myMarkerManager;
                wormBody[i].transform.position = marker.markers[0].position;
                wormBody[i].transform.rotation = marker.markers[0].rotation;
                marker.markers.RemoveAt(0);
            }
        }
    }

    private void CreateBodyParts()
    {
        if(wormBody.Count == 0)
        {
            wormBody.Add(_head);
            bodyParts.RemoveAt(0);
        }

        // tail marker manager
        MarkerManager marker = wormBody[wormBody.Count - 1].myMarkerManager;

        if(distanceTimer == 0)
        {
            marker.ClearMarkerList();
        }

        distanceTimer += Time.deltaTime;

        if(distanceTimer >= _distanceBetween)
        {
            WormSegment temp = Instantiate(bodyParts[0], marker.markers[0].position, marker.markers[0].rotation, transform);

            float scale = Mathf.Lerp(1.0f, _minScaleFalloff, (float)currentIndex / (float)initialLength);
            currentIndex++;

            temp.transform.localScale = Vector3.one * scale;

            wormBody.Add(temp);
            bodyParts.RemoveAt(0);
            temp.myMarkerManager.ClearMarkerList();
            distanceTimer = 0.0f;
        }
    }
}
