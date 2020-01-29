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
            Order target = new Order
            {
                HostingUnitKey = original.HostingUnitKey,
                Status = original.Status,
                OrderKey = original.OrderKey,
                OrderDate = original.OrderDate,
                GuestRequestKey = original.GuestRequestKey,
                CreateDate = original.CreateDate,
                Commision = original.Commision
            };
            return target;
        }

        public static HostingUnit Clone(this HostingUnit original)
        {
            HostingUnit target = new HostingUnit();
            if (original.Diary == null)
            {
                target.Diary = new bool[12, 31];
                //target.DiaryDto = original.DiaryDto;
            }
            else
                target.Diary = (bool[,])original.Diary.Clone();
            target.HostingUnitKey = original.HostingUnitKey;
            target.bookDates = original.bookDates.ToList();
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
            target.SumComission = original.SumComission;
            target.Owner = new Host
            {
                HostId = original.Owner.HostId,
                Password=original.Owner.Password,
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
            GuestRequest target = new GuestRequest
            {
                Adults = original.Adults,
                Area = original.Area,
                Children = original.Children,
                ChildrensAttractions = original.ChildrensAttractions,
                EntryDate = original.EntryDate,
                FamilyName = original.FamilyName,
                Garden = original.Garden,
                GuestRequestKey = original.GuestRequestKey,
                HostingType = original.HostingType,
                Jacuzzi = original.Jacuzzi,
                MailAddress = original.MailAddress,
                Pool = original.Pool,
                PrivateName = original.PrivateName,
                RegistrationDate = original.RegistrationDate,
                ReleaseDate = original.ReleaseDate,
                Status = original.Status,
                SubArea = original.SubArea
            };
            return target;
        }
    }
}
