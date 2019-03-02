using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookController : MonoBehaviour
{
	private const int PLAYER_LAYER = 8;
	private const int PLAYER_WEAPONS_LAYER = 9;

	public KeyCode fireKey = KeyCode.E;
	public GameObject grapplingHook;

	private bool hasFiredHook = false;

	void Start()
	{
		Physics2D.IgnoreLayerCollision(PLAYER_LAYER, PLAYER_WEAPONS_LAYER);
	}

	void Update()
	{
		if (Input.GetKeyDown(fireKey))
			FireGrapplingHook();
	}

	private void FireGrapplingHook()
	{
		if (hasFiredHook)
			return;

		GrapplingHook firedGrapplingHook = Instantiate(grapplingHook, transform.position, Quaternion.identity, transform.parent)
				.GetComponent<GrapplingHook>();
		firedGrapplingHook.controller = this;
		hasFiredHook = true;
	}

	public void OnGrapplingHookStopped()
	{
		hasFiredHook = false;
	}
}
