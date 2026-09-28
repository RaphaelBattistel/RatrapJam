using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IObjectInteractable
{
    [SerializeField] float duration = 1f;
    private bool _opened = false;

    private void Start()
    {

    }

    public void DoOnInteract()
    {
        if (!_opened)
        {
            StartCoroutine(RotateDoor(new Vector3(0, -90, 0)));
            _opened = true;
        }
        else
        {
            StartCoroutine(RotateDoor(new Vector3(0, 0, 0)));
            _opened = false;
        }
    }

    private IEnumerator RotateDoor(Vector3 targetEulerAngles)
    {
        Vector3 startRotation = transform.eulerAngles;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.eulerAngles = Vector3.Lerp(startRotation, targetEulerAngles, elapsed / duration);

       
            elapsed += Time.deltaTime;
            yield return null;
        }


    }
}
