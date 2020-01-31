using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeoServer.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.ErrorTexts), ErrorMessageResourceName = "TheFieldIsNotAValidEmailAddress")]
        [Display(ResourceType = typeof(Resources.AccountTexts), Name = "Email")]
        public string Email { get; set; }
        [Display(ResourceType = typeof(Resources.AdminTexts), Name = "Roles")]
        public IList<string> Roles { get; set; }
    }
}