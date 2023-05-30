using System;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using RobeazyCore.Models;
using RobeazyCore.Variables;

namespace RobeazyCore.Services
{
    public class RobeazyNotifications
    {
        public enum NotificationType
        {
            EmailOnCreatedContribution, EmailOnCreatedStory, EmailOnLikedStory, EmailOnLikedContribution
        }

        public static async Task SendEmailNotification(ApplicationUser Recipient, NotificationType Type, Story Story)
        {
            var msg = CreateMessage(Recipient, Type, Story, new Contribution[0]);

            var client = GetClient();

            var response = await client.SendEmailAsync(msg);
        }

        public static async Task SendEmailNotification(ApplicationUser Recipient, NotificationType Type, Story Story, params Contribution[] Contributions)
        {
            var msg = CreateMessage(Recipient, Type, Story, Contributions);

            var client = GetClient();

            var response = await client.SendEmailAsync(msg);
        }

        private static SendGridMessage CreateMessage(ApplicationUser User)
        {
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("noreply@robeazy.com"));
            msg.AddTo(new EmailAddress(User.Email));
            msg.SetSubject("this is another test email.");
            msg.AddContent(MimeType.Text, "this is my test email body");
            return msg;
        }

        private static SendGridMessage CreateMessage(ApplicationUser Recipient, NotificationType Type, Story Story, params Contribution[] Contributions)
        {
            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress("noreply@robeazy.com"));
            msg.AddTo(new EmailAddress(Recipient.Email));
            string content = "";
            string subject = "";
            string queryString;
            switch (Type)
            {
                case NotificationType.EmailOnCreatedContribution:
                    queryString = GetStoryContributionQueryString(Story, Contributions);
                    subject = "A user has responded to your contribution";
                    content = string.Format("<p>A user has replied to your contribution for the story: {0}. <a href=\"{1}\">Click here</a> to go to the story and check it out!</p>",
                        Story.Title,
                        string.Concat(GlobalVar.GetDetailsURLByStory(Story), queryString));
                    break;
                case NotificationType.EmailOnCreatedStory:
                    queryString = GetStoryContributionQueryString(Story, Contributions);
                    subject = "A user has responded to your story";
                    content = string.Format("<p>A user has contributed to your story: {0}. <a href=\"{1}\">Click here</a> to go to the story and check it out!</p>",
                        Story.Title,
                        string.Concat(GlobalVar.GetDetailsURLByStory(Story), queryString));
                    break;
                case NotificationType.EmailOnLikedContribution:
                    queryString = GetStoryContributionQueryString(Story, Contributions);
                    subject = "A user has liked your contribution";
                    content = string.Format("<p>A user has liked your contribution to the story: {0}. <a href=\"{1}\">Click here</a> to view the story again!</p>",
                        Story.Title,
                        string.Concat(GlobalVar.GetDetailsURLByStory(Story), queryString));
                    break;
                case NotificationType.EmailOnLikedStory:
                    queryString = GetStoryContributionQueryString(Story, Contributions);
                    subject = "A user has liked your story";
                    content = string.Format("<p>A user has liked your story: {0}. <a href=\"{1}\">Click here</a> to view the story again!</p>",
                        Story.Title,
                        string.Concat(GlobalVar.GetDetailsURLByStory(Story), queryString));
                    break;
            }
            msg.SetSubject(subject);
            msg.AddContent(MimeType.Html, content);
            return msg;
        }

        private static SendGridClient GetClient()
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");           
            var client = new SendGridClient(apiKey);

            return client;
        }

        private static string GetStoryContributionQueryString(Story Story, params Contribution[] Contributions)
        {
            string QueryString = string.Format("?title={0}", Story.Title);
            foreach (Contribution Contribution in Contributions)
            {
                QueryString = string.Concat(QueryString, string.Format("&contributionIds={0}", Contribution.Id));
            }
            return QueryString;
        }

        public static bool UserRequiresNotification(ApplicationUser User, NotificationType Type)
        {
            //return (bool)User.GetType().GetProperty(NotificationProp).GetValue(User);
            return Type switch
            {
                NotificationType.EmailOnCreatedContribution => User.EmailOnCreatedContribution,
                NotificationType.EmailOnCreatedStory => User.EmailOnCreatedStory,
                NotificationType.EmailOnLikedContribution => User.EmailOnLikedContribution,
                NotificationType.EmailOnLikedStory => User.EmailOnLikedStory,
                _ => false,
            };
        }
    }
}