using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GUI.Statistics
{
    public class DiagramDrawer : MonoBehaviour
    {
        // TODO remove after merge
        private readonly int _exerciseCount = 30;
    
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI xLabel;
        [SerializeField] private TextMeshProUGUI yLabel;
        [SerializeField] private GameObject tickLabels;
    
        private List<float> _dataPoints;
        private Vector3[] _linePoints;
        
        void Awake()
        {
            _dataPoints = new List<float>();
        }

        private void OnEnable()
        {
            UpdateDataPoints();
            UpdateLinePoints(_dataPoints.Min());
        }
    
        void UpdateLinePoints(float min = 0)
        {
            lineRenderer.positionCount = _exerciseCount;
            
            float max = _dataPoints.Max();
            
            _linePoints = new Vector3[_exerciseCount];
            for (int i = 0; i < _exerciseCount; i++)
            {
                if (i == 0)
                {
                    _linePoints[i] = new Vector3(0, 0, -5);
                }

                // Create equal steps and therefore fill diagram space
                float offset = rectTransform.rect.width / (_exerciseCount - 1);
                float x = i * offset;
            
                // normalize results
                float y = (_dataPoints[i] - min) / (max - min) * rectTransform.rect.height;
            
                _linePoints[i] = new Vector3(x, y, -1);
                lineRenderer.SetPosition(i, _linePoints[i]);
            }
            
            // Update the ticks on the Y axis
            for (int i = 0; i < tickLabels.transform.childCount; i++)
            {
                float value = Mathf.Lerp(_dataPoints.Max(), _dataPoints.Min(), i / 8f);
                
                tickLabels.transform.GetChild(i).GetComponent<TextMeshProUGUI>().SetText(
                    Math.Round(value, 2).ToString(CultureInfo.CurrentUICulture)); 
            }
        }

        void UpdateDataPoints()
        {
            _dataPoints.Clear();
            for (int i = 0; i < _exerciseCount; i++)
            {
                _dataPoints.Add(Random.Range(80f,95f));
            }
        }
    }
}
