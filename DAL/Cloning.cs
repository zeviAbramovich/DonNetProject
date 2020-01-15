using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class Cloning
    {
        public static Order Clone(this Order original)
        {
            Order target = new Order();
            target.HostingUnitKey = original.HostingUnitKey;
            target.Status = original.Status;
            target.OrderKey = original.OrderKey;
            target.OrderDate = original.OrderDate;
            target.GuestRequestKey = original.GuestRequestKey;
            target.CreateDate = original.CreateDate;
            target.commision = original.commision;
            return target;
        }

        public static HostingUnit Clone(this HostingUnit original)
        {
            HostingUnit target = new HostingUnit();
            if (original.Diary != null)
            {
                target.Diary = (bool[,])original.Diary.Clone();
                target.DiaryDto = original.DiaryDto;
            }
            target.HostingUnitKey = original.HostingUnitKey;
            target.HostingUnitName = original.HostingUnitName;
            target.Adults = original.Adults;
            target.Children = original.Children;
            target.Area = original.Area;
            target.SubArea = original.SubArea;
            target.HostingType = original.HostingType;
            target.Pool = original.Pool;
            target.Jacuzzi = original.Jacuzzi;
            target.Garden = original.Garden;
            target.ChildrensAttractions = original.ChildrensAttractions;
            target.Owner = new Host
            {
                HostId = original.Owner.HostId,
                CollectionClearance = original.Owner.CollectionClearance,
                FamilyName = original.Owner.FamilyName,
                MailAddress = original.Owner.MailAddress,
                PhoneNumber = original.Owner.PhoneNumber,
                PrivateName = original.Owner.PrivateName,
                HostBankAccount = new BankAccount
                {
                    BankAccountNumber = original.Owner.HostBankAccount.BankAccountNumber,
                    BankName = original.Owner.HostBankAccount.BankName,
                    BankNumber = original.Owner.HostBankAccount.BankNumber,
                    BranchAddress = original.Owner.HostBankAccount.BranchAddress,
                    BranchCity = original.Owner.HostBankAccount.BranchCity,
                    BranchNumber = original.Owner.HostBankAccount.BranchNumber
                }
            };
            return target;
        }

        public static GuestRequest Clone(this GuestRequest original)
        {
            GuestRequest target = new GuestRequest();
            target.Adults = original.Adults;
            target.Area = original.Area;
            target.Children = original.Children;
            target.ChildrensAttractions = original.ChildrensAttractions;
            target.EntryDate = original.EntryDate;
            target.FamilyName = original.FamilyName;
            target.Garden = original.Garden;
            target.GuestRequestKey = original.GuestRequestKey;
            target.HostingType = original.HostingType;
            target.Jacuzzi = original.Jacuzzi;
            target.MailAddress = original.MailAddress;
            target.Pool = original.Pool;
            target.PrivateName = original.PrivateName;
            target.RegistrationDate = original.RegistrationDate;
            target.ReleaseDate = original.ReleaseDate;
            target.Status = original.Status;
            target.SubArea = original.SubArea;
            return target;
        }
    }
}
