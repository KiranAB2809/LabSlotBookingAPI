using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class LabModel
    {
        public ObjectId _id { get; set; }

        [BsonElement("BookingId")]
        public int BookingId { get; set; }
        [BsonElement("LabId")]
        public int LabId { get; set; }

        [BsonElement("StartTime")]
        public DateTime StartTime { get; set; }

        [BsonElement("EndTime")]
        public DateTime EndTime { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }

        [BsonElement("UserName")]
        public string UserName { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("Approved")]
        public bool Approved { get; set; }

        [BsonElement("AdminApproved")]
        public bool isApproved { get; set; }

    }

    public class LabList
    {
        public ObjectId _id { get; set; }

        [BsonElement("LabId")]
        public int LabId { get; set; }

        [BsonElement("LabName")]
        public string LabName { get; set; }

        [BsonElement("LabAdminId")]
        public string LabAdminId { get; set; }

        [BsonElement("LabAdminMail")]
        public string LabAdminMail { get; set; }
    }

    public class ApprovalList
    {
        public int BookingId { get; set; }
        public string UserName { get; set; }
        public string LabName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
