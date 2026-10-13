using UnityEngine;

public class SawPingPong : MonoBehaviour
{
    public GameObject startPoint;
    public GameObject endPoint;
    public float speed = 2f;

    void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1); // Oscila entre 0 y 1
        transform.position = Vector3.Lerp(startPoint.transform.position, endPoint.transform.position, t);
    }
}
