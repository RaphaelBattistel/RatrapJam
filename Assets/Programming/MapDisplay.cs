using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class MapDisplay : MonoBehaviour
{
    [SerializeField] private Transform shipTransform;
    [SerializeField] private Transform CubeDisplay;
    [SerializeField] private GameObject shipMarkerPrefab;
    [SerializeField] private GameObject zoneMarkerPrefab;
    [SerializeField] private float verticalScale = 0.1f; // 1 unité monde = 0.1 unité map en Y
    [SerializeField] private float horizontalScale = 0.1f; // 1 unité monde = 0.1 unité map en X/Z
    [SerializeField] private float rotationSpeed = 20f; // auto-rotation de la carte
    [SerializeField] private float mapTubeRadius = 2f;
    [SerializeField] private float markerScale = 1f; // Contrôle l'échelle des marqueurs

    private Transform _shipMarker;
    private List<Transform> _zoneMarkers = new List<Transform>();
    private Vector3 _startPosition;

    void Start()
    {
        _startPosition = shipTransform.position;

        // Crée le marqueur du vaisseau en haut du cube, parenté sous MapDisplay
        GameObject shipMarkerGO = Instantiate(shipMarkerPrefab, transform);
        _shipMarker = shipMarkerGO.transform;
        _shipMarker.localScale = Vector3.one * markerScale * 50;

        // Crée les marqueurs des zones (parentés sous MapDisplay)
        foreach (RestPointData data in GameManager.Instance.RestPoints)
        {
            GameObject marker = Instantiate(zoneMarkerPrefab, transform);
            marker.transform.localPosition = WorldToMapPosition(data.position);
            marker.transform.localScale = Vector3.one * markerScale;
            marker.name = "Zone_" + data.pointName;
            _zoneMarkers.Add(marker.transform);
        }
    }

    void Update()
    {
        // Met à jour la position du vaisseau (en position locale puisqu'il est enfant de CubeDisplay)
        _shipMarker.localPosition = WorldToMapPosition(shipTransform.position);

        // Rotation auto de la carte (optionnel)
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
    }

    private Vector3 WorldToMapPosition(Vector3 worldPos)
    {
        float depth = (worldPos.y - _startPosition.y) * verticalScale;

        float dx = worldPos.x - _startPosition.x;
        float dz = worldPos.z - _startPosition.z;

        // Distance horizontale réelle par rapport au point de départ, clampée au rayon du tube
        float horizontalDist = new Vector2(dx, dz).magnitude * horizontalScale;
        float radialDist = Mathf.Min(horizontalDist, mapTubeRadius);

        float radialAngle = Mathf.Atan2(dx, dz);

        return new Vector3(
            Mathf.Cos(radialAngle) * radialDist,
            depth,
            Mathf.Sin(radialAngle) * radialDist
        );
    }
}