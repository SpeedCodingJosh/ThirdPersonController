using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour 
{
	public Vector3 move;
	public bool isRunning, jumped;

	public Camera mainCamera;
	public MouseLook mouseLook;

	private float horizontal, vertical;
	private Vector3 cameraForward;

	private void Start () 
	{
		if(mainCamera == null)
		{
			mainCamera = Camera.main;
		}
		//mouseLook = new MouseLook();

		// Apply init values
		mouseLook.Init(transform, mainCamera.transform);
	}
	
	// Update is called once per frame
	private void Update () 
	{
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");

		isRunning = Input.GetKey(KeyCode.LeftShift);
		jumped = Input.GetKey(KeyCode.Space);

		cameraForward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;
		move = vertical * cameraForward + horizontal * mainCamera.transform.right;
		
		mouseLook.LookAround(transform, mainCamera.transform);
	}

	public bool StopWalking ()
	{
		return Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0;
	}
}

[System.Serializable]
public class MouseLook 
{
	public float xSpeed = 2, ySpeed = 2, smoothAmount = 5;
	public float minimumX, maximumX;
	public bool lockCursor = true, smooth = false;
	private float xRot, yRot;

	private Quaternion charRotation, camRotation;

	public void Init (Transform character, Transform cam)
	{
		charRotation = character.localRotation;
		camRotation = cam.localRotation;
	}

	public void LookAround (Transform character, Transform cam)
	{
		xRot = Input.GetAxis("Mouse X") * xSpeed;
		yRot = Input.GetAxis("Mouse Y") * ySpeed;

		charRotation *= Quaternion.Euler(0, xRot, 0);
		camRotation *= Quaternion.Euler(-yRot, 0, 0);

		// Clamp the X rotation axis
		camRotation = ClampRotationAroundXAxis(camRotation);

		// Smooth movement
		if(smooth)
		{
			character.localRotation = Quaternion.Slerp(character.localRotation, charRotation, smoothAmount * Time.deltaTime);
			cam.localRotation = Quaternion.Slerp(cam.localRotation, camRotation, smoothAmount * Time.deltaTime);
		}
		else
		{
			character.localRotation = charRotation;
			cam.localRotation = camRotation;
		}

		// Cursor locked
		LockCursor();
	}

	private void LockCursor ()
	{
		Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
		Cursor.visible = !lockCursor;
	}

	private Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;

		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

		angleX = Mathf.Clamp (angleX, minimumX, maximumX);

		q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

		return q;
	}
}