//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kursovoii
{
    using System;
    using System.Collections.Generic;
    
    public partial class Пользователи
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Пользователи()
        {
            this.История_входа = new HashSet<История_входа>();
            this.Рейс = new HashSet<Рейс>();
        }
    
        public int Код_пользователя { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }
        public Nullable<System.DateTime> Дата_рождения { get; set; }
        public string Номер_телефона { get; set; }
        public string Электронная_почта { get; set; }
        public Nullable<int> Код_статуса_пользователя { get; set; }
        public string Логин { get; set; }
        public string Пароль { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<История_входа> История_входа { get; set; }
        public virtual Статус_пользователя Статус_пользователя { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Рейс> Рейс { get; set; }
    }
}
