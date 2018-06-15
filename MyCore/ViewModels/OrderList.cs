using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyCore.ViewModels
{
    public class OrderList
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string Server_ID { get; set; }

        [StringLength(50)]
        public string Customer_Name { get; set; }

        [StringLength(50)]
        public string Customer_Phone { get; set; }

        [StringLength(50)]
        public string Longitude { get; set; }

        [StringLength(50)]
        public string Latitude { get; set; }

        [StringLength(500)]
        public string Rescue_Address { get; set; }

        public long? Order_State { get; set; }

        [StringLength(50)]
        public string Waiter_Phone { get; set; }

        public DateTime? Addition_Date { get; set; }

        [StringLength(50)]
        public string Addition_Unix { get; set; }

        [StringLength(1000)]
        public string Remark { get; set; }

        public double? Damage_Cost { get; set; }

        public DateTime? Finish_Date { get; set; }

        [StringLength(50)]
        public string Finish_Unix { get; set; }

        public string[] Server_Img { get; set; }

        public string[] Server_Card { get; set; }
    }
}
