using System.Linq;
using System.Security.Claims;

namespace System.Collections.Generic
{

    /// <summary>
    /// 
    /// </summary>
    public static class ClaimsExtensions
    {

        #region Private Members

        private static readonly string[] ClaimTypesForUserId = { "userid" };
        private static readonly string[] ClaimTypesForRoles = { "roles", "role" };
        private static readonly string[] ClaimTypesForEmail = { "emails", "email" };
        private static readonly string[] ClaimTypesForGivenName = { "givenname", "firstname" };
        private static readonly string[] ClaimTypesForFamilyName = { "familyname", "lastname", "surname" };
        private static readonly string[] ClaimTypesForPostalCode = { "postalcode" };
        private static readonly string[] ClaimsToExclude = { "iss", "sub", "aud", "iat", "identities" };

        #endregion

        #region Public Methods

        /// <summary>
        /// Tranlates a set of generic Claims (like the ones returned from Auth0) to a set of Claims from the
        /// <see cref="ClaimTypes"/> constants wherever possible.
        /// </summary>
        /// <param name="claims"></param>
        public static List<Claim> GetStandardizedClaims(this IEnumerable<Claim> claims)
        {
            var newClaims = new List<Claim>();
            foreach (var claim in claims)
            {
                var newClaimType = GetClaimType(claim.Type);
                if (!newClaims.Any(c => c.Type == newClaimType))
                {
                    newClaims.Add(new Claim(newClaimType, claim.Value, claim.ValueType, claim.Issuer));
                }
            }
            return newClaims;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetClaimType(string name)
        {
            var newName = name.Replace("_", "").ToLower();
            if (newName == "name")
            {
                return ClaimTypes.Name;
            }
            if (ClaimTypesForUserId.Contains(newName))
            {
                return ClaimTypes.NameIdentifier;
            }
            if (ClaimTypesForRoles.Contains(newName))
            {
                return ClaimTypes.Role;
            }
            if (ClaimTypesForEmail.Contains(newName))
            {
                return ClaimTypes.Email;
            }
            if (ClaimTypesForGivenName.Contains(newName))
            {
                return ClaimTypes.GivenName;
            }
            if (ClaimTypesForFamilyName.Contains(newName))
            {
                return ClaimTypes.Surname;
            }
            if (ClaimTypesForPostalCode.Contains(newName))
            {
                return ClaimTypes.PostalCode;
            }
            if (name == "gender")
            {
                return ClaimTypes.Gender;
            }
            if (name == "exp")
            {
                return ClaimTypes.Expiration;
            }
            if (name == "actor")
            {
                return ClaimTypes.Actor;
            }

            return name;
        }

        #endregion

    }
}
