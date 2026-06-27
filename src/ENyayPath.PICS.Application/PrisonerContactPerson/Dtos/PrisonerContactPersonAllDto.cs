using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.PrisonerContactPerson.Dtos
{
    public class PrisonerContactPersonAllDto
    {
        public Core.Eny.Prisoner.PrisonerContactPerson? Person { get; set; }
        public Core.Eny.Prisoner.PrisonerContactDetail? Detail { get; set; }
        public List<Core.Eny.Prisoner.PrisonerContactPersonDocument>? Documents { get; set; }

    }
}
