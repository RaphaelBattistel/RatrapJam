using UnityEngine;

public class RestPointSpawner : MonoBehaviour
{
    [SerializeField] private GameObject restPointPrefab;

    void Start()
    {
        foreach (RestPointData data in GameManager.Instance.RestPoints)
        {
            SpawnRestPoint(data);
        }
    }

    private void SpawnRestPoint(RestPointData data)
    {
        GameObject instance = Instantiate(restPointPrefab, data.position, Quaternion.identity);
        instance.name = string.IsNullOrEmpty(data.pointName) ? "RestPoint" : data.pointName;

        if (instance.TryGetComponent<RestPointZone>(out RestPointZone zone))
        {
            // Marque la dernière zone comme isLast = true
            bool isLastZone = data == GameManager.Instance.RestPoints[GameManager.Instance.RestPoints.Count - 1];
            zone.Initialize(data.coordinateDialogue, data.zoneDialogue, data.pecentageTempMinus, isLastZone);
        }

        if (instance.TryGetComponent<SphereCollider>(out SphereCollider col))
        {
            col.radius = data.triggerRadius;
        }
    }
}