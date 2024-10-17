using UnityEngine;

public class FitObjectWithCamera : MonoBehaviour
{
    private Camera mainCamera;

    // Lưu lại kích thước màn hình ban đầu và scale ban đầu
    private Vector2 initialScreenSize = new Vector2(1920, 1080); // Mặc định kích thước ban đầu là 1920x1080
    private Vector3 initialScale;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        // Lưu lại scale ban đầu của đối tượng
        initialScale = transform.localScale;

        // Chỉ điều chỉnh nếu màn hình hiện tại khác với màn hình ban đầu
        if (Screen.width != initialScreenSize.x || Screen.height != initialScreenSize.y)
        {
            FitToScreen();
        }
    }

    void FitToScreen()
    {
        // Lấy kích thước hiện tại của màn hình
        Vector2 currentScreenSize = new Vector2(Screen.width, Screen.height);

        // Tính toán tỉ lệ thay đổi kích thước màn hình
        float widthRatio = currentScreenSize.x / initialScreenSize.x;
        float heightRatio = currentScreenSize.y / initialScreenSize.y;

        // Chọn tỉ lệ thay đổi nhỏ hơn để giữ đúng tỉ lệ của đối tượng
        float scaleFactor = Mathf.Min(widthRatio, heightRatio);

        // Cập nhật scale mới cho đối tượng dựa trên tỉ lệ thay đổi màn hình
        transform.localScale = new Vector3(initialScale.x * widthRatio, initialScale.y * heightRatio);

        // Cập nhật scale mới cho đối tượng dựa trên tỉ lệ thay đổi màn hình
        //transform.localScale = initialScale * scaleFactor;
    }
    private void Update()
    {
        FitToScreen();
    }
}
