using Unity.VisualScripting;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _distanceFromTarget = 20f;

    [SerializeField] private float sensitivity = 1000f;
    private float _yaw = 0f;
    private float _pitch = 0f;

    private void Update()
    {
        HandleInput();

        Quaternion yawRotation = Quaternion.Euler(_pitch, _yaw, 0f);

        RotateCamera(yawRotation);
    }


    public void HandleInput()
    {
        Vector2 inputDelta = Vector2.zero;

        if(Input.GetMouseButton(1))
        {
            inputDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        _yaw += inputDelta.x * sensitivity * Time.deltaTime;
        _pitch -= inputDelta.y * sensitivity * Time.deltaTime;
    }

    private void RotateCamera(Quaternion rotation)
    {
        Vector3 positionOffset = rotation * new Vector3(0, 0, -_distanceFromTarget);
        transform.position = _target.position + positionOffset;
        transform.rotation = rotation;
        _target.rotation = rotation;
    }
}
