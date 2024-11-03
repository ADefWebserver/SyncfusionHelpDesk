#nullable disable
using System.ComponentModel.DataAnnotations;

namespace SyncfusionHelpDesk.Models
{
    public class HelpDeskTicketDTO
    {
        public int Id { get; set; }

        [Required]
        public string TicketStatus { get; set; }

        [Required]
        public DateTime TicketDate { get; set; }

        [Required(ErrorMessage = "The Ticket Description field is required.")]
        [StringLength(50, MinimumLength = 2,
        ErrorMessage = "Description must be a minimum of 2 and maximum of 50 characters.")]
        public string TicketDescription { get; set; }

        [Required(ErrorMessage = "The Ticket Requester Email field is required.")]
        [EmailAddress(ErrorMessage = "Must use a valid email address.")]
        public string TicketRequesterEmail { get; set; }

        public string TicketGuid { get; set; }
    }
}