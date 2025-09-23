namespace UnifiedContacts.Models.Responses
{
    /// <summary>
    /// Response model for /api/general/connection
    /// </summary>
    public class CheckConnectionResponse
    {
        /// <summary>
        /// Indicates wheather the admin grant was given to the (enterprise) application
        /// </summary>
        public bool IsAdminGranted { get; set; } = false;
        /// <summary>
        /// All permissions that are granted
        /// </summary>
        public List<string> GrantedPermissions { get; set; } = new List<string>();
        /// <summary>
        /// All permissions that are needed for Unified Contacts to work fully but are missing
        /// </summary>
        public List<string> NotGrantedPermissions { get; set; } = new List<string>();
        /// <summary>
        /// Client Id the authentication was made against
        /// </summary>
        public string? ClientId { get; set; }

    }
}
