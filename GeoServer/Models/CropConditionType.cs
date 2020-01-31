using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeoServer.Models
{
    [Table("CropConditionTypes", Schema = "public")]
    public class CropConditionType
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.CropConditionTypeTexts), Name = "Max")]
        public decimal Max { get; set; }

        [Display(ResourceType = typeof(Resources.CropConditionTypeTexts), Name = "Name")]
        public string Name { get; set; }
    }
}