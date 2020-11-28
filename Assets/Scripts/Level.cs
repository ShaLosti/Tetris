using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    private static Level instance;
    Camera mainCamera;
    [SerializeField]
    float leftPoint, rightPoint, downPoint;
    [SerializeField]
    int maxLineBlocks;
    public static Level Instance { get => instance; }
    public float LeftPoint { get => leftPoint; }
    public float RightPoint { get => rightPoint; }
    public float DownPoint { get => downPoint; }
    public float MaxLineBlocks { get => maxLineBlocks; }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Vector2 width, height;
        mainCamera = Camera.main;
        width = mainCamera.ScreenToWorldPoint(new Vector3(0, mainCamera.pixelHeight));
        height = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, 0));
        leftPoint = width.x;
        rightPoint = height.x;
        downPoint = height.y;

        maxLineBlocks = Mathf.RoundToInt((rightPoint - 1f) - (leftPoint + 1));
    }
}
