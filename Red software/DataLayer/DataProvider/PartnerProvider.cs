using System.Collections.Generic;
using EntityLayer;
using System.Linq.Expressions;
using System;

namespace DataLayer
{
    public class PartnerProvider
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Adds a new partner to the database.
        /// </summary>
        /// <param name="partner">The Id property is automatically generated.</param>
        /// <returns></returns>
        public static bool Add(PartnerEntity partner)
        {
            if (partner == null)
                return false;
            return DatabaseConnection.Add<PartnerEntity>(partner);
        }

        /// <summary>
        /// Modifies a partner in the database.
        /// </summary>
        /// <param name="partner">The Id property cannot be modified.</param>
        /// <returns></returns>
        public static bool Modify(PartnerEntity partner)
        {
            if (partner == null)
                return false;
            return DatabaseConnection.Modify<PartnerEntity>(partner, p => p.Id == partner.Id);
        }

        /// <summary>
        /// Removes a partner from the database with the same Id.
        /// </summary>
        /// <param name="partner">The Id property selects the partner.</param>
        /// <returns></returns>
        public static bool Remove(PartnerEntity partner)
        {
            if ((partner == null)
                || (TransactionProvider.IsExistHead(p=>p.PartnerId == partner.Id)))
                return false;
            return DatabaseConnection.Remove<PartnerEntity>(p => p.Id == partner.Id);
        }

        /// <summary>
        /// Lists every record from the Partners table where the condition returns true
        /// </summary>
        /// <param name="condition">Condition on the records of the table. Eg. (p => p.Id == record.Id)</param>
        /// <returns></returns>
        public static List<PartnerEntity> List(Expression<Func<PartnerEntity,bool>> condition)
        {
            var list = DatabaseConnection.ListTable<PartnerEntity>(condition);
            list.Sort();
            return list;
        }

    }
}
