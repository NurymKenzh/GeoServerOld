using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeoServer.Models
{
    [Table("fields", Schema = "public")]
    public class field
    {
        public int gid { get; set; }
        public CATO CATO { get; set; }
        public int? catoid { get; set; }
        public int idfrommap { get; set; }
        //public int adm2 { get; set; }
        public decimal area { get; set; }
        public int croprotation { get; set; }
    }
}