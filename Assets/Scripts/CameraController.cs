using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        if (!GameManager.Instance.isGameOver)
        {
            transform.position = player.transform.position + offset;
        }
    }
}
