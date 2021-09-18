using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudNimble.BlazorEssentials.Authentication
{

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AuthenticatedEndpointAttribute : Attribute
    {

        internal string ClientNameProperty { get; set; }

        public AuthenticatedEndpointAttribute(string clientNameProperty)
        {
            ClientNameProperty = clientNameProperty;
        }

    }

}
