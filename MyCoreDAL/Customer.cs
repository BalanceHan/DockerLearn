using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyCoreDAL
{
    [ElasticsearchType(IdProperty= "CustomerGuid")]
    public class Customer : FartherEntity
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        [StringLength(50)]
        public string CustomerGuid { get; set; }
        
        [StringLength(50)]
        public string CustomerName { get; set; }
        
        [StringLength(50)]
        public string CustomerPhone { get; set; }

        public virtual ICollection<CustomerAddress> CustomerAddress { get; set; }
    }
}
