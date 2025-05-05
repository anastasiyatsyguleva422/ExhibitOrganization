using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp1
{
    [MetadataType(typeof(Место_проведенияMetadata))]
    public partial class Место_проведения { }

    public class Место_проведенияMetadata
    {
        [Key]
        public int ID_Место_проведения { get; set; }
    }

    [MetadataType(typeof(ОтзывыMetadata))]
    public partial class Отзывы { }

    public class ОтзывыMetadata
    {
        [Key]
        public int ID_Отзывы { get; set; }
    }

    [MetadataType(typeof(ПосетителиMetadata))]
    public partial class Посетители { }

    public class ПосетителиMetadata
    {
        [Key]
        public int ID_Посетители { get; set; }
    }

    [MetadataType(typeof(Посетители_ОтзывыMetadata))]
    public partial class Посетители_Отзывы { }

    public class Посетители_ОтзывыMetadata
    {
        [Key, Column(Order = 0)]
        public int ID_Посетители { get; set; }

        [Key, Column(Order = 1)]
        public int ID_Отзывы { get; set; }
    }

    [MetadataType(typeof(ПродажиMetadata))]
    public partial class Продажи { }

    public class ПродажиMetadata
    {
        [Key]
        public int ID_Продажи { get; set; }
    }

    [MetadataType(typeof(ЭкспонатMetadata))]
    public partial class Экспонат { }

    public class ЭкспонатMetadata
    {
        [Key]
        public int ID_Экспонат { get; set; }
    }

    [MetadataType(typeof(UserMetadata))]
    public partial class User { }

    public class UserMetadata
    {
        [Key]
        public int ID { get; set; }
    }
    [MetadataType(typeof(ПриглашенияMetadata))]
    public partial class Приглашения { }

    public class ПриглашенияMetadata
    {
        [Key]
        public int ID_Приглашения { get; set; }

        public int ID_Выставки { get; set; }
        public int ID_Деятели { get; set; }
        public string Статус { get; set; }
        public DateTime? Дата_приглашения { get; set; }
    }

}
