using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using System.Collections;

public class ExperienceManager : MonoBehaviour
{
    [SerializeField] private Button addObject;
    [SerializeField] private Button changeForm;
    [SerializeField] private Button newTarget;

    [SerializeField] private ARRaycastManager aRRaycastManager;
    [SerializeField] private GameObject objectPrefab;

    private bool _canAddObject;
    private Vector3 _detectionPosition = new Vector3();
    private Quaternion _detectionRotation;
    private ARTrackable _currentTrackable = null;

    public void Start()
    {
        InputHandler.OnTap += SpwanObject;
        SetCanAddObject(true);
    }

    public void Update()
    {

        GetRaycastHitTransform();
    }

    private void GetRaycastHitTransform()
    {
        var hits = new List<ARRaycastHit>();
        var middleScreen = new Vector2(Screen.width / 2, Screen.height / 2);

        if (aRRaycastManager.Raycast(middleScreen, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            _detectionPosition = hits[0].pose.position;
            _detectionRotation = hits[0].pose.rotation;
            _currentTrackable = hits[0].trackable;
        }
    }

    public void MoveObject ()
    {
        var hits = new List<ARRaycastHit>();
        var mousePosition = Input.mousePosition;
        if (aRRaycastManager.Raycast(mousePosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            Vector3 targetPosition  = hits[0].pose.position;
            objectPrefab.transform.position = Vector3.Lerp(objectPrefab.transform.position, targetPosition, 0.3f);
            objectPrefab.transform.rotation = hits[0].pose.rotation;

            _currentTrackable = hits[0].trackable;
        }


    }
    private void SpwanObject()
    {
        if (!_canAddObject) return;
        GameObject cubo = Instantiate(objectPrefab);

        cubo.GetComponent<ObjectLogic>().PlaceObject(_currentTrackable);
        cubo.transform.position = _detectionPosition;
        cubo.transform.rotation = _detectionRotation;
        SetCanAddObject(false);
    }

    private void OnDestroy()
    {
        InputHandler.OnTap -= SpwanObject;
    }

    public void SetCanAddObject(bool canaddObject)
    {
        _canAddObject = canaddObject;
        addObject.gameObject.SetActive(!_canAddObject);

    }

}
