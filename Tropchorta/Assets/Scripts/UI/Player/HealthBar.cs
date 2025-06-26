using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject point1;
    [SerializeField] private GameObject point2;
    [SerializeField] private GameObject point3;
    [SerializeField] private GameObject point4;
    [SerializeField] private GameObject point5;
    [SerializeField] private GameObject point6;
    [SerializeField] private GameObject point7;
    [SerializeField] private GameObject point8;
    [SerializeField] private GameObject point9;
    [SerializeField] private GameObject point10;
    [SerializeField] private GameObject point11;
    [SerializeField] private GameObject point12;
    [SerializeField] private GameObject point13;
    [SerializeField] private GameObject point14;
    [SerializeField] private GameObject point15;

    private List<GameObject> points = new List<GameObject>();
    [SerializeField] private int initialWidth =10;

    private void Awake()
    {
        points.Add(point15);
        points.Add(point14);
        points.Add(point13);
        points.Add(point12);
        points.Add(point11);
        points.Add(point10);
        points.Add(point9);
        points.Add(point8);
        points.Add(point7);
        points.Add(point6);
        points.Add(point5);
        points.Add(point4);
        points.Add(point3);
        points.Add(point2);
        points.Add(point1);
    }
    public void SetHealth(int health)
    {
        health = Mathf.Clamp(health, 0, initialWidth);

        for (int i = 0; i < points.Count; i++)
        {
            int indexFromEnd = points.Count - 1 - i;
            points[indexFromEnd].SetActive(i < health);
        }
    }
}
