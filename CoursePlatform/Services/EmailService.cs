using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Helpers;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Models.Courses;
using CoursesPlatform.Models.Razor;
using CoursesPlatform.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace CoursesPlatform.Services
{
    public class EmailService : IEmailService
    {
        private readonly ITemplateHelper templateHelper;
        private readonly UserManager<User> userManager;
        private readonly AppSettings appSettings;

        public EmailService(ITemplateHelper templateHelper,
                            UserManager<User> userManager,
                        IOptions<AppSettings> appSettings)
        {
            this.templateHelper = templateHelper;
            this.userManager = userManager;
            this.appSettings = appSettings.Value;
        }

        public async Task SendCourseStartEmailAsync(string courseTitle, string startIn, string userEmail)
        {
            var message = await templateHelper.GetTemplateHtmlAsStringAsync<CourseStartEmail>(
                "CourseStartEmail",
                new CourseStartEmail
                {
                    CourseTitle = courseTitle,
                    StartIn = startIn
                });

            await SendEmailAsync(userEmail, "Course start", message);
        }

        public async Task SendConfirmationEmailAsync(User user)
        {
            var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

            var message = await templateHelper.GetTemplateHtmlAsStringAsync<ConfirmationEmail>(
                "ConfirmationEmail",
                new ConfirmationEmail
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    Link = StringConstants.EmailConfirmationCallbackUrl + "/" + confirmationToken + "/" + user.Email
                });

            await SendEmailAsync(user.Email, "Confirm your account", message);
        }

        public async Task SendCourseEditingNotificationEmailAsync(CourseDTO newInfo, string oldTitle, string oldDescription, User user)
        {
            var message = await templateHelper.GetTemplateHtmlAsStringAsync<CourseEditingEmail>(
            "CourseEditingEmail",
            new CourseEditingEmail
            {
                OldTitle = oldTitle,
                OldDescription = oldDescription,
                NewCourseInfo = newInfo,
                IsTitleChanged = newInfo.Title != oldTitle,
                IsDescriptionChanged = newInfo.Description != oldDescription,
                User = user
            });

            await SendEmailAsync(user.Email, "Change of course information", message);
        }

        public async Task SendUserInfoChangingNotificationEmailAsync(User newInfo, UserDTO oldInfo)
        {
            var message = await templateHelper.GetTemplateHtmlAsStringAsync<UserInfoChangingEmail>(
            "UserInfoChangingEmail",
            new UserInfoChangingEmail
            {
               OldInfo = oldInfo,
               NewInfo = newInfo
            });

            await SendEmailAsync(oldInfo.Email, "Profile info was changed", message);
        }

        public async Task SendCourseRemovalNotificationEmailAsync(string courseTitle, User user)
        {
            var message = await templateHelper.GetTemplateHtmlAsStringAsync<CourseRemovalEmail>(
            "CourseRemovalEmail",
            new CourseRemovalEmail
            {
                UserName = user.Name,
                UserSurname = user.Surname,
                CourseTitle = courseTitle
            });

            await SendEmailAsync(user.Email, "Subscription cancellation", message);
        }

        public async Task SendAccountRemovalNotificationEmailAsync(UserDTO user)
        {
            var message = await templateHelper.GetTemplateHtmlAsStringAsync<AccountRemovalEmail>(
            "AccountRemovalEmail",
            new AccountRemovalEmail
            {
                User = user
            });

            await SendEmailAsync(user.Email, "Account removal", message);
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SendGridClient(appSettings.SendGridKey);
            var from = new EmailAddress(appSettings.SendGridEmail, appSettings.SendGridSenderName);
            var to = new EmailAddress(email, email);
            var plainTextContent = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, message);

            var result = await client.SendEmailAsync(msg);

            if (!result.IsSuccessStatusCode)
            {
                throw new RestException(System.Net.HttpStatusCode.InternalServerError, new { Message = "Failed to send message!" });
            }
        }
    }
}
