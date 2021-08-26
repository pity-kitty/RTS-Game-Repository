using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float horizontalInput;
    float verticalInput;
    float mouseWheelInput;
    float border = 10;
    [SerializeField] float minY = 10;
    [SerializeField] float maxY = 20;
    [SerializeField] float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - border)
        {
            position.z += speed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= border)
        {
            position.z -= speed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - border)
        {
            position.x += speed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= border)
        {
            position.x -= speed * Time.deltaTime;
        }

        mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");
        position.y -= mouseWheelInput * speed * Time.deltaTime * 100;

        position.y = Mathf.Clamp(position.y, minY, maxY);

        transform.position = position;
    }
}
