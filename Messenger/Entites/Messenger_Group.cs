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
    
    public partial class Messenger_Group
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Messenger_Group()
        {
            this.Messenger_Post = new HashSet<Messenger_Post>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int IdOwner { get; set; }
    
        public virtual Messenger_User Messenger_User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Messenger_Post> Messenger_Post { get; set; }
    }
}
