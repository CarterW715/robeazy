using RobeazyCore.Models.Validation;
using RobeazyCore.Variables;
using System.Linq;
using System.Text.RegularExpressions;

namespace RobeazyCore.Models.Text.TextStory
{
    public static class TextStoryValidation
    {
        public static RobeazyValidationResult IsContributionWordCountValid(int? minWords, int? maxWords, string content)
        {
            var result = new RobeazyValidationResult();
            string[] words = Regex.Split(content, @"\s{1,}");
            int wordCount = words.Count();
            minWords = minWords ?? GlobalVar.TEXT_WORD_MIN;
            maxWords = maxWords ?? GlobalVar.TEXT_WORD_MAX;
            if (wordCount < minWords || wordCount > maxWords) {
                result.AddError($"Your contribution was {wordCount} words and must be between {minWords} and {maxWords} words in length");
            }
            return result;
        }
    }
}