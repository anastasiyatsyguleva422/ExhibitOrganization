//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp1
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string FIO { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<int> ID_Деятели { get; set; }
        public Nullable<int> ID_Посетители { get; set; }
    
        public virtual Деятели Деятели { get; set; }
    }
}
