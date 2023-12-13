namespace weblog_API.Enums;

public static class AddressType
{
   public static string GetHouseType(int? index)
   {
      switch (index)
      {
         case 1:
            return "Владение";
         case 2:
            return "Дом";
         case 3:
            return "Домовладение";
         case 4:
            return "Гараж";
         case 5:
            return "Здание";
         case 6:
            return "Шахта";
         case 7:
            return "Строение";
         case 8:
            return "Сооружение";
         case 9:
            return "Литера";
         case 10:
            return "Корпус";
         case 11:
            return "Подвал";
         case 12:
            return "Котельная";
         case 13:
            return "Погреб";
         case 14:
            return "Объект незавершенного строительства (ОНС)";
         default:
            return "Здание (сооружение)";
      }
   }

   public static string GetAddressType(string level)
   {
      switch (level)
      {
         case "1":
            return "Субъект РФ";
         case "2":
            return "Административный район";
         case "3":
            return "Муниципальный район";
         case "4":
            return "Сельское/городское поселение";
         case "5":
            return "Город";
         case "6":
            return "Населенный пункт";
         case "7":
            return "Элемент планировочной структуры";
         case "8":
            return "Элемент улично-дорожной сети";
         case "9":
            return "Земельный участок";
         case "10":
            return "Здание (сооружение)";
         case "11":
            return "Помещение";
         case "12":
            return "Помещения в пределах помещения";
         case "13":
            return "Уровень автономного округа (устаревшее)";
         case "14":
            return "Уровень внутригородской территории (устаревшее)";
         case "15":
            return "Уровень дополнительных территорий (устаревшее)";
         case "16":
            return "Уровень объектов на дополнительных территориях (устаревшее)";
         case "17":
            return "Машиноместо";
         default:
            return "Здание (сооружение)";
      }
   }
}