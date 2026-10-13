using System;
using System.Collections;
using UnityEngine;

public class Motosierra : MonoBehaviour
{
    Rigidbody2D rb;
    public float rotationOffset = 0f;
    float rotationSpeed = 30.0f;
    float rotationAngle = 20.0f;
    public float vibrationLenght = 1.0f;
    public float vibrationTime = 0.01f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.angularVelocity = 1 * rotationSpeed;
        transform.GetChild(1).position = transform.GetChild(0).position;
        StartCoroutine(MoverSierra());

    }

    public Vector3 FloatToVector3(float value)
    {
        Vector3 result;
        result = new Vector3(MathF.Cos(value), MathF.Sin(value), 0);
        return result;
    }

    IEnumerator MoverSierra()
    {
        while(true)
        {
            transform.GetChild(0).GetComponent<Transform>().position = transform.GetChild(1).GetComponent<Transform>().position + FloatToVector3(rb.rotation).normalized * vibrationLenght;
            yield return new WaitForSeconds(vibrationTime);
            transform.GetChild(0).GetComponent<Transform>().position = transform.GetChild(1).GetComponent<Transform>().position - FloatToVector3(rb.rotation).normalized * vibrationLenght;
            yield return new WaitForSeconds(vibrationTime);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if(rb.rotation > rotationAngle + rotationOffset)
        {
            rb.angularVelocity = -1 * rotationSpeed;
        } else if(rb.rotation < -rotationAngle + rotationOffset)
        {
            rb.angularVelocity = 1 * rotationSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("jugador"))
        {

        }
    }

    public void SetRotationOffset(float angulo)
    {
        Debug.Log("SetRotationOffset");
        rotationOffset = angulo;
    }

}
