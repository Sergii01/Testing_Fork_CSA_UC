namespace UnifiedContacts.Server.Models.Dto
{
    public class FilterValidationResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public FilterValidationResponse(bool success, string? message = null)
        {
            Success = success;
            Message = message;
        }
    }
}