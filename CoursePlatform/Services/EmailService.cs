using CoursesPlatform.EntityFramework.Models;
using CoursesPlatform.ErrorMiddleware.Errors;
using CoursesPlatform.Interfaces;
using CoursesPlatform.Models.Courses;
using CoursesPlatform.Models.Razor;
using CoursesPlatform.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace CoursesPlatform.Services
{
    public class EmailService : IEmailService
    {
        private readonly ITemplateHelper templateHelper;
        private readonly UserManager<User> userManager;

        public EmailService(ITemplateHelper templateHelper,
                            UserManager<User> userManager)
        {
            this.templateHelper = templateHelper;
            this.userManager = userManager;
        }

        public async Task SendConfirmationEmail(HttpRequest request, User user)
        {
            string confirmationToken = userManager.GenerateEmailConfirmationTokenAsync(user).Result;

            var callbackUrl = $"https://localhost:3000/emailConfirmation";
            var message = await templateHelper.GetTemplateHtmlAsStringAsync<ConfirmationEmail>(
                "ConfirmationEmail",
                new ConfirmationEmail
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    Link = callbackUrl + "/" + confirmationToken + "/" + user.Email
                });

            await SendEmail(user.Email, "Confirm your account", message);
        }

        public async Task SendCourseEditingNotificationEmail(CourseDTO newInfo, string oldTitle, string oldDescription, User user)
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

            await SendEmail(user.Email, "Change of course information", message);
        }

        public async Task SendUserInfoChangingNotificationEmail(User newInfo, UserDTO oldInfo)
        {
            var message = await templateHelper.GetTemplateHtmlAsStringAsync<UserInfoChangingEmail>(
            "UserInfoChangingEmail",
            new UserInfoChangingEmail
            {
               OldInfo = oldInfo,
               NewInfo = newInfo
            });

            await SendEmail(oldInfo.Email, "Profile info was changed", message);
        }

        public async Task SendCourseRemovalNotificationEmail(string courseTitle, User user)
        {
            var message = await templateHelper.GetTemplateHtmlAsStringAsync<CourseRemovalEmail>(
            "CourseRemovalEmail",
            new CourseRemovalEmail
            {
                UserName = user.Name,
                UserSurname = user.Surname,
                CourseTitle = courseTitle
            });

            await SendEmail(user.Email, "Subscription cancellation", message);
        }

        public async Task SendEmail(string email, string subject, string message)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("markkernychny@gmail.com", "no-reply");
            var to = new EmailAddress(email, email);
            var plainTextContent = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, message);

            var result = await client.SendEmailAsync(msg);

            if (!result.IsSuccessStatusCode)
            {
                throw new InternalServerError();
            }
        }

        public async Task SendAccountRemovalNotificationEmail(UserDTO user)
        {
            var message = await templateHelper.GetTemplateHtmlAsStringAsync<AccountRemovalEmail>(
            "AccountRemovalEmail",
            new AccountRemovalEmail
            {
                User = user
            });

            await SendEmail(user.Email, "Account removal", message);
        }
    }
}
