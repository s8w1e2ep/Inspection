using System.Collections.Generic;

namespace InspectionWeb.Models.ViewModel
{
    public class UserEditViewModel
    {
        public UserDetailViewModel user { get; set; }
        public List<UserDetailViewModel> agents = new List<UserDetailViewModel>();
        public List<GroupDetailViewModel> groups = new List<GroupDetailViewModel>();
    }
}