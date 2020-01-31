using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeoServer.Models
{
    [Table("NDVIs", Schema = "public")]
    public class NDVI
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.FieldTexts), Name = "Field")]
        public field field { get; set; }
        [Display(ResourceType = typeof(Resources.FieldTexts), Name = "Field")]
        public int fieldgid { get; set; } // Id, не MapId

        [Display(ResourceType = typeof(Resources.NDVITexts), Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }

        public int Count { get; set; }
        //[DisplayFormat(DataFormatString = "{0:0.###################}", ApplyFormatInEditMode = true)]
        //public decimal Area { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.###################}", ApplyFormatInEditMode = true)]
        public decimal Min { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.###################}", ApplyFormatInEditMode = true)]
        public decimal Max { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.###################}", ApplyFormatInEditMode = true)]
        public decimal Range { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.###################}", ApplyFormatInEditMode = true)]
        public decimal Mean { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.###################}", ApplyFormatInEditMode = true)]
        public decimal STD { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.###################}", ApplyFormatInEditMode = true)]
        public decimal Sum { get; set; }
    }
}