using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudNimble.BlazorEssentials
{
    public static class Html
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static MarkupString Raw(string content)
        {
            return new MarkupString(content);
        }

    }

}
