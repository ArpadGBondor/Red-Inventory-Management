using BusinessLayer;
using EntityLayer;
using Red_Inventory_Management.Model;
using Red_Inventory_Management.Notifications;
using Red_Inventory_Management.Views;

namespace Red_Inventory_Management.ViewModel
{
    public class PartnersViewModel : TableModel<PartnerEntity>
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PartnersViewModel()
        {
            ItemName = "partner";
            TableName = "Partners";
        }
        protected override void DeleteItem(object parameter)
        {
            log.Debug("Delete " + ItemName + " button");

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
            log.Debug("Edit " + ItemName + " button");

            PartnerEntity Item = new PartnerEntity();
            EntityCloner.CloneProperties<PartnerEntity>(SelectedItem, Item);
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
            log.Debug("New " + ItemName + " button");

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
            log.Debug("Refresh " + ItemName + " list");

            List = ManagePartners.ListPartners();
        }
    }
}
