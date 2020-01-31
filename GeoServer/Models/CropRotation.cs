using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeoServer.Models
{
    [Table("CropRotations", Schema = "public")]
    public class CropRotation
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.CropRotationTexts), Name = "CropRotationType")]
        public CropRotationType CropRotationType { get; set; }
        [Display(ResourceType = typeof(Resources.CropRotationTexts), Name = "CropRotationType")]
        public int? CropRotationTypeId { get; set; }

        [Display(ResourceType = typeof(Resources.FieldTexts), Name = "Field")]
        public int fieldgid { get; set; } // Id, не MapId
        [Display(ResourceType = typeof(Resources.FieldTexts), Name = "Field")]
        public field field { get; set; }

        [Display(ResourceType = typeof(Resources.Interface), Name = "Year")]
        public int Year { get; set; }
    }
}