using System.Collections.Generic;
using DataLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class ManagePartners
    {
        public static List<PartnerEntity> ListPartners()
        {
            return PartnerProvider.List();
        }

        public static bool NewPartner(PartnerEntity partner)
        {
            return PartnerProvider.Add(partner);
        }

        public static bool DeletePartner(PartnerEntity partner)
        {
            return PartnerProvider.Remove(partner);
        }

        public static bool ModifyPartner(PartnerEntity partner)
        {
            return PartnerProvider.Modify(partner);
        }
    }
}
