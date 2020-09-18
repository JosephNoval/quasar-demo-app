using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DEMO.Core.DAL.Entities.Common
{
    public class BaseEntity : ICloneable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.Now;
        public RecordStatusType RecordStatus { get; set; } = RecordStatusType.Active;
        public string Remarks { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
        public enum RecordStatusType
        {
            Active = 0,
            Deleted = 1
        }
    }
}
