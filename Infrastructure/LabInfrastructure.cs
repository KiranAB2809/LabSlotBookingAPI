using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Volvo.NAMS.LoggingDomain.Model.DomainLayer;
using System.Net.Mail;

namespace Infrastructure
{
    public class LabInfrastructure : ILabInfrastructure
    {
        private readonly ISettings _settings;
        private MongoClient _client;
        private MongoServer _server;
        private MongoDatabase _db;

        public LabInfrastructure(ISettings settings)
        {
            this._settings = settings;

            //Establishing connection to Mongo
            _client = new MongoClient(_settings.GetMongoDB());
            _server = _client.GetServer();
            _db = _server.GetDatabase(_settings.GetDatabaseName());
        }

        public List<LabList> GetLabList()
        {
            try
            {
                List<LabList> list = new List<LabList>();
                list = _db.GetCollection<LabList>("LabList").FindAll().ToList();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public bool GetLabSlots(LabModel labModel)
        {
            bool available = true;
            try
            {
                var mongoCollection = _db.GetCollection<LabModel>("LabSlots").AsQueryable<LabModel>()
                    .Where(e => e.LabId == labModel.LabId
                    //&& (e.StartTime != labModel.StartTime || e.EndTime != labModel.EndTime)   
                    //&& e.isApproved == false
                    //&& e.Approved == false
                    ).ToList();
                if(mongoCollection.Count == 0)
                {                    
                    return available;
                }
                available = CheckforSpeicificTime(mongoCollection, labModel.StartTime, labModel.EndTime);
            }
            catch (Exception)
            {

                throw;
            }
            return available;
        }

        public List<ApprovalList> ApprovalSlots()
        {
            try
            {
                List<LabModel> list = new List<LabModel>();
                List<ApprovalList> approvalLists = new List<ApprovalList>();
                list = _db.GetCollection<LabModel>("LabSlots").AsQueryable().Where(
                    e => e.Approved == false 
                    && e.isApproved == false
                    && e.StartTime >= DateTime.Now
                    ).ToList();
                foreach(var lis in list)
                {
                    ApprovalList approval = new ApprovalList
                    {
                        BookingId = lis.BookingId,
                        UserName = lis.UserName,
                        EndTime = lis.EndTime,
                        StartTime = lis.StartTime,
                        LabName = getLabName(lis.LabId)
                    };
                    approvalLists.Add(approval);
                }
                return approvalLists;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string GetLabName(int labId)
        {
            var mongoCollection = _db.GetCollection<LabList>("LabList").AsQueryable<LabList>()
                    .Where(e => e.LabId == labId
                    ).FirstOrDefault();
            return mongoCollection.LabName;
        }

        private string GetAdminMailId(int labId)
        {
            var mongoCollection = _db.GetCollection<LabList>("LabList").AsQueryable<LabList>()
                    .Where(e => e.LabId == labId
                    ).FirstOrDefault();
            return mongoCollection.LabAdminMail;
        }

        public string InsertLabSLot(LabModel labModel)
        {            
            try
            {
                LabModel labSlot = new LabModel()
                {
                    Approved = false,
                    BookingId = GetLatestBookId() + 1,
                    Email = labModel.Email,
                    EndTime = labModel.EndTime,
                    isApproved = false,
                    LabId = labModel.LabId,
                    StartTime = labModel.StartTime,
                    UserId = labModel.UserId,
                    UserName = labModel.UserName
                };
                labSlot.StartTime = labSlot.StartTime.AddHours(5.5);
                labSlot.EndTime = labSlot.EndTime.AddHours(5.5);
                _db.GetCollection<LabModel>("LabSlots").Insert(labSlot);
                SendMail(labSlot, 0);
            }
            catch (Exception)
            {

                throw;
            }
            return "Success";
        }

        public string ApproveLabSlot(int bookingId, bool approved)
        {
            try
            {
                var mongoCollection = _db.GetCollection<LabModel>("LabSlots");
                var documents = mongoCollection.AsQueryable().Where(e=>e.BookingId == bookingId).FirstOrDefault();
                //var requestedDoc = mongoCollection.FindOneById(documents.bookingId);
                if (CheckforSpeicificTime(mongoCollection.AsQueryable().ToList(), documents.StartTime, documents.EndTime))
                {
                    documents.Approved = approved;
                    documents.isApproved = true;
                    mongoCollection.Save(documents);
                    SendMail(documents, 1);
                }
                else
                    return "Slot is already Booked and approved or No longer Valid";

            }
            catch (Exception)
            {

                throw;
            }
            return "Success";
        }
        

        private bool CheckforSpeicificTime(List<LabModel> mongoCollection, DateTime startTime, DateTime endTime)
        {
            foreach(var doc in mongoCollection)
            {
                if ((startTime >= doc.StartTime && startTime < doc.EndTime) || (endTime <= doc.EndTime && endTime > doc.StartTime))
                {
                    if (doc.isApproved == true && doc.Approved == true)
                        return false;
                    
                }
            }

            return true;
        }

        private int GetLatestBookId()
        {
            try
            {
                var mongocollection = _db.GetCollection<LabModel>("LabSlots").FindAll().ToList();
                if (mongocollection.Count == 0)
                    return 1;
                else
                    return (mongocollection.Last().BookingId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SendMail(LabModel lab, int i)
        {
            var notifyMsg = new MailMessage();
            switch (i)
            {
                case 0:
                    notifyMsg = new MailMessage
                    {
                        Subject = "New Request for Lab Slot",
                        Body = $"User {lab.UserName} requested for lab {GetLabName(lab.LabId)} in timeslot from {lab.StartTime} to {lab.EndTime}. Kindly refer Dashboard for Approve/Decline",
                        From = new MailAddress(_settings.GetSMTPMailId())
                    };
                    notifyMsg.To.Add(new MailAddress(GetAdminMailId(lab.LabId)));
                    notifyMsg.CC.Add(new MailAddress(lab.Email));
                    break;

                case 1:
                    string approve = (lab.Approved == true ? "Approved" : "Declined");
                    notifyMsg = new MailMessage
                    {
                        Subject = "Lab Slot Approval Status",
                        Body = $"Your lab request is {approve} by Lab Admin",
                        From = new MailAddress(_settings.GetSMTPMailId())
                    };
                    break;
            }          

            var smtpSender = new SmtpClient(_settings.GetSMTPServer())
            {
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            smtpSender.Send(notifyMsg);
        }
               
    }
}
