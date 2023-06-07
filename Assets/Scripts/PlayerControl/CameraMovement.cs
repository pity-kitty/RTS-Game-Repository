using UnityEngine;

namespace PlayerControl
{
    public class CameraMovement : MonoBehaviour
    {
        private const string MouseScrollWheelAxisName = "Mouse ScrollWheel";
        private const int HeightMultiplier = 100;
        private const float Border = 10;

        [SerializeField] float minY = 10;
        [SerializeField] float maxY = 20;
        [SerializeField] float speed = 10;

        private float horizontalInput;
        private float verticalInput;
        private float mouseWheelInput;

        private void Update()
        {
            MoveCamera();
        }

        private void MoveCamera()
        {
            var position = transform.position;

            if (Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - Border)
            {
                position.z += speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= Border)
            {
                position.z -= speed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - Border)
            {
                position.x += speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= Border)
            {
                position.x -= speed * Time.deltaTime;
            }

            mouseWheelInput = Input.GetAxis(MouseScrollWheelAxisName);
            if (mouseWheelInput != 0)
            {
                position.y -= mouseWheelInput * speed * Time.deltaTime * HeightMultiplier;
                position.y = Mathf.Clamp(position.y, minY, maxY);
            }
            transform.position = position;
        }
    }
}
