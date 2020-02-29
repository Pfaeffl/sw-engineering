using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LagerverwaltungLib;

namespace LagerverwaltungWebApp.Models
{
    public class Order
    {
        [Key]
        [Column(Order = 0)]
        public int UserId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int MaterialId { get; set; }
        public User User { get; set; }
        public Material Material { get; set; }
    }

    public class GoodsReceipt
    {
        [Key]
        [Column(Order = 0)]

        public int Wareneingang { get; set; }

        [Key]
        [Column(Order = 1)]
        public int MaterialId { get; set; }

        public Material Mat { get; set; }

    }

    public class GoodsIssue
    {
        [Key]
        [Column(Order = 0)]
        public int Warenausgang { get; set; }

        [Key]
        [Column(Order = 1)]
        public int MaterialId { get; set; }

        public Material Mat { get; set; }

    }

}