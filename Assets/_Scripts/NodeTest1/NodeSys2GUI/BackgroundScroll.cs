using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float scrollSensitivity;
    public static Vector2 zoom;

    // Update is called once per frame
    void Update()
    {
        zoom = new Vector2(
            transform.localScale.x + Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollSensitivity
            , transform.localScale.y + Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scrollSensitivity);
        transform.localScale = zoom;
    }
}
