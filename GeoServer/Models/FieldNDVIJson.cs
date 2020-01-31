using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeoServer.Models
{
    [Table("FieldNDVIJsons", Schema = "public")]
    public class FieldNDVIJson
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public CATO CATO { get; set; }
        public int? catoid { get; set; }

        public string Json { get; set; }
    }
}