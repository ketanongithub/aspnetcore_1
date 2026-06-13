using ENyayPath.PICS.Core.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ENyayPath.PICS.Core.Eny.Common
{
    [Table("EnyCurrencies")]
    public class Currency : FullAuditedEntity<string> //, IMayHaveTenant
    {
        public Currency()
        {

        }

        //public int? TenantId { get; set; }

        [Key]
        [StringLength(3, ErrorMessage = "Maximum length is 3")]
        public override string Id { get; set; }
        //public override string Id
        //{
        //    get { return this.Id; }
        //    set { this.Id = this.Code; }
        //}

        [Display(Name = "Code")]
        [StringLength(3, ErrorMessage = "Maximum length is 3")]
        //public string Code
        //{
        //    get { return this.Code; }
        //    set
        //    {
        //        string code = Utils.Utils.GenerateSlug(this.Code);
        //        this.Code = code.ToUpper();
        //    }
        //}
        public string Code { get; set; }

        [Display(Name = "Name")]
        [StringLength(50, ErrorMessage = "Maximum length is 50")]
        public string Name { get; set; }

        public string Symbol { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DefaultValue(2)]
        public decimal Rounding { get; set; }

        [DefaultValue(2)]
        public Int16 DecimalPlaces { get; set; }

        public bool IsPrimary { get; set; }
    }
}
