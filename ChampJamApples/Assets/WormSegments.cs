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

    public void Pulse()
    {
        StartCoroutine(IPulse());
    }

    private IEnumerator IPulse()
    {
        for (int i = 1; i < wormBody.Count; i++) 
        {
            yield return null;
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
