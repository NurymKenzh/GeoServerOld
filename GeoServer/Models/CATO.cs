using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeoServer.Models
{
    [Table("catoes", Schema = "public")]
    public class CATO
    {
        public int Id { get; set; }
        public string AB { get; set; }
        public string CD { get; set; }
        public string EF { get; set; }
        public string HIJ { get; set; }
        [Display(ResourceType = typeof(Resources.Interface), Name = "Name")]
        public string Name { get; set; }

        //public static string GetName(int Id)
        //{
        //    NpgsqlContext db = new NpgsqlContext();
        //    string Name = "";
        //    Name = db.CATOes
        //        .Where(c => c.Id == Id)
        //        .Select(c => c.Name)
        //        .FirstOrDefault();
        //    return Name;
        //}
    }
}