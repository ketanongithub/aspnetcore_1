using ENyayPath.PICS.Core.Auditing;
using ENyayPath.PICS.Core.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Text;

namespace ENyayPath.PICS.Core.Eny.Common
{
    [Table("EnyAddresses")]
    public class Address : FullAuditedEntity<Int64>, IMayHaveTenant
    {
        public Address()
        {

        }

        public int? TenantId { get; set; }

        [Display(Name = "Street Number")]
        [StringLength(20, ErrorMessage = "Maximum length is 20")]
        public string StreetNumber { get; set; }

        [Display(Name = "Street")]
        [StringLength(100, ErrorMessage = "Maximum length is 100")]
        public string Street { get; set; }

        [Display(Name = "City")]
        [StringLength(50, ErrorMessage = "Maximum length is 50")]
        public string City { get; set; }

        [Display(Name = "State")]
        [StringLength(50, ErrorMessage = "Maximum length is 50")]
        public virtual CountryState State { get; set; }

        [Display(Name = "District")]
        [StringLength(50, ErrorMessage = "Maximum length is 50")]
        public string District { get; set; }

        [Display(Name = "Country")]
        [StringLength(50, ErrorMessage = "Maximum length is 50")]
        public virtual Country Country { get; set; }

        [Display(Name = "PostalCode")]
        [StringLength(10, ErrorMessage = "Maximum length is 10")]
        public string PostalCode { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [Display(Name = "AdrAddress")]
        [StringLength(255, ErrorMessage = "Maximum length is 255")]
        public string AdrAddress { get; set; }

        [Display(Name = "Name")]
        [StringLength(100, ErrorMessage = "Maximum length is 100")]
        public string Name { get; set; }

        [Display(Name = "Place ID")]
        [StringLength(100, ErrorMessage = "Maximum length is 100")]
        public string PlaceID { get; set; }

        /// <summary>
        /// Google Place Details json
        /// </summary>
        [Display(Name = "Place Details")]
        [StringLength(100, ErrorMessage = "Maximum length is 100")]
        public string PlaceDetails { get; set; }

        [Display(Name = "Types")]
        [StringLength(100, ErrorMessage = "Maximum length is 100")]
        public string Types { get; set; }

        [Display(Name = "URL")]
        [StringLength(255, ErrorMessage = "Maximum length is 255")]
        public string Url { get; set; }

        [Display(Name = "UTC Offset")]
        public double UtcOffset { get; set; }

        [Display(Name = "Vicinity")]
        [StringLength(100, ErrorMessage = "Maximum length is 100")]
        public string Vicinity { get; set; }

        public bool IsPrimary { get; set; }
    }
}
