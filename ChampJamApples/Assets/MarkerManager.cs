using System.Collections.Generic;
using UnityEngine;

public class MarkerManager : MonoBehaviour
{
    public class Marker
    {
        public Vector3 position;
        public Quaternion rotation;

        public Marker(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    }

    public List<Marker> markers = new List<Marker>();

    private void FixedUpdate()
    {
        UpdateMarkerList();
    }

    public void UpdateMarkerList()
    {
        markers.Add(new Marker(transform.position, transform.rotation));
    }

    public void ClearMarkerList()
    {
        markers.Clear();
        markers.Add(new Marker(transform.position, transform.rotation));
    }
}