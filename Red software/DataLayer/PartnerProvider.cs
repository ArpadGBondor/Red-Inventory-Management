using System.Collections.Generic;
using EntityLayer;

namespace DataLayer
{
    public class PartnerProvider
    {
        static PartnerProvider()
        {
            Database.InitializeTable(typeof(PartnerEntity));
        }

        public static bool Add(PartnerEntity partner)
        {
            return Database.Add<PartnerEntity>(partner);
        }

        public static bool Modify(PartnerEntity partner)
        {
            return Database.Modify<PartnerEntity>(partner, p => p.Id == partner.Id);
        }

        public static bool Remove(PartnerEntity partner)
        {
            return Database.Remove<PartnerEntity>(p => p.Id == partner.Id);
        }

        public static List<PartnerEntity> List()
        {
            return Database.ListTable<PartnerEntity>(p => true);
        }

    }
}
