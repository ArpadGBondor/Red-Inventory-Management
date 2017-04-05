using EntityLayer;
using BusinessLayer;
using Red_Inventory_Management.Views;
using Red_Inventory_Management.Model;
using System;
using Red_Inventory_Management.Notifications;

namespace Red_Inventory_Management.ViewModel
{
    public class PartnersViewModel : TableModel<PartnerEntity>
    {
        public PartnersViewModel()
        {
            ItemName = "partner";
            TableName = "Partners";
        }
        protected override void DeleteItem(object parameter)
        {
            string name = SelectedItem.Name;
            if (ManagePartners.DeletePartner(SelectedItem))
            {
                RefreshList(parameter);
                NotificationProvider.Info("Partner deleted", string.Format("Partner name:{0}", name));
            }
            else
            {
                NotificationProvider.Error("Delete partner error", "This partner is set to one or more transactions.");
            }
        }

        protected override void EditItem(object parameter)
        {
            PartnerEntity Item = new PartnerEntity();
            EntityCloner.CloneProperties<PartnerEntity>(Item, SelectedItem);
            EditPartnerViewModel EPVM = new EditPartnerViewModel(Item, false, ItemName);
            EditItemWindow EIV = new EditItemWindow() { DataContext = EPVM };
            EIV.ShowDialog();
            if (EPVM.SaveEdit)
            {
                Item = EPVM.Item;
                NotificationProvider.Info("Partner saved", string.Format("Partner name:{0}", Item.Name));
                RefreshList(parameter);
                foreach (var p in List)
                    if (Item.Id == p.Id)
                        SelectedItem = p;
            }
        }

        protected override void NewItem(object parameter)
        {
            PartnerEntity Item = new PartnerEntity();
            EditPartnerViewModel EPVM = new EditPartnerViewModel(Item, true, ItemName);
            EditItemWindow EIV = new EditItemWindow() { DataContext = EPVM };
            EIV.ShowDialog();
            if (EPVM.SaveEdit)
            {
                Item = EPVM.Item;
                NotificationProvider.Info("Partner added", string.Format("Partner name:{0}", Item.Name));
                RefreshList(parameter);
                foreach (var p in List)
                    if (Item.Id == p.Id)
                        SelectedItem = p;
            }
        }

        protected override void RefreshList(object parameter)
        {
            List = ManagePartners.ListPartners();
        }
    }
}
