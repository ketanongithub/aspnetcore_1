using ENyayPath.PICS.Core.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ENyayPath.PICS.Core.Eny.Common
{
    [Table("EnyCountryStates")]
    public class CountryState : FullAuditedEntity<Int16> //, IMayHaveTenant
    {
        public CountryState()
        {

        }

        //public int? TenantId { get; set; }

        //[Key]
        //[StringLength(50, ErrorMessage = "Maximum length is 50")]
        //public override string Id
        //{
        //    get { return this.Id; }
        //    set { this.Id = Utils.Utils.GenerateSlug(this.Name); }
        //}

        [Display(Name = "Code")]
        [StringLength(3, ErrorMessage = "Maximum length is 3")]
        public string Code { get; set; }

        /// <summary>
        /// Name => Id
        /// </summary>
        [Display(Name = "Name")]
        [StringLength(50, ErrorMessage = "Maximum length is 50")]
        public string Name { get; set; }

        [Required]
        public string CountryId { get; set; }
        //[NotMapped]
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
    }
}
