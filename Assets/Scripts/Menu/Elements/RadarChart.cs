using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class RadarChart : MonoBehaviour
{
    [SerializeField] private int max = 150;
    [SerializeField] private uint[] values = new uint[6];
    [SerializeField] private Transform[] vertices = new Transform[6];
    [SerializeField] private List<GameObject> lines = new List<GameObject>();
    [SerializeField] private UIPolygon statChart;
    [SerializeField] private UIPolygon tempStatChart;

    public void SetValues(List<int> values)
    {
        for (int i = 0; i < 6; ++i)
            this.values[i] = values[i] < 0 ? 0 : (uint)values[i];
    }

    public void GenerateChart()
    {
        #region Set vertex positions

        float root3 = Mathf.Sqrt(3);
        float scale = 50 / (float) max;

        vertices[0].localPosition = new Vector3(0, values[0] * scale);
        vertices[1].localPosition = new Vector3(values[1] * scale * root3 / 2, values[1] * scale / 2);
        vertices[2].localPosition = new Vector3(values[2] * scale * root3 / 2, -values[2] * scale / 2);
        vertices[3].localPosition = new Vector3(0, -values[3] * scale);
        vertices[4].localPosition = new Vector3(-values[4] * scale * root3 / 2, -values[4] * scale / 2);
        vertices[5].localPosition = new Vector3(-values[5] * scale * root3 / 2, values[5] * scale / 2);

        #endregion

        #region Draw lines between vertices

        Color lineColor = new Color(0, 0, 0, 255);

        List<GameObject> _lines = lines.ToList();
        foreach (GameObject line in _lines)
        {
            Destroy(line);
            lines.Remove(line);
        }

        for (int i = 0; i < 5; ++i)
            DrawLine(vertices[i], vertices[i + 1], lineColor);
        DrawLine(vertices[0], vertices[5], lineColor);

        #endregion

        #region Fill chart

        List<float> tempValues = values.Select(v => (float)v / 150).ToList();
        tempValues.Add( (float)values[0] / 150);
        statChart.DrawPolygon(6, tempValues.ToArray(), 270);

        #endregion
    }

    private void DrawLine(Transform v1, Transform v2, Color color)
    {
        GameObject obj = new GameObject();
        Image img = obj.AddComponent<Image>();
        RectTransform rect = obj.GetComponent<RectTransform>();

        lines.Add(obj);
        img.sprite = Resources.Load<Sprite>("Graphics/Radar Chart/Line");
        img.color = color;
        rect.SetParent(transform);
        rect.localScale = Vector3.one;

        Vector3 a = v1.localPosition;
        Vector3 b = v2.localPosition;

        rect.localPosition = (a + b) / 2;
        Vector3 dif = a - b;
        rect.sizeDelta = new Vector3(dif.magnitude, 1);
        rect.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * (dif.x == 0 ? Mathf.PI / 2 : Mathf.Atan(dif.y / dif.x)) / Mathf.PI));
    }
}
