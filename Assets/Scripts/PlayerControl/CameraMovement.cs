using UnityEngine;

namespace PlayerControl
{
    public class CameraMovement : MonoBehaviour
    {
        private const string MouseScrollWheelAxisName = "Mouse ScrollWheel";
        private const int HeightMultiplier = 100;

        [SerializeField] float minY = 10;
        [SerializeField] float maxY = 20;
        [SerializeField] float speed = 10;

        private float horizontalInput;
        private float verticalInput;
        private float mouseWheelInput;
        private float border = 10;
        
        private void Update()
        {
            MoveCamera();
        }

        private void MoveCamera()
        {
            var position = transform.position;

            if (Input.GetKey(KeyCode.UpArrow)) //|| Input.mousePosition.y >= Screen.height - border)
            {
                position.z += speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.DownArrow))// || Input.mousePosition.y <= border)
            {
                position.z -= speed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.RightArrow))// || Input.mousePosition.x >= Screen.width - border)
            {
                position.x += speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftArrow))// || Input.mousePosition.x <= border)
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
