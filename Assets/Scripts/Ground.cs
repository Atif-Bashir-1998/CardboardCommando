using UnityEngine;

public class Ground : Interactive
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public new void Interact()
    {
        // Get the hit position from CameraInteract
        RaycastHit hit = CameraInteract.GetLatestHit();
        
        if (hit.collider != null && hit.collider.gameObject == this.gameObject)
        {
            Vector3 gazePosition = hit.point;
            Debug.Log($"User is looking at ground position: X={gazePosition.x:F2}, Y={gazePosition.y:F2}, Z={gazePosition.z:F2}");
        }
    }
}
