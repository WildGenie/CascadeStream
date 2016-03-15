// Decompiled with JetBrains decompiler
// Type: CascadeEquipment.Messages
// Assembly: EquipmentManager, Version=2.0.5674.31272, Culture=neutral, PublicKeyToken=null
// MVID: E33C0263-50E9-4060-BEFA-328D80B2C038
// Assembly location: D:\Загрузки\КаскадПоток\Distr\client\Equipment\EquipmentManager.exe

using CS.Client.Common.Abstract;
using System.Resources;

namespace CascadeEquipment
{
  public class Messages : ILocalizationProvider
  {
    public static ResourceManager Manager { get; set; }

    public static string Pause
    {
      get
      {
        return Messages.Manager.GetString("Pause");
      }
    }

    public static string ConnectionError
    {
      get
      {
        return Messages.Manager.GetString("ConnectionError");
      }
    }

    public static string ConnectionSuccessful
    {
      get
      {
        return Messages.Manager.GetString("ConnectionSuccessful");
      }
    }

    public static string DetectionServer
    {
      get
      {
        return Messages.Manager.GetString("DetectionServer");
      }
    }

    public static string ExtractorServer
    {
      get
      {
        return Messages.Manager.GetString("ExtractorServer");
      }
    }

    public static string VideoServer
    {
      get
      {
        return Messages.Manager.GetString("VideoServer");
      }
    }

    public static string Available
    {
      get
      {
        return Messages.Manager.GetString("Available");
      }
    }

    public static string Unavailable
    {
      get
      {
        return Messages.Manager.GetString("Unavailable");
      }
    }

    public static string Archives
    {
      get
      {
        return Messages.Manager.GetString("Archives");
      }
    }

    public static string All
    {
      get
      {
        return Messages.Manager.GetString("All");
      }
    }

    public static string SetMainPhoto
    {
      get
      {
        return Messages.Manager.GetString("SetMainPhoto");
      }
    }

    public static string Continue
    {
      get
      {
        return Messages.Manager.GetString("Continue");
      }
    }

    public static string SetCategory
    {
      get
      {
        return Messages.Manager.GetString("SetCategory");
      }
    }

    public static string Start
    {
      get
      {
        return Messages.Manager.GetString("Start");
      }
    }

    public static string ErrorImport
    {
      get
      {
        return Messages.Manager.GetString("ErrorImport");
      }
    }

    public static string ImportComplete
    {
      get
      {
        return Messages.Manager.GetString("ImportComplete");
      }
    }

    public static string NoActive
    {
      get
      {
        return Messages.Manager.GetString("NoActive");
      }
    }

    public static string ManagmentServerUnavailble
    {
      get
      {
        return Messages.Manager.GetString("ManagmentServerUnavailble");
      }
    }

    public static string CheckInputParameters
    {
      get
      {
        return Messages.Manager.GetString("CheckInputParameters");
      }
    }

    public static string CheckPassword
    {
      get
      {
        return Messages.Manager.GetString("CheckPassword");
      }
    }

    public static string PassDateAndTime
    {
      get
      {
        return Messages.Manager.GetString("PassDateAndTime");
      }
    }

    public static string Male
    {
      get
      {
        return Messages.Manager.GetString("Male");
      }
    }

    public static string Female
    {
      get
      {
        return Messages.Manager.GetString("Female");
      }
    }

    public static string Process
    {
      get
      {
        return Messages.Manager.GetString("Process");
      }
    }

    public static string RemoveProcess
    {
      get
      {
        return Messages.Manager.GetString("RemoveProcess");
      }
    }

    public static string DoYouWantToSaveChanges
    {
      get
      {
        return Messages.Manager.GetString("DoYouWantToSaveChanges");
      }
    }

    public static string ManagmentServerWork
    {
      get
      {
        return Messages.Manager.GetString("ManagmentServerWork");
      }
    }

    public static string Active
    {
      get
      {
        return Messages.Manager.GetString("Active");
      }
    }

    public static string EnterPeriod
    {
      get
      {
        return Messages.Manager.GetString("EnterPeriod");
      }
    }

    public static string ControlledObject
    {
      get
      {
        return Messages.Manager.GetString("ControlledObject");
      }
    }

    public static string NonCategory
    {
      get
      {
        return Messages.Manager.GetString("NonCategory");
      }
    }

    public static string SelectRecord
    {
      get
      {
        return Messages.Manager.GetString("SelectRecord");
      }
    }

    public static string ChooseUpToFourDevices
    {
      get
      {
        return Messages.Manager.GetString("ChooseUpToFourDevices");
      }
    }

    public static string Archive
    {
      get
      {
        return Messages.Manager.GetString("Archive");
      }
    }

    public static string NewRecordWasCreated
    {
      get
      {
        return Messages.Manager.GetString("NewRecordWasCreated");
      }
    }

