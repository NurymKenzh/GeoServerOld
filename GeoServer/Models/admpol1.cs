using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeoServer.Models
{
    [Table("admpol1", Schema = "public")]
    public class admpol1
    {
        public int gid { get; set; }
        public CATO CATO { get; set; }
        public int? catoid { get; set; }
        //public int code_obl { get; set; }
    }
}