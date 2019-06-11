using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

// Translates the Kinect-data to Unity data, and interacts with any objects thrown or clicked
public class DetectionManager : MonoBehaviour
{
    public LayerMask layer;
    public Camera cam;
    public CollisionType collisionType;
    [Tooltip("Enable to detect mouse-clicks")]
    public bool DeveloperMode;

    private void Start()
    {
        Application.targetFrameRate = 10;
        SceneManager.sceneLoaded += AttachCamera;
        if (cam == null)
            cam = Camera.main;

        StartCoroutine(_BlobHandle());
        if (DeveloperMode)
            StartCoroutine(_MouseHandle());
    }

    private void AttachCamera(Scene scene, LoadSceneMode mode)
    {
        cam = Camera.main;
    }

    //Checks the collision of each blob (hit registration from the SmartWall), and interacts with the object it hit
    IEnumerator _BlobHandle()
    {
        while (true)
        {
            foreach (Blob b in BlobTracking.Blobs)
            {
                if (OkayCheck(b))
                {
                    GetCollisionType(TranslateToWorld(b));
                }
            }

            yield return null;
        }
    }

    //Developer tool. Clicks on the mouseposition and interacts with the object the same way as a blob would
    IEnumerator _MouseHandle()
    {
        while (true)
        {
            if (DeveloperMode && Input.GetMouseButtonDown(0))
            {
                GetCollisionType(GetMousePosition());
            }

            yield return null;
        }
    }

    //Checks if the detection needs to detect 2D- or 3D-colliders, or both
    void GetCollisionType(Vector3 position)
    {
        switch (collisionType)
        {
            case CollisionType._2D:
                //2D
                Raycast(layer, (Vector2)position);
                break;
            case CollisionType._3D: 
                //3D
                Raycast(layer, position);
                break;
            case CollisionType.Both:
                Raycast(layer, (Vector2)position);
                Raycast(layer, position);
                break;
        }
    }

    #region Data Conversion
    //Checks if the blob falls within the boundaries of the screen
    private bool OkayCheck(Blob _blob)
    {
        return (_blob.XPosition >= 0 &&
            _blob.XPosition <= 1 &&
            _blob.YPosition >= 0 &&
            _blob.YPosition <= 1 &&
            _blob.Height > 0 &&
            _blob.Width > 0);
    }

    //Translates the kinect data to Unity coordinates.
    private Vector3 TranslateToWorld(Blob b)
    {
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        return new Vector3(((cam.transform.position.x) + (b.XPosition * width) - (width / 2)),
            ((cam.transform.position.y) + (b.YPosition * height) - (height / 2)), -3f);
    }

    //Translates the mouseclick to Unity coordinates.
    private Vector3 GetMousePosition()
    {
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }
    #endregion

    #region 2D
    //Raycasts at the given position, returns the hit object.
    private void Raycast(LayerMask layerMask, Vector2 position)
    {
        Ray ray = new Ray(position, Vector3.forward);

        Debug.DrawRay(ray.origin, ray.direction, Color.red, 5);
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(position, Vector3.forward, Mathf.Infinity, layerMask))
        {
            Click(hit);
        }
    }

    //Clicks at the given position, checks for an interactable object, and interacts with it.
    private void Click(RaycastHit2D hit)
    {
        if (hit.transform != null)
        {
            Interactable obj = hit.transform.GetComponent<Interactable>();
            if (obj != null)
            {
                obj.Interact(hit.point);
            }
        }
    }
    #endregion

    #region 3D
    //Raycasts at the given position, returns the hit object.
    private void Raycast(LayerMask layerMask, Vector3 position)
    {
        Ray ray = new Ray(position, Vector3.forward);

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 5);
        foreach (RaycastHit hit in Physics.RaycastAll(ray, Mathf.Infinity, layerMask))
        {
            Click(hit);
        }
    }

    //Clicks at the given position, checks for an interactable object, and interacts with it.
    private void Click(RaycastHit hit)
    {
        if (hit.transform != null)
        {
            Interactable obj = hit.transform.GetComponent<Interactable>();
            if (obj != null)
            {
                obj.Interact(hit.point);
            }
        }
    }
    #endregion
}

public enum CollisionType
{
    [Description("2D")]
    _2D,
    [Description("3D")]
    _3D,
    Both
}