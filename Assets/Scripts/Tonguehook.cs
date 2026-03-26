/*****************************************************************************
// File Name : Comments.cs
// Author : Alexander R. Safranek
// Creation Date : January 27, 2026
// Last Updated : 3/25/2026 
// Brief Description : Adds a grappling hook with rope physics and swing forces.
*****************************************************************************/
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles grappling hook shooting and swinging mechanics.
/// </summary>
public class Tonguehook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody rb;
    [SerializeField] Camera playerCamera;
    [SerializeField] LineRenderer lineRenderer;

    [Header("Grapple Settings")]
    [SerializeField] float ropeExtendSpeed = 50f;
    [SerializeField] float maxDistance = 30f;

    [Header("Swing Settings")]
    [SerializeField] float swingForce = 30f;
    [SerializeField] float swingPullForce = 8f;

    InputAction grappleAction;

    Vector3 grapplePoint;
    bool isGrappling;
    bool ropeExtending;
    float ropeLength;

    SpringJoint joint;
    PlayerMove playerMove;

    /// <summary>
    /// Initializes grapple references and line renderer.
    /// </summary>
    void Start()
    {
        playerMove = GetComponent<PlayerMove>();

        grappleAction = InputSystem.actions.FindAction("Grapple");
        if (grappleAction != null)
            grappleAction.performed += StartGrapple;

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;
            lineRenderer.startWidth = 0.7f;
            lineRenderer.endWidth = 0.7f;
        }

        if (playerCamera == null)
            playerCamera = Camera.main;
    }

    /// <summary>
    /// Updates rope visuals and checks release input.
    /// </summary>
    void Update()
    {
        if (grappleAction != null && grappleAction.WasReleasedThisFrame())
        {
            StopGrapple();
        }

        if (isGrappling && ropeExtending && lineRenderer != null)
        {
            Vector3 direction = (grapplePoint - transform.position).normalized;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + direction * ropeLength);
        }
    }

    /// <summary>
    /// Applies physics forces for rope extension or swing.
    /// </summary>
    void FixedUpdate()
    {
        if (!isGrappling) return;

        Vector3 direction = (grapplePoint - transform.position).normalized;

        if (ropeExtending)
        {
            ropeLength += ropeExtendSpeed * Time.fixedDeltaTime;

            float distanceToPoint = Vector3.Distance(transform.position, grapplePoint);

            if (ropeLength >= distanceToPoint)
            {
                ropeLength = distanceToPoint;
                ropeExtending = false;
            }

            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position + direction * ropeLength);
            }
        }
        else
        {
            if (playerCamera != null)
                rb.AddForce(playerCamera.transform.forward * swingForce, ForceMode.Acceleration);

            rb.AddForce(direction * swingPullForce, ForceMode.Acceleration);

            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, grapplePoint);
            }
        }
    }

    /// <summary>
    /// Starts grappling hook towards hit point.
    /// </summary>
    /// <param name="context">Input callback context for grapple</param>
    void StartGrapple(InputAction.CallbackContext context)
    {
        if (this == null || !gameObject.activeInHierarchy) return;

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                Debug.LogWarning("Tonguehook: No camera found, cannot grapple.");
                return;
            }
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            grapplePoint = hit.point;
            isGrappling = true;
            ropeExtending = true;
            ropeLength = 0f;

            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distance = Vector3.Distance(transform.position, grapplePoint);
            joint.maxDistance = distance * 0.8f;
            joint.minDistance = distance * 0.25f;
            joint.spring = 3f;
            joint.damper = 5f;
            joint.massScale = 4.5f;

            if (playerMove != null)
                playerMove.isGrappling = true;

            if (lineRenderer != null)
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position);
            }
        }
    }

    /// <summary>
    /// Stops grappling hook and disables joint and rope.
    /// </summary>
    void StopGrapple()
    {
        isGrappling = false;
        ropeExtending = false;

        if (joint != null)
            Destroy(joint);

        if (playerMove != null)
            playerMove.isGrappling = false;

        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }

    /// <summary>
    /// Unsubscribes input actions on destroy to prevent errors.
    /// </summary>
    private void OnDestroy()
    {
        if (grappleAction != null)
            grappleAction.performed -= StartGrapple;
    }
}