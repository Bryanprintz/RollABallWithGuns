using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
   
   [Header("Balancing Values"),SerializeField]
   private float movementSpeed = 5f;
   [SerializeField]
   private float jumpHeight = 2f;
   [SerializeField]
   private float groundDistance = 0.6f;
   [SerializeField]
   private float dashDistance = 5f;
   [SerializeField] 
   private float ballRotationSpeed = 5f;
   
   [Header("Components"),SerializeField]
   private LayerMask ground;
   [SerializeField]
   private Transform groundChecker;
   [SerializeField] 
   private GameObject ballMesh;

   private Rigidbody rb;
   private Vector3 inputs = Vector3.zero;
   private bool isGrounded = true;

   void Start()
   {
      rb = GetComponent<Rigidbody>();
   }

   void Update()
   {
      isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, ground, QueryTriggerInteraction.Ignore);
      Debug.Log(isGrounded);
      
      inputs = Vector3.zero;
      inputs.x = Input.GetAxis("Horizontal");
      inputs.z = Input.GetAxis("Vertical");
      if (inputs != Vector3.zero)
      {
         transform.forward = inputs;
         ballMesh.transform.Rotate(-inputs.x * ballRotationSpeed, 0.0f, -inputs.z * ballRotationSpeed);
      }

      if (Input.GetButtonDown("Jump") && isGrounded)
      {
         rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
      }
      if (Input.GetButtonDown("Fire1"))
      {
         Vector3 dashVelocity = Vector3.Scale(transform.forward, dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * rb.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * rb.drag + 1)) / -Time.deltaTime)));
         rb.AddForce(dashVelocity, ForceMode.VelocityChange);
      }
   }


   void FixedUpdate()
   {
      rb.MovePosition(rb.position + inputs * movementSpeed * Time.fixedDeltaTime);
   }
   
}
