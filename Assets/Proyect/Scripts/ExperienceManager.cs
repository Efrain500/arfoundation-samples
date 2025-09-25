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
    private bool targetMode;

    [SerializeField] private ARRaycastManager aRRaycastManager;
    [SerializeField] private GameObject objectPrefab;

    private bool _canAddObject;
    private Vector3 _detectionPosition = new Vector3();
    private Quaternion _detectionRotation;
    private ARTrackable _currentTrackable = null;
    private GameObject _currentObject;

    public void Start()
    {
        InputHandler.OnTap += SpwanObject;
        SetCanAddObject(true);
        newTarget.onClick.AddListener(ActiveTargetMode);
    }

    public void Update()
    {
        GetRaycastHitTransform();
        if (targetMode && _currentObject != null)
        {
            MoveObject();
        }
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

    void ActiveTargetMode()
    {
        targetMode = !targetMode; 

        if (targetMode)
        {
            Debug.Log("Modo movimiento activado");
            if (_currentObject == null)
            {
                targetMode = false;
            }
        }
        else
        {
            Debug.Log("Modo movimiento desactivado");
        }
    }

    public void MoveObject ()
    {
        var hits = new List<ARRaycastHit>();
        var middleScreen = new Vector2(Screen.width / 2, Screen.height / 2);

        if (aRRaycastManager.Raycast(middleScreen, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            Vector3 targetPosition  = hits[0].pose.position;

            _currentObject.transform.position = Vector3.MoveTowards(_currentObject.transform.position, targetPosition, Time.deltaTime*1f);
            _currentObject.transform.rotation = hits[0].pose.rotation;

            _currentTrackable = hits[0].trackable;
        }
    }
    private void SpwanObject()
    {
        if (!_canAddObject) return;
        _currentObject = Instantiate(objectPrefab);

        _currentObject.GetComponent<ObjectLogic>().PlaceObject(_currentTrackable);
        _currentObject.transform.position = _detectionPosition;
        _currentObject.transform.rotation = _detectionRotation;
        SetCanAddObject(false);
        if (targetMode)
        {
            targetMode = false;
        }
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
