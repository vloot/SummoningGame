using UnityEngine;
using System.Collections.Generic;

namespace DebugTools
{
    public class ScreenLogger : MonoBehaviour
    {
        [Header("Base rect config")]
        [SerializeField] private float baseX;
        [SerializeField] private float baseY;
        [SerializeField] private float baseWidthPerChar;
        [SerializeField] private float baseHeight;

        private Dictionary<string, ScreenLoggerLine> lines;

        public static ScreenLogger Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                lines = new Dictionary<string, ScreenLoggerLine>();
            }
        }


        public void AddLine(string lineLabel, string lineText)
        {
            var line = new ScreenLoggerLine(lineLabel, lineText);
            lines[lineLabel] = line;
        }

        public void UpdateLine(string lineLabel, string newText)
        {
            lines[lineLabel].UpdateLineText(newText);
        }

        private void OnGUI()
        {
            int i = 0;
            foreach (KeyValuePair<string, ScreenLoggerLine> item in lines)
            {
                GUI.Label(new Rect(baseX, baseY * (i + 1), item.Value.Length * baseWidthPerChar, baseHeight), item.Value.Line);
                i++;
            }
        }
    }
}
