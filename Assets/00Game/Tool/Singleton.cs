using UnityEngine;

public abstract class Singleton<T>:MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // Tìm kiếm instance hiện tại trong scene
                instance = FindObjectOfType<T>();

                // Nếu không tìm thấy, tạo mới một GameObject với instance này
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    instance = singletonObject.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    // Đảm bảo rằng instance không bị phá hủy giữa các scene
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            Destroy(gameObject);  // Hủy bản sao nếu có hơn 1 instance
        }
    }
}
