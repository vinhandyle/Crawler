using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

/// <summary>
/// Defines a hexagon chart to display character stats.
/// </summary>
public class RadarChart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int max = 150;
    [SerializeField] private uint[] values = new uint[6];
    [SerializeField] private Transform[] icons = new Transform[6];
    [SerializeField] private Transform lineParent;
    [SerializeField] private List<GameObject> lines = new List<GameObject>();
    [SerializeField] private UIPolygon statChart;

    [SerializeField] private GameObject displayLvl;

    [Header("Level Up")]
    [SerializeField] private Transform tempLineParent;
    [SerializeField] private List<GameObject> tempLines = new List<GameObject>();
    [SerializeField] private UIPolygon tempStatChart;
    [SerializeField] private Button trigger;

    private float root3 = Mathf.Sqrt(3);
    private float scale;

    private void Awake()
    {
        scale = 50 / (float)max;
        DrawIcons();
    }

    private void OnDisable()
    {
        displayLvl.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        displayLvl.SetActive(true);
        displayLvl.GetComponentInChildren<Text>().text = string.Format(
            "VIT: {0}\nARC: {1}\nAGI: {2}\nSTR: {3}\nDEX: {4}\nINT: {5}",
            values[5],
            values[1],
            values[3],
            values[4],
            values[2],
            values[0]
            );
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        displayLvl.SetActive(false);
    }

    /// <summary>
    /// Loads the click functionality of this radar chart.
    /// </summary>
    public void SetClickBehavior(Menu menu)
    {
        if (menu == null || trigger == null) return;

        trigger.onClick.AddListener( 
            () => 
            {
                if (menu.open) menu.Close();
                else menu.Open(); 
            }
        );
    }

    /// <summary>
    /// Set the distances of the vertices.
    /// </summary>
    public void SetValues(List<int> values)
    {
        for (int i = 0; i < 6; ++i)
            this.values[i] = values[i] < 0 ? 0 : (uint)values[i];
    }

    /// <summary>
    /// Generate the radar chart given the current information.
    /// </summary>
    public void GenerateChart()
    {
        DrawChart(values.Select(v => (int)v).ToList(), lines, lineParent, new Color(0, 255, 0, 255), statChart);
        GenerateTempChart(new List<int>() { 0, 0, 0, 0, 0, 0 });
    }

    /// <summary>
    /// Generate a temporary radar chart given the current information and the specified increments.
    /// </summary>
    public void GenerateTempChart(List<int> deltas)
    {
        List<int> tempValues = values.Select(v => (int)v).ToList();

        for (int i = 0; i < tempValues.Count; ++i)
        {
            tempValues[i] += (tempValues[i] < 150) ? deltas[i] : 0;
        }

        DrawChart(tempValues, tempLines, tempLineParent, new Color(255, 0, 0, 255), tempStatChart);
    }

    /// <summary>
    /// Place the icons at the vertices of the hexagon base.
    /// </summary>
    private void DrawIcons()
    {
        int iconPos = max + 20;

        icons[0].localPosition = new Vector3(0, iconPos * scale);
        icons[1].localPosition = new Vector3(iconPos * scale * root3 / 2, iconPos * scale / 2);
        icons[2].localPosition = new Vector3(iconPos * scale * root3 / 2, -iconPos * scale / 2);
        icons[3].localPosition = new Vector3(0, -iconPos * scale);
        icons[4].localPosition = new Vector3(-iconPos * scale * root3 / 2, -iconPos * scale / 2);
        icons[5].localPosition = new Vector3(-iconPos * scale * root3 / 2, iconPos * scale / 2);
    }

    /// <summary>
    /// Draw a line with the specified color from point a to point b.
    /// </summary>
    private void DrawLine(Vector3 a, Vector3 b, Color color, List<GameObject> lines, Transform parent)
    {
        GameObject obj = new GameObject();
        Image img = obj.AddComponent<Image>();
        RectTransform rect = obj.GetComponent<RectTransform>();

        lines.Add(obj);
        img.sprite = Resources.Load<Sprite>("Graphics/Radar Chart/Line");
        img.color = color;
        rect.SetParent(parent);
        rect.localScale = Vector3.one;

        rect.localPosition = (a + b) / 2;
        Vector3 dif = a - b;
        rect.sizeDelta = new Vector3(dif.magnitude, 1);
        rect.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * (dif.x == 0 ? Mathf.PI / 2 : Mathf.Atan(dif.y / dif.x)) / Mathf.PI));
    }

    /// <summary>
    /// Draw a radar chart with the specified information.
    /// </summary>
    private void DrawChart(List<int> values, List<GameObject> lines, Transform lineParent, Color lineColor, UIPolygon chart)
    {
        #region Set vertex positions

        List<Vector3> points = new List<Vector3>()
        {
            new Vector3(0, values[0] * scale),
            new Vector3(values[1] * scale * root3 / 2, values[1] * scale / 2),
            new Vector3(values[2] * scale * root3 / 2, -values[2] * scale / 2),
            new Vector3(0, -values[3] * scale),
            new Vector3(-values[4] * scale * root3 / 2, -values[4] * scale / 2),
            new Vector3(-values[5] * scale * root3 / 2, values[5] * scale / 2)
        };

        #endregion

        #region Draw lines between vertices

        List<GameObject> _lines = lines.ToList();
        foreach (GameObject line in _lines)
        {
            Destroy(line);
            lines.Remove(line);
        }

        for (int i = 0; i < 5; ++i)
            DrawLine(points[i], points[i + 1], lineColor, lines, lineParent);
        DrawLine(points[0], points[5], lineColor, lines, lineParent);

        #endregion

        #region Fill chart

        List<float> tempValues = values.Select(v => (float)v / 150).ToList();
        tempValues.Add((float)values[0] / 150);
        chart.DrawPolygon(6, tempValues.ToArray(), 270);
        chart.UpdatePolygon();

        #endregion
    }
}