    public static string RecordHasBeenChanged
    {
      get
      {
        return Messages.Manager.GetString("RecordHasBeenChanged");
      }
    }

    public static string ListsHasBeenChanged
    {
      get
      {
        return Messages.Manager.GetString("ListsHasBeenChanged");
      }
    }

    public static string IncorrectInputFormat
    {
      get
      {
        return Messages.Manager.GetString("IncorrectInputFormat");
      }
    }

    public static string DoYouWantToExitNow
    {
      get
      {
        return Messages.Manager.GetString("DoYouWantToExitNow");
      }
    }

    public static string DouYouWantToStopProcess
    {
      get
      {
        return Messages.Manager.GetString("DouYouWantToStopProcess");
      }
    }

    public static string DouYouWantToSendArchive
    {
      get
      {
        return Messages.Manager.GetString("DouYouWantToSendArchive");
      }
    }

    public static string DouYouWantToDelete
    {
      get
      {
        return Messages.Manager.GetString("DouYouWantToDelete");
      }
    }

    public static string SelectCategoriesToChange
    {
      get
      {
        return Messages.Manager.GetString("SelectCategoriesToChange");
      }
    }

    public static string PersonInArchive
    {
      get
      {
        return Messages.Manager.GetString("PersonInArchive");
      }
    }

    public static string NoFaceWasFound
    {
      get
      {
        return Messages.Manager.GetString("NoFaceWasFound");
      }
    }

    public static string NoFaceWasFoundDoYouWantToSet
    {
      get
      {
        return Messages.Manager.GetString("NoFaceWasFoundDoYouWantToSet");
      }
    }

    public static string UserLoginField
    {
      get
      {
        return Messages.Manager.GetString("UserLoginField");
      }
    }

    public static string OperationComplete
    {
      get
      {
        return Messages.Manager.GetString("OperationComplete");
      }
    }

    public static string Error
    {
      get
      {
        return Messages.Manager.GetString("Error");
      }
    }

    public static string AuthorizationError
    {
      get
      {
        return Messages.Manager.GetString("AuthorizationError");
      }
    }

    public static string ErrorConnection
    {
      get
      {
        return Messages.Manager.GetString("ErrorConnection");
      }
    }

    public static string Search
    {
      get
      {
        return Messages.Manager.GetString("Search");
      }
    }

    public static string Warning
    {
      get
      {
        return Messages.Manager.GetString("Warning");
      }
    }

    public static string UserNameAlreadyExist
    {
      get
      {
        return Messages.Manager.GetString("UserNameAlreadyExist");
      }
    }

    public static string CheckConnection
    {
      get
      {
        return Messages.Manager.GetString("CheckConnection");
      }
    }

    public static string Angles
    {
      get
      {
        return Messages.Manager.GetString("Angles");
      }
    }

    public static string PersonEdit
    {
      get
      {
        return Messages.Manager.GetString("PersonEdit");
      }
    }

    public static string Reset
    {
      get
      {
        return Messages.Manager.GetString("Reset");
      }
    }

    public static string IdentificationServer
    {
      get
      {
        return Messages.Manager.GetString("IdentificationServer");
      }
    }

    public static string IdentificationServerUnavailble
    {
      get
      {
        return Messages.Manager.GetString("IdentificationServerUnavailble");
      }
    }

    public static string IdentificationServerWork
    {
      get
      {
        return Messages.Manager.GetString("IdentificationServerWork");
      }
    }

    public static string Message
    {
      get
      {
        return Messages.Manager.GetString("Message");
      }
    }

    public static string DeletePerson
    {
      get
      {
        return Messages.Manager.GetString("DeletePerson");
      }
    }

    public static string SetThePrimaryPhoto
    {
      get
      {
        return Messages.Manager.GetString("SetThePrimaryPhoto");
      }
    }

    public static string DeviceIp
    {
      get
      {
        return Messages.Manager.GetString("DeviceIP");
      }
    }

    public static string SystemStatistic
    {
      get
      {
        return Messages.Manager.GetString("SystemStatistic");
      }
    }

    public static string AddToList
    {
      get
      {
        return Messages.Manager.GetString("AddToList");
      }
    }

    public static string Authorization
    {
      get
      {
        return Messages.Manager.GetString("Authorization");
      }
    }

    public static string Authentication
    {
      get
      {
        return Messages.Manager.GetString("Authentication");
      }
    }

    public static string Brightness
    {
      get
      {
        return Messages.Manager.GetString("Brightness");
      }
    }

    public static string Contrast
    {
      get
      {
        return Messages.Manager.GetString("Contrast");
      }
    }

    public static string Size
    {
      get
      {
        return Messages.Manager.GetString("Size");
      }
    }

    public static string Clear
    {
      get
      {
        return Messages.Manager.GetString("Clear");
      }
    }

    public string GetLocalizedString(string key)
    {
      return Messages.Manager.GetString(key);
    }
  }
}
