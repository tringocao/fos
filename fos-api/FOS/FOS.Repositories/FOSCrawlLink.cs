//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FOS.Repositories
{
    using System;
    using System.Collections.Generic;
    
    public partial class FOSCrawlLink
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FOSCrawlLink()
        {
            this.FOSBodyFieldLinks = new HashSet<FOSBodyFieldLink>();
            this.FOSHeaderLinks = new HashSet<FOSHeaderLink>();
        }
    
        public int ID { get; set; }
        public string Link { get; set; }
        public int HostID { get; set; }
        public string Description { get; set; }
        public string Request_Method { get; set; }
    
        public virtual FOSHostLink FOSHostLink { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FOSBodyFieldLink> FOSBodyFieldLinks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FOSHeaderLink> FOSHeaderLinks { get; set; }
    }
}
