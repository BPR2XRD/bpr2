using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField]private Rigidbody rb;
    [SerializeField] private float speed;
    private Vector2 moveInput;

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }
    private void MoveLogic()
    {
        Vector3 result = new Vector3(moveInput.x * speed * Time.fixedDeltaTime, 0, moveInput.y * speed * Time.fixedDeltaTime).normalized;
        rb.velocity = result;

    }

    private void FixedUpdate()
    {
        MoveLogic();
    }
}
