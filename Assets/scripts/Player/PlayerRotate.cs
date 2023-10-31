using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotate : MonoBehaviour
{
    private Vector3 mousePosition;

    void FixedUpdate()
    {
        mousePosition = Input.mousePosition;
        rotate();
    }

    private void rotate()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.up = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
    }
}
