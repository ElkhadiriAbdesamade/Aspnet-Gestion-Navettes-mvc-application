//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gestion_Navettes.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Gestion_NavettesEntities : DbContext
    {
        public Gestion_NavettesEntities()
            : base("name=Gestion_NavettesEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<abonnement> abonnement { get; set; }
        public virtual DbSet<demande_abn> demande_abn { get; set; }
        public virtual DbSet<demande_user> demande_user { get; set; }
        public virtual DbSet<reservation> reservation { get; set; }
        public virtual DbSet<societe> societe { get; set; }
        public virtual DbSet<users> users { get; set; }
        public virtual DbSet<vehicule> vehicule { get; set; }
    }
}
