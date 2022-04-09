using Flight_Manager.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using X.PagedList;

namespace Flight_Manager.Models
{
    public class UsersViewModel
    {
        [Display(Name = "Page size")]
        public int PageSize { get; set; }
        public IEnumerable<SelectListItem> PageSizeList { get; set; }
        public IPagedList<ApplicationUser> Users { get; set; }

    }
}
