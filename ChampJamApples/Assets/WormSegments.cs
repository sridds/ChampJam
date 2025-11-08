using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
    private List<WormSegment> bodyParts = new List<WormSegment>();

    List<WormSegment> wormBody = new List<WormSegment>();

    private float distanceTimer;

    private void Start()
    {
        CreateBodyParts();
    }

    private void OnDisable()
    {
        distanceTimer = 0.0f;

        for (int i = 0; i < wormBody.Count; i++)
        {
            wormBody[i].myMarkerManager.ClearMarkerList();

            wormBody[i].transform.position = wormBody[0].transform.position;
            wormBody[i].transform.rotation = wormBody[0].transform.rotation;
        }
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Pulse(0.0f, 1.7f, 0.3f, Ease.Linear, Ease.OutQuad);
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
                MarkerManager marker = wormBody[i - 1].GetComponent<MarkerManager>();
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
            wormBody.Add(temp);
            bodyParts.RemoveAt(0);
            temp.myMarkerManager.ClearMarkerList();
            distanceTimer = 0.0f;
        }
    }
}
