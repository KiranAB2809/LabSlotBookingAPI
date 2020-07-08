using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure;
using Models;

namespace Services
{
    public class LabServices : ILabServices
    {
        private ILabInfrastructure _lab;

        public LabServices(ILabInfrastructure lab)
        {
            _lab = lab;
        }

        public List<LabList> GetLabList()
        {
            return _lab.GetLabList();
        }

        public bool GetLabSlots(LabModel labModel)
        {
            return _lab.GetLabSlots(labModel);
        }

        public string InsertLabSLot(LabModel labModel)
        {
            return _lab.InsertLabSLot(labModel);
        }

        public string ApproveLabSlot(int bookingid, bool approved)
        {
            return _lab.ApproveLabSlot(bookingid, approved);
        }

        public List<ApprovalList> ApprovalSlots()
        {
            return _lab.ApprovalSlots();
        }

        public void DeleteLabSlot()
        {

        }

        public void WeeklyReportJob()
        {

        }

    }
}
