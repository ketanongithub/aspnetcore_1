using System;
using System.Collections.Generic;

namespace ENyayPath.PICS.Application.PrisonerContactPerson.Dtos
{
    public class CreatePrisonerContactDto
    {
        public Guid PrisonerId { get; set; }

        public string ContactPersonName { get; set; } = null!;

        public string Relation { get; set; } = null!;

        public List<string> PhoneNumbers { get; set; } = new();
    }
}