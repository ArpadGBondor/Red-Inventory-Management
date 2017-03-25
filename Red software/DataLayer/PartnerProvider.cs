using System.Collections.Generic;
using EntityLayer;
using System.Linq.Expressions;
using System;

namespace DataLayer
{
    public class PartnerProvider
    {
        static PartnerProvider()
        {
            Database.InitializeTable<PartnerEntity>();
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
            if (TransactionProvider.IsExistHead(p=>p.Partner_Id == partner.Id))
                return false;
            return Database.Remove<PartnerEntity>(p => p.Id == partner.Id);
        }

        public static List<PartnerEntity> List(Expression<Func<PartnerEntity,bool>> condition)
        {
            var list = Database.ListTable<PartnerEntity>(condition);
            list.Sort();
            return list;
        }

    }
}
