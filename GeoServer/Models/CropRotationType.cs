﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Display(ResourceType = typeof(Resources.CropRotationTexts), Name = "Name")]
        public string Name
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
                if (cookie != null)
                {
                    if (cookie.Value != null)
                    {
                        if (cookie.Value == "en")
                        {
                            return NameEN;
                        }
                        if (cookie.Value == "ru")
                        {
                            return NameRU;
                        }
                    }
                }
                else
                {
                    return NameEN;
                }
                return NameEN;
            }
        }
    }
}