using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Flight_Manager.Models
{
    public class ReservationsViewModel
    {
        [Display(Name = "Page size")]
        public int PageSize { get; set; }
        public IEnumerable<SelectListItem> PageSizeList { get; set; }
        public IPagedList<ReservationViewModel> Reservations { get; set; }
    }
}
