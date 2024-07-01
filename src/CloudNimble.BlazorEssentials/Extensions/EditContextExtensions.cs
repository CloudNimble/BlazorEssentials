

namespace Microsoft.AspNetCore.Components.Forms
{

    /// <summary>
    /// 
    /// </summary>
    public static class EditContextExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="editContext"></param>
        /// <param name="fields">An inline list of fields that were changed in the process, typically specified using "nameof(YourObject.YourProperty").</param>
        public static void NotifyFieldsChanged(this EditContext editContext, params string[] fields)
        {
            if (editContext is null || fields is null) return;

            foreach (var field in fields)
            {
                editContext.NotifyFieldChanged(editContext.Field(field));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="editContext"></param>
        /// <param name="field">The field name that was changed in the process, typically specified using "nameof(YourObject.YourProperty").</param>
        public static void NotifyFieldChanged(this EditContext editContext, string field)
        {
            if (editContext is null || string.IsNullOrWhiteSpace(field)) return;

            editContext.NotifyFieldChanged(editContext.Field(field));
        }

    }

}
