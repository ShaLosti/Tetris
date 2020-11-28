using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TetrisBlock : MonoBehaviour
{
    [SerializeField]
    float maxTickTime = .8f;
    float tickTime;
    [SerializeField]
    Transform[] blocks;

    [SerializeField]
    Transform rotationPoint;
    [SerializeField]
    short rotateAngle = 90;

    private void Start()
    {
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (ValidPosition(PositionChangeType.Right))
            {
                transform.position += Vector3.right;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (ValidPosition(PositionChangeType.Left))
            {
                transform.position += Vector3.left;
            }
        }
        tickTime += Time.deltaTime;
        if (tickTime > (Input.GetKey(KeyCode.DownArrow) ? maxTickTime / 10 : maxTickTime)
            && ValidPosition(PositionChangeType.Down))
        {
            tickTime = 0;
            transform.position += Vector3.down;
        }
        else if (tickTime > (Input.GetKey(KeyCode.DownArrow) ? maxTickTime / 10 : maxTickTime))
        {
            this.enabled = false;
            SpawnBlocks.Instance.SpawnBlock();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint.localPosition), Vector3.forward, rotateAngle);
            if (!ValidPosition(PositionChangeType.All))
                transform.RotateAround(transform.TransformPoint(rotationPoint.localPosition), Vector3.forward, -rotateAngle);
        }
    }

    private bool ValidPosition(PositionChangeType positionChangeType)
    {
        foreach (Transform item in blocks)
        {
            switch (positionChangeType)
            {
                case PositionChangeType.Left:
                    if (item.position.x - 1 <= Level.Instance.LeftPoint + 1
                        || SpawnBlocks.Instance.EngagedGrid.Find(x => x.position == item.position + Vector3.left) != null)
                        return false;
                    break;
                case PositionChangeType.Right:
                    if (item.position.x + 1 >= Level.Instance.RightPoint - 1
                        || SpawnBlocks.Instance.EngagedGrid.Find(x => x.position == item.position + Vector3.right) != null)
                        return false;
                    break;
                case PositionChangeType.Down:
                    if (item.position.y - 1 <= Level.Instance.DownPoint + 1
                        || SpawnBlocks.Instance.EngagedGrid.Find(x => x.position == item.position + Vector3.down) != null)
                    {
                        float minY = blocks[0].position.y;
                        foreach (var x in blocks)
                        {
                            SpawnBlocks.Instance.AddToGrid(x.transform);
                            minY = Mathf.Min(x.transform.position.y, minY);
                        }
                        SpawnBlocks.Instance.CheckLine(minY);
                        return false;
                    }
                    break;
                case PositionChangeType.All:
                    if (item.position.x - 1 <= Level.Instance.LeftPoint
                        || item.position.x + 1 >= Level.Instance.RightPoint
                        || item.position.y - 1 <= Level.Instance.DownPoint
                        || SpawnBlocks.Instance.EngagedGrid.Find(x=>(x.position == item.position + Vector3.left)
                            || (x.position == item.position + Vector3.right) 
                            || (x.position == item.position + Vector3.down)) != null)
                        return false;
                    break;
            }
        }
        return true;
    }
    
    public enum PositionChangeType
    {
        Left,
        Right,
        Down,
        All,
        None,
    }
}
