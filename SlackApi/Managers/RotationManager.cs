using System;
using System.Linq;
using System.Text;

namespace SlackApi.Managers
{
    public class RotationManager
    {
        public string GetRotation(string text)
        {
            var tokens = text.Trim().Split(' ').Distinct();
            int count;
            var list = int.TryParse(tokens.FirstOrDefault(), out count)
                ? tokens.Skip(1).ToList()
                : tokens.ToList();
            if (count == 0) count = 1;
            var result = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                var randomIndex = random.Next(list.Count);
                var randomItem = list[randomIndex];
                result.Append(randomItem).Append(' ');
                list.RemoveAt(randomIndex);
            }
            return result.ToString();
        }
    }
}
