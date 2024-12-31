using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Đối tượng mà camera sẽ tập trung vào
    public Vector3 offset; // Khoảng cách giữa camera và đối tượng
    public float smoothTime = 0.3f; // Thời gian để camera làm mượt

    private Vector3 velocity = Vector3.zero;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            // Tính toán vị trí mong muốn
            Vector3 desiredPosition = target.position + offset;

            // Làm mượt chuyển động của camera
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

            // Đảm bảo camera luôn nhìn vào đối tượng
            transform.LookAt(target);
        }
    }
}