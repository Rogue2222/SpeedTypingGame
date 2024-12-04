using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SpeedTypingGame.Game.Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GUI.Statistics
{
    public class DiagramDrawer : MonoBehaviour
    {
        // TODO remove after merge
        private int _exerciseCount;
    
        
        [SerializeField] private RectTransform diagramSpace;
        [SerializeField] private LineRenderer statLineRenderer;
        [SerializeField] private LineRenderer tickLineRenderer;
        [SerializeField] private TextMeshProUGUI xLabel;
        [SerializeField] private TextMeshProUGUI yLabel;
        [SerializeField] private GameObject tickLabels;
    
        private List<double> _dataPoints;
        private Vector3[] _linePoints;
        
        void Awake()
        {
            _dataPoints = new List<double>();
            SetTickLines();
        }
    
        private void UpdateLinePoints(double min = 0)
        {
            statLineRenderer.positionCount = _exerciseCount;
            
            double max = _dataPoints.Max();
            
            _linePoints = new Vector3[_exerciseCount];
            for (int i = 0; i < _exerciseCount; i++)
            {
                if (i == 0)
                {
                    _linePoints[i] = new Vector3(0, 0, -5);
                }

                // Create equal steps and therefore fill diagram space
                float offset = diagramSpace.rect.width / (_exerciseCount - 1);
                float x = i * offset;
            
                // normalize results
                double y = (_dataPoints[i] - min) / (max - min) * diagramSpace.rect.height;
            
                _linePoints[i] = new Vector3(x, (float)y, -1);
                statLineRenderer.SetPosition(i, _linePoints[i]);
            }
        }

        private void SetTickLines()
        {
            tickLineRenderer.positionCount = 14;
            for (int i = 0; i < 14; ++i)
            {
                float x;
                if ((i + 3) % 4 >= 2)
                    x = -4;
                else
                    x = 4 + diagramSpace.rect.width;
                
                float y = (Mathf.Floor(i / 2.0f) + 1) * diagramSpace.rect.height / 8;
                
                tickLineRenderer.SetPosition(i, new Vector3(x, y, -1));
            }
        }

        private void UpdateTicks()
        {
            // Update the ticks on the Y axis
            for (int i = 0; i < tickLabels.transform.childCount; i++)
            {
                double value = Mathf.Lerp((float)_dataPoints.Max(), (float)_dataPoints.Min(), i / 8f);
                
                tickLabels.transform.GetChild(i).GetComponent<TextMeshProUGUI>().SetText($"{value:F2}"); 
            }
        }

        public void UpdateDataPoints(List<double> newDataPoints)
        {
            _dataPoints = newDataPoints;
            _exerciseCount = newDataPoints.Count;
            statLineRenderer.widthMultiplier = Mathf.Lerp(0.05f, 0.021f, _exerciseCount / 500.0f);
            UpdateLinePoints(_dataPoints.Min());
            UpdateTicks();
        }
    }
}
