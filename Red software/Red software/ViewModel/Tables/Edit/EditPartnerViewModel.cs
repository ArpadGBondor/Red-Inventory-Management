using System;
using EntityLayer;
using Red_software.Model;
using BusinessLayer;

namespace Red_software.ViewModel
{
    public class EditPartnerViewModel : EditItemModel<PartnerEntity>
    {
        public EditPartnerViewModel(PartnerEntity _Item, bool _NewRecord, string _ItemName) : base(_Item, _NewRecord, _ItemName) { }

        protected override bool Save(object parameter)
        {
            bool lSuccess = false;
            if (NewRecord)
            {
                lSuccess = ManagePartners.NewPartner(Item);
            }
            else
            {
                lSuccess = ManagePartners.ModifyPartner(Item);
            }
            return lSuccess;
        }
    }
}
