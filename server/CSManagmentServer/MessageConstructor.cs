// Decompiled with JetBrains decompiler
// Type: CSManagmentServer.MessageConstructor
// Assembly: CSManagmentServer, Version=2.0.5674.31275, Culture=neutral, PublicKeyToken=null
// MVID: C5B7D3C1-7999-4FC6-B40F-178E2CEECAE4
// Assembly location: D:\Загрузки\КаскадПоток\Distr\server\x86\manager\CSManagmentServer.exe

using BasicComponents;
using System;

namespace CSManagmentServer
{
  public class MessageConstructor
  {
    public static string GetMessageString(string name, Guid id, bool state, ObjectType objType, ObjEventType eventtype)
    {
      switch (objType)
      {
        case ObjectType.Device:
          if (eventtype == ObjEventType.Add)
            return string.Concat(new object[4]
            {
              (object) "Добавлена новая Камера:",
              (object) name,
              (object) ", ID ",
              (object) id
            });
          if (eventtype == ObjEventType.Delete)
            return string.Concat(new object[4]
            {
              (object) "Камера была удалена:",
              (object) name,
              (object) ", ID ",
              (object) id
            });
          if (eventtype == ObjEventType.StateChanged)
          {
            string str = "Работает";
            if (!state)
              str = "Не работает";
            return "Состояние камеры было изменено на " + (object) str + " :" + name + ", ID " + (string) (object) id;
          }
          break;
        case ObjectType.VideoServer:
          if (eventtype == ObjEventType.Add)
            return string.Concat(new object[4]
            {
              (object) "Добавлен новый видео сервис:",
              (object) name,
              (object) ", ID ",
              (object) id
            });
          if (eventtype == ObjEventType.Delete)
            return string.Concat(new object[4]
            {
              (object) "Видео сервис был удален:",
              (object) name,
              (object) ", ID ",
              (object) id
            });
          if (eventtype == ObjEventType.Update)
            return string.Concat(new object[4]
            {
              (object) "Видео сервис был изменен:",
              (object) name,
              (object) ", ID ",
              (object) id
            });
          if (eventtype == ObjEventType.StateChanged)
          {
            string str = "Работает";
            if (!state)
              str = "Не работает";
            return "Состояние видео сервиса было изменено на " + (object) str + " :" + name + ", ID " + (string) (object) id;
          }
          break;
        case ObjectType.DetectorServer:
          if (eventtype == ObjEventType.Add)
            return string.Concat(new object[4]
            {
              (object) "Добавлен новый сервис детектирования:",
              (object) name,
              (object) ", ID ",
              (object) id
            });
          if (eventtype == ObjEventType.Update)
            return string.Concat(new object[4]
            {
              (object) "Сервис детектирования был удален:",
              (object) name,
              (object) ", ID ",
              (object) id
            });
          if (eventtype == ObjEventType.Delete)
            return string.Concat(new object[4]
            {
              (object) "Сервис детектирования был изменен:",
              (object) name,
              (object) ", ID ",
              (object) id
            });
          if (eventtype == ObjEventType.StateChanged)
          {
            string str = "Работает";
            if (!state)
              str = "Не работает";
            return "Состояние сервиса детектирования было изменено на " + (object) str + " :" + name + ", ID " + (string) (object) id;
          }
          break;
        case ObjectType.IdentificationServer:
          if (eventtype == ObjEventType.Add)
            return string.Concat(new object[4]
            {
              (object) "Добавлен новый сервис идентификации:",
              (object) name,
              (object) ", ID ",
              (object) id
            });
          if (eventtype == ObjEventType.Delete)
            return string.Concat(new object[4]
            {
              (object) "Сервис идентификации был удален:",
              (object) name,
              (object) ", ID ",
              (object) id
            });
          if (eventtype == ObjEventType.Update)
            return string.Concat(new object[4]
            {
              (object) "Сервис идентификации был изменен:",
              (object) name,
              (object) ", ID ",
              (object) id
            });
          if (eventtype == ObjEventType.StateChanged)
          {
            string str = "Работает";
            if (!state)
              str = "Не работает";
            return "Состояние сервиса идентификации было изменено на " + (object) str + " :" + name + ", ID " + (string) (object) id;
          }
          break;
        case ObjectType.ManagmentServer:
          if (eventtype == ObjEventType.Add)
            return string.Concat(new object[4]
            {
              (object) "Добавлен новый сервиса управления:",
              (object) name,
              (object) ", ID ",
              (object) id
            });
          if (eventtype == ObjEventType.Delete)
            return string.Concat(new object[4]
            {
              (object) "Сервис управления был удален:",
              (object) name,
              (object) ", ID ",
              (object) id
            });
          if (eventtype == ObjEventType.StateChanged)
          {
            string str = "Работает";
            if (!state)
              str = "Не работает";
            return "Состояние сервиса управления было изменено на " + (object) str + " :" + name + ", ID " + (string) (object) id;
          }
          break;
        case ObjectType.ExtractorServer:
          if (eventtype == ObjEventType.Add)
            return string.Concat(new object[4]
            {
              (object) "Добавлен новый сервис построения шаблонов:",
              (object) name,
              (object) ", ID ",
              (object) id
            });
          if (eventtype == ObjEventType.Update)
            return string.Concat(new object[4]
            {
              (object) "Сервис построения шаблонов был изменен:",
              (object) name,
              (object) ", ID ",
              (object) id
            });
          if (eventtype == ObjEventType.Delete)
            return string.Concat(new object[4]
            {
              (object) "Сервис построения шаблонов был удален:",
              (object) name,
              (object) ", ID ",
              (object) id
            });
          if (eventtype == ObjEventType.StateChanged)
          {
            string str = "Работает";
            if (!state)
              str = "Не работает";
            return "Состояние сервиса построения шаблонов было изменено на " + (object) str + " :" + name + ", ID " + (string) (object) id;
          }
          break;
        default:
          return "";
      }
      return "";
    }
  }
}
