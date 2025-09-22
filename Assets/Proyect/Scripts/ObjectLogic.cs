using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ObjectLogic : MonoBehaviour
{
    public void PlaceObject(ARTrackable trackableParent)
    {
        transform.SetParent(trackableParent?.transform);
    }
}
