//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Messenger.Entites
{
    using System;
    using System.Collections.Generic;
    
    public partial class Messenger_Band
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Messenger_Band()
        {
            this.Messenger_User = new HashSet<Messenger_User>();
        }
    
        public int Id { get; set; }
        public int IdSpecialization { get; set; }
        public string Band { get; set; }
    
        public virtual Messenger_Specialization Messenger_Specialization { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Messenger_User> Messenger_User { get; set; }
    }
}
