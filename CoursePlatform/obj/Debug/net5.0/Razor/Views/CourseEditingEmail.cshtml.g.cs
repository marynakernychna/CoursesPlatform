#pragma checksum "D:\Desktop\CoursesPlatform\CoursePlatform\Views\CourseEditingEmail.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fed2d6c69d2ac000728d092811763b42d51690cf"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_CourseEditingEmail), @"mvc.1.0.view", @"/Views/CourseEditingEmail.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fed2d6c69d2ac000728d092811763b42d51690cf", @"/Views/CourseEditingEmail.cshtml")]
    public class Views_CourseEditingEmail : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<CoursesPlatform.Models.Razor.CourseEditingEmail>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("padding: 20px; background-color: black; color: white"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<!DOCTYPE html>\r\n<html>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fed2d6c69d2ac000728d092811763b42d51690cf3169", async() => {
                WriteLiteral("\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fed2d6c69d2ac000728d092811763b42d51690cf4135", async() => {
                WriteLiteral("\r\n    <div>\r\n        <p style=\"font-size: large\">Hi, <span style=\"font-style: italic; font-weight: 900\">");
#nullable restore
#line 9 "D:\Desktop\CoursesPlatform\CoursePlatform\Views\CourseEditingEmail.cshtml"
                                                                                      Write(Model.User.Name);

#line default
#line hidden
#nullable disable
                WriteLiteral(" ");
#nullable restore
#line 9 "D:\Desktop\CoursesPlatform\CoursePlatform\Views\CourseEditingEmail.cshtml"
                                                                                                       Write(Model.User.Surname);

#line default
#line hidden
#nullable disable
                WriteLiteral("</span> !</p>\r\n\r\n");
#nullable restore
#line 11 "D:\Desktop\CoursesPlatform\CoursePlatform\Views\CourseEditingEmail.cshtml"
         if (Model.IsTitleChanged && Model.IsDescriptionChanged)
        {

#line default
#line hidden
#nullable disable
                WriteLiteral("            <p>\r\n                We inform you that the course is called\r\n                <span style=\"font-style: italic; font-weight: 600\">\r\n                    «");
#nullable restore
#line 16 "D:\Desktop\CoursesPlatform\CoursePlatform\Views\CourseEditingEmail.cshtml"
                Write(Model.OldTitle);

#line default
#line hidden
#nullable disable
                WriteLiteral("» \r\n                </span>\r\n                has been renamed to «");
#nullable restore
#line 18 "D:\Desktop\CoursesPlatform\CoursePlatform\Views\CourseEditingEmail.cshtml"
                                Write(Model.NewCourseInfo.Title);

#line default
#line hidden
#nullable disable
                WriteLiteral("».<br />\r\n                The description has been changed from\r\n                <span style=\"font-style: italic; font-weight: 600\">\r\n                    «");
#nullable restore
#line 21 "D:\Desktop\CoursesPlatform\CoursePlatform\Views\CourseEditingEmail.cshtml"
                Write(Model.OldDescription);

#line default
#line hidden
#nullable disable
                WriteLiteral("» \r\n                </span>\r\n                to \r\n                <span style=\"font-style: italic; font-weight: 600\">\r\n                    «");
#nullable restore
#line 25 "D:\Desktop\CoursesPlatform\CoursePlatform\Views\CourseEditingEmail.cshtml"
                Write(Model.NewCourseInfo.Description);

#line default
#line hidden
#nullable disable
                WriteLiteral("»\r\n                </span>.\r\n            </p>\r\n");
#nullable restore
#line 28 "D:\Desktop\CoursesPlatform\CoursePlatform\Views\CourseEditingEmail.cshtml"
        }
        else
        {
            if (Model.IsTitleChanged)
            {

#line default
#line hidden
#nullable disable
                WriteLiteral("                <p>\r\n                    We inform you that the course is called\r\n                    <span style=\"font-style: italic; font-weight: 600\">\r\n                        «");
#nullable restore
#line 36 "D:\Desktop\CoursesPlatform\CoursePlatform\Views\CourseEditingEmail.cshtml"
                    Write(Model.OldTitle);

#line default
#line hidden
#nullable disable
                WriteLiteral("» \r\n                    </span>\r\n                    has been renamed to \r\n                    <span style=\"font-style: italic; font-weight: 600\">\r\n                        «");
#nullable restore
#line 40 "D:\Desktop\CoursesPlatform\CoursePlatform\Views\CourseEditingEmail.cshtml"
                    Write(Model.NewCourseInfo.Title);

#line default
#line hidden
#nullable disable
                WriteLiteral("».\r\n                    </span>\r\n                </p>\r\n");
#nullable restore
#line 43 "D:\Desktop\CoursesPlatform\CoursePlatform\Views\CourseEditingEmail.cshtml"
            }
            else
            {

#line default
#line hidden
#nullable disable
                WriteLiteral("                <p>\r\n                    We inform you that the description of the course\r\n                    <span style=\"font-style: italic; font-weight: 600\">\r\n                        «");
#nullable restore
#line 49 "D:\Desktop\CoursesPlatform\CoursePlatform\Views\CourseEditingEmail.cshtml"
                    Write(Model.OldTitle);

#line default
#line hidden
#nullable disable
                WriteLiteral("» \r\n                    </span>\r\n                    has been changed from \r\n                    <span style=\"font-style: italic; font-weight: 600\">\r\n                        «");
#nullable restore
#line 53 "D:\Desktop\CoursesPlatform\CoursePlatform\Views\CourseEditingEmail.cshtml"
                    Write(Model.OldDescription);

#line default
#line hidden
#nullable disable
                WriteLiteral("» \r\n                    </span>\r\n                    to \r\n                    <span style=\"font-style: italic; font-weight: 600\">\r\n                        «");
#nullable restore
#line 57 "D:\Desktop\CoursesPlatform\CoursePlatform\Views\CourseEditingEmail.cshtml"
                    Write(Model.NewCourseInfo.Description);

#line default
#line hidden
#nullable disable
                WriteLiteral("»\r\n                    </span>.\r\n                </p>\r\n");
#nullable restore
#line 60 "D:\Desktop\CoursesPlatform\CoursePlatform\Views\CourseEditingEmail.cshtml"
            }
        }

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n        <p>\r\n            <br />\r\n            <br />\r\n            Good day!\r\n        </p>\r\n    </div>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</html>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<CoursesPlatform.Models.Razor.CourseEditingEmail> Html { get; private set; }
    }
}
#pragma warning restore 1591