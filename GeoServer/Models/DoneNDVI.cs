using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeoServer.Models
{
    [Table("DoneNDVIs", Schema = "public")]
    public class DoneNDVI
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.NDVITexts), Name = "Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }

        public CATO CATO { get; set; }
        public int? catoid { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.###################}", ApplyFormatInEditMode = true)]
        public decimal MeanAverage { get; set; }
    }
}