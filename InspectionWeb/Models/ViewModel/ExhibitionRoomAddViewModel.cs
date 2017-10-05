using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InspectionWeb.Models.ViewModel
{
    public class ExhibitionRoomAddViewModel
    {
        public string RoomId;
        public string RoomCode;
        public string RoomName;
        public string Description;
        public string Floor;
        public string Picture;
        public string FieldId;
        public user Inspector;
        public company Company;
        public string MapFileName;


        public int Active;

        // for svg {{{
        public int? X;
        public int? Y;
        public int? Width;
        public int? Height;
        //}}}

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime? CreateTime;
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime? LastUpdateTime;

        public List<fieldMap> Fields;
        public List<user> Inspectors;
        public List<company> Companys;

        public List<exhibitionItem> ExhibitionItems;
        public List<roomActiveRecord> RoomActiveRecords;

        public string ErrorMsg;

    }
}