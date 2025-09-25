using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ObjectLogic : MonoBehaviour
{
    private Renderer renderObject;

    private void OnEnable()
    {
        renderObject = GetComponent<Renderer>();
    }
    public void PlaceObject(ARTrackable trackableParent)
    {
        transform.SetParent(trackableParent?.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Object")&& other.gameObject != this.gameObject)
        {
            
            Debug.Log("colision detectada");
            Color randomColor = Random.ColorHSV();
            renderObject.material.color = randomColor;
        }
    }
}
