using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject ship;

    public Vector3 transitionPosition;  // Die Zielposition, zu der die Kamera bewegt werden soll
    public Quaternion transitionRotation;  // Die Zielausrichtung (Rotation) der Kamera
    public float bewegungsgeschwindigkeit = 2.0f;  // Die Geschwindigkeit, mit der die Kamera bewegt wird
    public float rotationsgeschwindigkeit = 2.0f;  // Die Geschwindigkeit, mit der die Kamera rotiert wird

    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;


    private Vector3 offset = new Vector3(-2.23f, 1.52f, -0.55f);

    public float driveModeMinDistance = 2.0f;
    public float driveModeMaxDistance = 20.0f;
    public float driveModeDistance = 5.0f;
    public float driveModeRotationSpeed = 2.0f;
    public float driveModeZoomSpeed = 4.0f;
    public float driveModeYRotation = 45.0f;

    public float mapModeMoveSpeed = 5f;
    public float mapModeZoomSpeed = 5f;
    public float mapModeRotationSpeed = 2.0f;
    public float mapModeRotationX = 90.0f;
    public float mapModeZoomMin = 10f;
    public float mapModeZoomMax = 40f;

    private float rotationX = 0.0f;
    private bool isMap = false;
    private bool transition = false;

    public Vector3 lastMapPostition;
    public Quaternion lastMapRotation;
    public Vector3 lastDrivePostition;
    public Quaternion lastDriveRotation;

    public GameObject MapCanvas;
    public GameObject DriveCanvas;
    internal RenderTexture targetTexture;

    private void Update()
    {
        ship = GameObject.FindWithTag("Player");
        if (ship == null)
        {
            return;
        }

        if (transition) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMap = !isMap;

            if (isMap)
            {
                transitionPosition = new Vector3(ship.transform.position.x, lastMapPostition.y,ship.transform.position.z);
                //transitionPosition = lastMapPostition;
                transitionRotation = lastMapRotation;
            }
            else
            {
                transitionPosition = lastDrivePostition + ship.transform.position;
                transitionRotation = lastDriveRotation;
            }


            StartCoroutine(Transition());
        }

        if (isMap)
        {
            HandleMapMode();
        }
        else
        {
            HandleDriveMode();
        }

    }





    void HandleDriveMode()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        driveModeDistance = Mathf.Clamp(driveModeDistance - scroll * driveModeZoomSpeed, driveModeMinDistance, driveModeMaxDistance);









        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            rotationX += mouseX * driveModeRotationSpeed;
        }
        Quaternion rotation = Quaternion.Euler(driveModeYRotation, rotationX, 0);

        transform.position = ship.transform.position + rotation * offset - transform.forward * driveModeDistance;
        transform.LookAt(ship.transform);

        lastDrivePostition = transform.position - ship.transform.position;
        lastDriveRotation = transform.rotation;
    }

    void HandleMapMode()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dragStartPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            dragCurrentPosition = Input.mousePosition;
            Vector3 difference = dragStartPosition - dragCurrentPosition;
            dragStartPosition = dragCurrentPosition;

            Vector3 newPosition = transform.position + new Vector3(difference.x, 0, difference.y) * mapModeMoveSpeed * Time.deltaTime;
            transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 zoom = transform.position + transform.forward * scroll * mapModeZoomSpeed;
        transform.position = new Vector3(zoom.x, Mathf.Clamp(zoom.y, mapModeZoomMin, mapModeZoomMax), zoom.z);

        Quaternion rotation = Quaternion.Euler(mapModeRotationX, 0, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, mapModeRotationSpeed * Time.deltaTime);

        lastMapPostition = transform.position;
        lastMapRotation = transform.rotation;
    }




    private IEnumerator Transition()
    {
        MapCanvas.gameObject.SetActive(false);
        DriveCanvas.gameObject.SetActive(false);
        transition = true;
        while (Vector3.Distance(transform.position, transitionPosition) > 0.01f || Quaternion.Angle(transform.rotation, transitionRotation) > 0.01f)
        {
            Debug.Log("Transition");
            // Bewege die Kamera zur Zielposition
            transform.position = Vector3.Lerp(transform.position, transitionPosition, bewegungsgeschwindigkeit * Time.deltaTime);

            // Rotiere die Kamera zur Zielrotation
            transform.rotation = Quaternion.Slerp(transform.rotation, transitionRotation, rotationsgeschwindigkeit * Time.deltaTime);

            yield return null;
        }
        transition = false;

        if (isMap)
        {
            MapCanvas.gameObject.SetActive(true);
        }
        else
        {
            DriveCanvas.gameObject.SetActive(true);
        }
    }

    internal void CopyFrom(Camera mainCamera)
    {
        throw new NotImplementedException();
    }
}
