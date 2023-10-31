using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb; 
    public GameObject body;
    public float dashForce = 1f;
    public float speed = 1f;
    public float dashCooldown = 2f;
    public float dashTime = 1f;
    public bool canDash;
    public bool isDashing;

    void Awake()
    {
        rb = this.gameObject.transform.GetChild(0).GetComponent<Rigidbody2D>();
        canDash = true;
        isDashing = false;
    }

    void Update()
    {
        move();
    }

    private void move() 
    {
        if(isDashing)
        {
            return;
        }

        if(Input.GetKeyDown("space") && canDash) {
            StartCoroutine(Dash());
        }

        if(Input.GetKey("w")) {
            transform.position += body.transform.up * speed * Time.deltaTime;
        }
        else if(Input.GetKey("s")) {
            transform.position += -body.transform.up * (speed/2) * Time.deltaTime; 
        }
        else if(Input.GetKey("d")) {
            transform.position += body.transform.right * (speed/2) * Time.deltaTime;
        }
        else if(Input.GetKey("a")) {
            transform.position += -body.transform.right * (speed/2) * Time.deltaTime;
        }
    }

    public IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        for(float i = 0; i < dashTime; i += Time.deltaTime)
        {
            transform.position += body.transform.up * dashForce * Time.deltaTime;
            yield return null;
        }

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }
}
