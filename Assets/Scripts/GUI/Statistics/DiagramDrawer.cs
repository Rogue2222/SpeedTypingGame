using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace GUI.Statistics
{
    [AddComponentMenu("SpeedTypingGame/GUI/Statistics/Diagram drawer")]
    public class DiagramDrawer : MonoBehaviour
    {
        // TODO remove after merge
        private int _exerciseCount;
    
        [SerializeField] private RectTransform diagramSpace;
        [SerializeField] private LineRenderer statLineRenderer;
        [SerializeField] private LineRenderer tickLineRenderer;
        [SerializeField] private TextMeshProUGUI xLabel;
        [SerializeField] private TextMeshProUGUI yLabel;
        [SerializeField] private TextMeshProUGUI xMinTick;
        [SerializeField] private TextMeshProUGUI xMaxTick;
        [SerializeField] private GameObject yTickLabels;
    
        private List<float> _dataPoints;
        
        void Awake()
        {
            _dataPoints = new List<float>();
            SetTickLines();
        }
    
        private void UpdateLinePoints(float min = 0)
        {
            statLineRenderer.positionCount = _exerciseCount;
            
            statLineRenderer.widthMultiplier = Mathf.Lerp(0.05f, 0.021f, _exerciseCount / 500.0f);
            
            float max = _dataPoints.Max();
            
            for (int i = 0; i < _exerciseCount; i++)
            {
                // Create equal steps and therefore fill diagram space
                float offset = diagramSpace.rect.width / (_exerciseCount - 1);
                float x = i * offset;
            
                // normalize results
                float y = (_dataPoints[i] - min) / (max - min) * diagramSpace.rect.height;
                
                statLineRenderer.SetPosition(i, new Vector3(x, y, -1));
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
            xMinTick.SetText("1");
            xMaxTick.SetText($"{_exerciseCount}");
            
            for (int i = 0; i < yTickLabels.transform.childCount; i++)
            {
                float value = Mathf.Lerp(_dataPoints.Max(), _dataPoints.Min(), i / 8f);
                yTickLabels.transform.GetChild(i).GetComponent<TextMeshProUGUI>().SetText($"{value:F2}"); 
            }
        }

        private void Reset() {
            xMinTick.SetText("0");
            xMaxTick.SetText("0");
            statLineRenderer.positionCount = 0;
            statLineRenderer.SetPositions(new Vector3[]{});
            for (int i = 0; i < yTickLabels.transform.childCount; i++)
            {
                yTickLabels.transform.GetChild(i).GetComponent<TextMeshProUGUI>().SetText(0.0f.ToString("F2")); 
            }
        }

        private void DrawOne() {
            xMinTick.SetText("1");
            xMaxTick.SetText("1");
            for (int i = 0; i < yTickLabels.transform.childCount; i++)
            {
                float value = Mathf.Lerp(_dataPoints[0] + _dataPoints[0] / 10,
                                         _dataPoints[0] - _dataPoints[0] / 10, 
                                         i / 8f);
                yTickLabels.transform.GetChild(i).GetComponent<TextMeshProUGUI>().SetText($"{value:F2}"); 
            }

            statLineRenderer.positionCount = 2;
            statLineRenderer.widthMultiplier = 0.05f;
            statLineRenderer.SetPosition(0, 
                new Vector3(0, 0.5f * diagramSpace.rect.height, -1));
            statLineRenderer.SetPosition(1, 
                new Vector3(diagramSpace.rect.width, 0.5f * diagramSpace.rect.height, -1));
        }

        public void UpdateDataPoints(List<double> newDataPoints)
        {
            _dataPoints = newDataPoints.Select(x => (float)x).ToList();
            _exerciseCount = newDataPoints.Count;
            
            if (_exerciseCount == 0) {
                Reset();
                return;
            }
            
            if (_exerciseCount == 1) {
                DrawOne();
                return;
            }
            
            UpdateLinePoints(_dataPoints.Min());
            UpdateTicks();
        }
    }
}
