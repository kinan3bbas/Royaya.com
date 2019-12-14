using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Royaya.com.Models
{
    public class Attachment
    {
        [Key]
        public int id { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Last Modification Date")]
        public DateTime LastModificationDate { get; set; }

        public string Creator { get; set; }

        public string Modifier { get; set; }

        public String FileName { get; set; }

        public String Tag { get; set; }

        public String extension { get; set; }


        public String Link { get; set; }
    }
}