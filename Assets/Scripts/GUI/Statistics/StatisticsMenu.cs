using System.Linq;
using SpeedTypingGame.Game.Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpeedTypingGame.GUI.Statistics
{
    [AddComponentMenu("SpeedTypingGame/GUI/Statistics/Statistics menu")]
    
    public class StatisticsMenu : Menu
    {
        [SerializeField] private DiagramDrawer accuracyDiagram;
        [FormerlySerializedAs("speedDiagram")] [SerializeField] private DiagramDrawer wpmDiagram;
        [SerializeField] private PersistenceHandler persistenceHandler;
        
        [SerializeField] private TextMeshProUGUI maxAccuracyText;
        [SerializeField] private TextMeshProUGUI averageAccuracyText;
        [SerializeField] private TextMeshProUGUI maxWpmText;
        [SerializeField] private TextMeshProUGUI averageWpmText;
        
        // Methods
        public void OnEnable()
        {
            if (persistenceHandler.ExerciseDataCount > 0)
            {
                maxAccuracyText.SetText($"{persistenceHandler.Accuracy.Max():F2}");
                averageAccuracyText.SetText($"{persistenceHandler.Accuracy.Average():F2}");
                maxWpmText.SetText($"{persistenceHandler.WordsPerMinute.Max():F2}");
                averageWpmText.SetText($"{persistenceHandler.WordsPerMinute.Average():F2}");
            }
            else
            {
                maxAccuracyText.SetText($"{0:F2}");
                averageAccuracyText.SetText($"{0:F2}");
                maxWpmText.SetText($"{0:F2}");
                averageWpmText.SetText($"{0:F2}");
            }

            /*
            List<double> randomData = new List<double>();
            for (int i = 0; i < 500; i++)
            {
                randomData.Add(Mathf.Log(i + 1, 1.5f) + 50 + UnityEngine.Random.Range(-Mathf.Log(i + 1, 15), Mathf.Log(i + 1, 15)));
            }
            */

            accuracyDiagram.UpdateDataPoints(persistenceHandler.Accuracy.TakeLast(500).ToList());
            wpmDiagram.UpdateDataPoints(persistenceHandler.WordsPerMinute.TakeLast(500).ToList());
        }
    }
}