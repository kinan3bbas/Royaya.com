using Royaya.com.Models;
using RoyayaControlPanel.com.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Royaya.com.Extras
{
    public class BasicModel
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

        public int AttachmentId { get; set; }


    }
}