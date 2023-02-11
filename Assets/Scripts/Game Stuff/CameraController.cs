using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 13f;
    [SerializeField] float zoomSpeed = 0.1f;
    [SerializeField] float minOrthographicSize = 2f;
    [SerializeField] float maxOrthographicSize = 40f;
    [SerializeField] Camera myCamera;
    Vector2 movement;

    void Update()
    {
        Zoom();
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }

    private void Zoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            myCamera.orthographicSize *= 1 + zoomSpeed;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            myCamera.orthographicSize *= 1 - zoomSpeed;
        }
        myCamera.orthographicSize = Mathf.Clamp(
            myCamera.orthographicSize, minOrthographicSize, maxOrthographicSize);
    }
}