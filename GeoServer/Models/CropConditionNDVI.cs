using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeoServer.Models
{
    [Table("CropConditionNDVIs", Schema = "public")]
    public class CropConditionNDVI
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.NDVITexts), Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }

        public CATO CATO { get; set; }
        public int? catoid { get; set; }

        [Display(ResourceType = typeof(Resources.CropConditionTypeTexts), Name = "CropConditionType")]
        public CropConditionType CropConditionType { get; set; }
        [Display(ResourceType = typeof(Resources.CropConditionTypeTexts), Name = "CropConditionType")]
        public int? CropConditionTypeId { get; set; }

        public decimal Value { get; set; }
    }
}