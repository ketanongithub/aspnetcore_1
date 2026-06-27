using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ENyayPath.PICS.Application.PrisonerContactPerson.Dtos
{
    public class CreatePrisonerContactPersonDto
    {
        public Guid PrisonerId { get; set; }
        [Required]
        public string? ContactPersonName { get; set; }
        [Required]
        public string? SonDaughterOf { get; set; }
        [Required]
        public string? Relation { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        public string? PhoneNumberPrefix { get; set; }
        public bool IsAudioCall { get; set; }
        public bool IsVideoCall { get; set; }
        public string? AppId { get; set; }
        public string? RegisteredName { get; set; }
        public List<UploadDocumentDto>? Documents { get; set; } = new List<UploadDocumentDto>();   

    }
    public class UploadDocumentDto
    {     
        public Guid DocumentId { get; set; }
        public IFormFile? file { get; set; } 
        public bool? IsValidDocument { get; set; } = false;
    }
}
