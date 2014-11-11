using UnityEngine;
using System.Collections;

public class PlayerManager : Photon.MonoBehaviour
{
	public float speed = 10f;
	public bool debug = false;

	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;
	
	void Update()
	{
		if (photonView.isMine)
		{
			InputMovement();
		}
		else
		{
			SyncedMovement();
		}
	}
	
	void InputMovement()
	{
		if (Input.GetKey(KeyCode.W))
		{
			rigidbody.MovePosition(rigidbody.position + Vector3.forward * speed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.S))
		{
			rigidbody.MovePosition(rigidbody.position - Vector3.forward * speed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.D))
		{
			rigidbody.MovePosition(rigidbody.position + Vector3.right * speed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.A))
		{
			rigidbody.MovePosition(rigidbody.position - Vector3.right * speed * Time.deltaTime);
		}

		if (Input.GetKeyDown(KeyCode.B))
		{
			if(debug == true)
			{
				debug = false;
			}
			else
			{
				debug = true;
			}
		}
	}

	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		rigidbody.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(rigidbody.position);
			stream.SendNext(rigidbody.velocity);
		}
		else
		{
			Vector3 syncPosition = (Vector3)stream.ReceiveNext();
			Vector3 syncVelocity = (Vector3)stream.ReceiveNext();
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;

			syncEndPosition = syncPosition + syncVelocity * syncDelay;
			syncStartPosition = rigidbody.position;
		}
	}

	void OnGUI()
	{
		if(debug == true)
		{
			GUI.Label(new Rect(30, 20, 250, 20), "Debug: deltaTime: "+Time.deltaTime.ToString());
			GUI.Label(new Rect(30, 35, 250, 20), "Debug: syncTime: "+syncTime.ToString());
			GUI.Label(new Rect(30, 50, 250, 20), "Debug: syncDelay: "+syncDelay.ToString());
			GUI.Label(new Rect(30, 65, 250, 20), "Debug: syncStartPosition: "+syncEndPosition.ToString());
			GUI.Label(new Rect(30, 80, 250, 20), "Debug: syncEndPosition: "+syncStartPosition.ToString());
			GUI.Label(new Rect(30, 95, 250, 20), "Debug: transformPosition: "+transform.position.ToString());
		}
	}
}
