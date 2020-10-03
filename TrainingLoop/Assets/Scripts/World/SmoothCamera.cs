using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
	public float maxY;
	public Transform Player;

	public float smooth = 0.125f;
	public Vector3 Offset;

    private void Awake()
    {
		//Offset = transform.position - Player.position;
    }

    private void FixedUpdate()
	{
		Vector3 position = Player.position + Offset;
		position.x = 0f;

		if (Mathf.Abs(position.y) > maxY)
		{
			if (position.y > 0)
				position = new Vector3(0f, maxY, Offset.z);
			else
				position = new Vector3(0f, -maxY, Offset.z);
		}

		transform.position = Vector3.Lerp(transform.position, position, smooth);
	}
}
