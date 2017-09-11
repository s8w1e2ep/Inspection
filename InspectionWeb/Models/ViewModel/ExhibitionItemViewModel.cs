using System;
using System.ComponentModel.DataAnnotations;

namespace InspectionWeb.Models.ViewModel
{
    public class ExhibitionItemViewModel
    {
        //若需要擴充請自行擴充
        public string itemId { get; set; }
        public string itemName { get; set; }
    }
}