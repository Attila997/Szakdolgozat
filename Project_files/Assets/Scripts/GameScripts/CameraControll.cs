using System;
using UnityEngine;

[Flags]
public enum Direction
{
	None = 0,
	Horizontal = 1,
	Vertical = 2,
	Both = 3
}

public class CameraControll : MonoBehaviour
{
	public Transform target;
	public float smoothSpeed = 1f;
	public Direction followType = Direction.Both;
	[Range(0.0f, 1.0f)]
	public float cameraCenterX = 0.5f;
	[Range(0.0f, 1.0f)]
	public float cameraCenterY = 0.5f;
	public Direction boundType = Direction.None;
	public float leftBound = 0;
	public float rightBound = 0;
	public float upperBound = 0;
	public float lowerBound = 0;
	public Direction deadZoneType = Direction.None;
	public float leftDeadBound = 0;
	public float rightDeadBound = 0;
	public float upperDeadBound = 0;
	public float lowerDeadBound = 0;

	Camera camera;
	Vector3 tempVec;
	bool isBoundHorizontal;
	bool isBoundVertical;
	bool isFollowHorizontal;
	bool isFollowVertical;
	bool isDeadZoneHorizontal;
	bool isDeadZoneVertical;
	Vector3 velocity = Vector3.zero;
	Vector3 deltaCenterVec;

	private void Start()
	{
		camera = GetComponent<Camera>();

		isFollowHorizontal = (followType & Direction.Horizontal) == Direction.Horizontal;
		isFollowVertical = (followType & Direction.Vertical) == Direction.Vertical;
		isBoundHorizontal = (boundType & Direction.Horizontal) == Direction.Horizontal;
		isBoundVertical = (boundType & Direction.Vertical) == Direction.Vertical;
		isDeadZoneHorizontal = ((deadZoneType & Direction.Horizontal) == Direction.Horizontal) && isFollowHorizontal;
		isDeadZoneVertical = ((deadZoneType & Direction.Vertical) == Direction.Vertical) && isFollowVertical;
		tempVec = Vector3.one;
		deltaCenterVec = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0))
						- camera.ViewportToWorldPoint(new Vector3(cameraCenterX, cameraCenterY, 0));

	}

	void FixedUpdate()
	{
		if (target)
		{
			Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(cameraCenterX, cameraCenterY, 0));

			if (!isFollowHorizontal)
			{
				delta.x = 0;
			}
			if (!isFollowVertical)
			{
				delta.y = 0;
			}

			Vector3 desiredPosition = target.position + delta;
			tempVec = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

			if (isDeadZoneHorizontal)
			{
				if (delta.x > rightDeadBound)
				{
					tempVec.x = target.position.x - rightDeadBound + deltaCenterVec.x;
				}
				if (delta.x < -leftDeadBound)
				{
					tempVec.x = target.position.x + leftDeadBound + deltaCenterVec.x;
				}
			}
			if (isDeadZoneVertical)
			{
				if (delta.y > upperDeadBound)
				{
					tempVec.y = target.position.y - upperDeadBound + deltaCenterVec.y;
				}
				if (delta.y < -lowerDeadBound)
				{
					tempVec.y = target.position.y + lowerDeadBound + deltaCenterVec.y;
				}
			}

			tempVec.z = transform.position.z;
			transform.position = tempVec;
		}

	}
}