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
    
    public partial class Messenger_SavedPost
    {
        public int Id { get; set; }
        public int IdPost { get; set; }
        public int IdUser { get; set; }
    
        public virtual Messenger_Post Messenger_Post { get; set; }
        public virtual Messenger_User Messenger_User { get; set; }
    }
}