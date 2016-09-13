using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    class PurchaseOrder
    {
        public decimal AmountToPay { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string CompanyName { get; set; }
        public int PaymentDayTerms { get; set; }
    }
}
