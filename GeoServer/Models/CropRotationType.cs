using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeoServer.Models
{
    [Table("CropRotationTypes", Schema = "public")]
    public class CropRotationType
    {
        public int Id { get; set; }
        [Display(ResourceType = typeof(Resources.CropRotationTexts), Name = "NameRU")]
        public string NameRU { get; set; }
        [Display(ResourceType = typeof(Resources.CropRotationTexts), Name = "NameKK")]
        public string NameKK { get; set; }
        [Display(ResourceType = typeof(Resources.CropRotationTexts), Name = "NameEN")]
        public string NameEN { get; set; }
    }
}