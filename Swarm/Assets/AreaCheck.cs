using UnityEngine;

public class AreaCheck : MonoBehaviour
{
    private void Update()
    {
        if (Camera.main.WorldToScreenPoint(transform.position).y < 0)
        {
            transform.position = new Vector3(Screen.width - transform.position.x, Screen.height, 0);
        }
        else if (transform.position.y > Screen.height)
        {
            transform.position = new Vector3(Screen.width - transform.position.x, 0, 0);
        }
        else if (Camera.main.WorldToScreenPoint(transform.position).x < 0)
        {
            transform.position = new Vector3(Screen.width, Screen.height - transform.position.y, 0);
        }
        else if (transform.position.x > Screen.width)
        {
            transform.position = new Vector3(0, Screen.height - transform.position.y, 0);
        }
    }
}