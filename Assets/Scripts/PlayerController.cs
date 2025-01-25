using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  [SerializeField] private GameObject cameraHolder;
  [SerializeField] private float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;
  private bool grounded;
  private float verticalLookRotation;
  private Vector3 smoothMoveVelocity;
  private Vector3 moveAmount;
  private Rigidbody rb;
  private PhotonView PV;
  void Awake()
  {
    rb = GetComponent<Rigidbody>();
    PV = GetComponent<PhotonView>();
  }

  private void Start()
  {
    if (!PV.IsMine)
    {
      Destroy(GetComponentInChildren<Camera>().gameObject);
      Destroy(rb);
    }
  }

  void Update()
  {
    if (!PV.IsMine)
    {
      return;
    }
    Look();
    Move();
    Jump();
  }

  void Look()
  {
    transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
    verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
    verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

    cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
  }

  void Move()
  {
    Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed),
      ref smoothMoveVelocity, smoothTime);
   
  }

  void Jump()
  {
    if (Input.GetKeyDown(KeyCode.Space) && grounded)
    {
      rb.AddForce(transform.up*jumpForce);
    }
  }
  public void SetGroundedState(bool _grounded)
  {
    grounded = _grounded;
  }

  private void FixedUpdate()
  {
    if (!PV.IsMine)
    {
      return;
    }
    rb.MovePosition(rb.position+transform.TransformDirection(moveAmount)*Time.fixedDeltaTime);
  }
}
