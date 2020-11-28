using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnBlocks : MonoBehaviour
{
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    GameObject[] blockPrefs;

    private static SpawnBlocks instance;

    public static SpawnBlocks Instance { get => instance; }
    public List<Transform> EngagedGrid { get => engagedGrid; }
    [SerializeField]
    List<Transform> engagedGrid = new List<Transform>();

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SpawnBlock();
    }
    public void SpawnBlock()
    {
        int blockIndex = Random.Range(0, blockPrefs.Length);
        Instantiate(blockPrefs[blockIndex], spawnPoint.position, Quaternion.identity);
    }

    public void AddToGrid(Transform _cell)
    {
        engagedGrid.Add(_cell);
    }

    internal void CheckLine(float y)
    {
        List<Transform> gridLine = new List<Transform>();
        List<Transform> upperGrid = new List<Transform>();

        foreach (var item in engagedGrid)
        {
            if (Mathf.Approximately(item.position.y, y))
            {
                gridLine.Add(item);
            }
            if(item.position.y > y)
            {
                upperGrid.Add(item);
            }
        }

        if(gridLine.Count == Level.Instance.MaxLineBlocks)
        {
            DestroyLine(gridLine);
            ChangeLinesYPos(upperGrid);
            CheckLine(y);
        }
    }

    private void ChangeLinesYPos(List<Transform> upperGrid)
    {
        foreach (var item in upperGrid)
        {
            item.position += Vector3.down;
        }
    }

    private void DestroyLine(List<Transform> gridLine)
    {
        foreach (var item in gridLine)
        {
            engagedGrid.Remove(item);
            Destroy(item.gameObject);
        }
    }
}
