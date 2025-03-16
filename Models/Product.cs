using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace 水水水果API.Models
{
    public class Product
    {
        [Column("id")]
        public Guid Id { get; set; } // Changed to Guid

        [Column("category")]
        public string Category { get; set; }

        [Column("origin")]
        public string Origin { get; set; }

        [Column("recommended_storage_method")]
        public string RecommendedStorageMethod { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("price")]
        public decimal? Price { get; set; }

        [Column("weight")]
        public string Weight { get; set; }

        [Column("image_url")]
        public string ImageUrl { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
