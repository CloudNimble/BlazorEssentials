using Microsoft.AspNetCore.Components;

namespace CloudNimble.BlazorEssentials
{

    /// <summary>
    /// A port of the ASP.NET MVC HtmlHelper class to Blazor.
    /// </summary>
    /// <remarks>
    /// https://github.com/aspnet/AspNetWebStack/blob/main/src/System.Web.Mvc/HtmlHelper.cs &amp;
    /// https://github.com/dotnet/aspnetcore/tree/main/src/Mvc/Mvc.ViewFeatures/src/Rendering
    /// </remarks>
    public static class Html
    {

        /// <summary>
        /// Outputs HTML to the browser without encoding it.
        /// </summary>
        /// <param name="content"></param>
        /// <returns>A <see cref="MarkupString"/> containing the pre-encoded HTML.</returns>
        public static MarkupString Raw(string content)
        {
            return new MarkupString(content);
        }

    }

}
