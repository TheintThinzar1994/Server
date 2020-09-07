using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IThankCardsService
    {
        List<object> updateThankCardView(int id, string status);
    }
    public class ThankCardsService : IThankCardsService
    {
        private ApplicationContext _context;

        public ThankCardsService(ApplicationContext context)
        {
            _context = context;
        }
        public List<object> updateThankCardView(int id,string status)
        {
            List<ThankCard> thankcard = new List<ThankCard>();
            var data1 = from s in _context.ThankCards
                        where s.Id == id && s.isActive==true
                        select s;
            thankcard = data1.ToList<ThankCard>();
            if (thankcard.Count() > 0)
            {
                ThankCard objthank = new ThankCard();
                objthank = thankcard[0];
                objthank.Status = status;
                objthank.ts = DateTime.Now;
                objthank.isActive = true;
                _context.ThankCards.Update(objthank);
                _context.SaveChanges();
            }
            data1 = from s in _context.ThankCards
                    where s.Id == id && s.isActive == true
                    select s;
           List<object> ret_thankcard = data1.ToList<object>();
            return ret_thankcard;
        }
    }
}
