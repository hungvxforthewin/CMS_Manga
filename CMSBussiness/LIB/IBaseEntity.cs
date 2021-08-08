using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRMModel.Models.Data;


namespace CRMBussiness.LIB
{
    public interface IBaseEntity<TKey>
    {
        TKey Id { get; set; }
    }
    public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
    {
        protected BaseEntity()
        {
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Required]
        [CrudField(CrudFieldType.Update | CrudFieldType.Delete)]
        public TKey Id { get; set; }
    }
}
