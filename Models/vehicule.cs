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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class vehicule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public vehicule()
        {
            this.abonnement = new HashSet<abonnement>();
        }
    
        public int id_vh { get; set; }
        [Required]
        public string nom_vh { get; set; }
        [Required]
        
        public int capacite_vh { get; set; }
        [Required]
        public string marque_vh { get; set; }
        [Required]
        public string modele_vh { get; set; }
        [Required]
        public string immatricul_vh { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<abonnement> abonnement { get; set; }
    }
}