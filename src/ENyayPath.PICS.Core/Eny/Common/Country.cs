using ENyayPath.PICS.Core.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ENyayPath.PICS.Core.Eny.Common
{
    [Table("EnyCountries")]
    public class Country : FullAuditedEntity<string>//, IMayHaveTenant
    {
        public Country()
        {

        }

        //public int? TenantId { get; set; }

        [Key]
        [StringLength(2, ErrorMessage = "Maximum length is 2")]
        //public override string Id
        //{
        //    get { return this.Id; }
        //    set { this.Id = Utils.Utils.GenerateSlug(this.Code); }
        //}
        public override string Id { get; set; }

        /// <summary>
        /// Code => Id
        /// </summary>
        [Display(Name = "Code")]
        [StringLength(2, ErrorMessage = "Maximum length is 2")]
        public string Code { get; set; }

        [Display(Name = "Name")]
        [StringLength(50, ErrorMessage = "Maximum length is 50")]
        public string Name { get; set; }

        [Required]
        [StringLength(3, ErrorMessage = "Maximum length is 3")]
        public string CurrencyId { get; set; }
        //[NotMapped]
        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }

        public Int16? PhoneCode { get; set; }

        public string NamePosition { get; set; }

        public string VATLabel { get; set; }
    }
}
