using RobeazyCore.Models;

namespace RobeazyCore.Variables
{
    public static class GlobalVar
    {
        public const string CreateDateFormat = "M/dd/yyyy h:mm tt";

        public const int TITLE_MAX_LENGTH = 125;
        public const int TITLE_MIN_LENGTH = 2;

        // For Text Stoires
        public const int TEXT_WORD_MIN = 3;
        public const int TEXT_WORD_MAX = 500;
        public const int DEFAULT_RESTRICTION_MIN = 5; // in minutes

        // For Movie Scripts
        public const int MOVIESCRIPT_ELEMENT_MIN = 1;
        public const int MOVIESCRIPT_ELEMENT_MAX = 15;
        public const int DEFAULT_MOVIESCRIPT_RESTRICTION_MIN = 5; // in minutes

        // Music
        public const int MUSIC_BAR_MIN = 1;
        public const int MUSIC_BAR_MAX = 8;

        // Contribution record count
        public const int TEXT_CONTRIBUTION_RECORD_COUNT = 4;
        public const int GUITARTAB_CONTRIBUTION_RECORD_COUNT = 4;
        public const int MOVIESCRIPT_CONTRIBUTION_RECORD_COUNT = 4;

        public const string TEXT_STORY_DETAILS_URL = "robeazy.com/TextStories/Details";
        public const string MOVIESCRIPT_DETAILS_URL = "robeazy.com/MovieScripts/Details";

        public const string GUITARTABS_DETAILS_URL = "robeazy.com/GuitarTabs/Details";

        public static string GetDetailsURLByStory(Story Story)
        {
            switch (Story.StorySubType)
            {
                case StorySubType.TextStory:
                    return TEXT_STORY_DETAILS_URL;
                case StorySubType.GuitarTab:
                    return GUITARTABS_DETAILS_URL;
                default:
                    return TEXT_STORY_DETAILS_URL; // Default to this for now
            }
        }
    }
}