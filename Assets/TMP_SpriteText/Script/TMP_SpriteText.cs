using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace pruss.Tool.TextMeshPro
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    [DisallowMultipleComponent]
// ReSharper disable once InconsistentNaming
    public class TMP_SpriteText : MonoBehaviour
    {
        [SerializeField] [HideInInspector] private TMP_Text tmpText;
        [SerializeField] private TMP_SpriteAsset spriteAsset;
        public TMP_Text TMPText => tmpText;
        private readonly Dictionary<char, string> _spriteTagCache = new();
        [SerializeField] [HideInInspector] private string originalText;
        [SerializeField] private TextAlignmentOptions textAlignment;

        // ReSharper disable once InconsistentNaming
        public string m_text;
        private readonly StringBuilder _stringBuilder = new StringBuilder(11 * 15);

        // ReSharper disable once InconsistentNaming
        public string text
        {
            get => FromSpriteText(tmpText.text);
            set
            {
                originalText = value;
                tmpText.text = ToSpriteText(value);
            }
        }

        private string ToSpriteText(string input)
        {
            _stringBuilder.Clear();
            foreach (var s in input)
            {
                if (!_spriteTagCache.TryGetValue(s, out var spriteTag))
                {
                    spriteTag = $"<sprite name=\"{s}\">";
                    _spriteTagCache[s] = spriteTag;
                }

                _stringBuilder.Append(spriteTag);
            }

            return _stringBuilder.ToString();
        }

        private static string FromSpriteText(string spriteText)
        {
            var result = new StringBuilder();

            var matches = Regex.Matches(spriteText, "<sprite name=\"(.*?)\">");
            foreach (Match match in matches)
            {
                if (match.Success && match.Groups.Count > 1)
                {
                    var name = match.Groups[1].Value;
                    result.Append(name);
                }
            }

            return result.ToString();
        }

        private static IEnumerable<string> SplitInput(string input)
        {
            return input.Select(c => c.ToString());
        }

        private void OnValidate()
        {
            tmpText.spriteAsset = spriteAsset;
            tmpText.alignment = textAlignment;
            text = m_text;
        }

        private void Reset()
        {
            tmpText = GetComponent<TextMeshProUGUI>();
            tmpText.alignment = textAlignment = TextAlignmentOptions.Midline;
        }
    }
}