using System;
using System.Collections.Generic;

namespace CommonLibs.Data
{
    /// <summary>
    /// Store information about message
    /// </summary>
    [Serializable]
    public class Message
    {

        #region Private Members

        DateTime date;

        DateTime lastTimeUpdated;

        #endregion

        #region public Members

        public int Id { get; private set; }

        public int SenderId { get; private set; }

        public int ReceiverId { get; private set; }

        public List<string> AttachmentsList = new List<string>();

        public DataType DataType { get; private set; }


        /// <summary>
        /// Get string representation of attachments list 
        /// </summary>
        public string Attachments
        {
            get
            {
                return DataConverter.ListToString(AttachmentsList);
            }

            set
            {
                AttachmentsList = DataConverter.StringToStrList(value);
            }
        }

        /// <summary>
        /// Get and set local DateTime
        /// </summary>
        public DateTime LocalDate {
            get
            {
                return date;
            }

            set
            {
                date = value;
            }
        }

        /// <summary>
        /// Converted binary representation of universal time
        /// </summary>
        public string Date {
            get
            {
                return date.ToUniversalTime().ToBinary().ToString();
            }
            set
            {
                date = DateTime.FromBinary((long)Convert.ToUInt64(value)).ToLocalTime();
            }
        }

        /// <summary>
        /// Get and set local last time updated DateTime
        /// </summary>
        public DateTime LocalLastTimeUpdated
        {
            get
            {
                return lastTimeUpdated;
            }

            set
            {
                lastTimeUpdated = value;
            }
        }

        /// <summary>
        /// Converted binary representation of universal time
        /// </summary>
        public string LastTimeUpdated
        {
            get
            {
                return lastTimeUpdated.ToUniversalTime().ToBinary().ToString();
            }
            set
            {
                lastTimeUpdated = DateTime.FromBinary((long)Convert.ToUInt64(value)).ToLocalTime();
            }
        }

        public string Data { get; private set; }

        public MessageStatus Status { get; set; }

        #endregion

        #region Constructor
        
        public Message(int senderId, int recieverId, DataType dataType, string UTCBin, string data,  MessageStatus status = MessageStatus.SendingInProgress, List<string> attachments = null, int id = 0)
        {
            Id = id;
            AttachmentsList = attachments;
            SenderId = senderId;
            ReceiverId = recieverId;
            DataType = dataType;
            LastTimeUpdated = UTCBin;
            this.Date = UTCBin;
            Status = status;
            Data = data;
        }
        
        public Message(int senderId, int recieverId, DataType dataType, DateTime localTime, string data, MessageStatus status = MessageStatus.SendingInProgress, List<string> attachments = null, int id = 0)
        {
            Id = id;
            AttachmentsList = attachments;
            SenderId = senderId;
            ReceiverId = recieverId;
            DataType = dataType;
            LocalDate = localTime;
            LocalLastTimeUpdated = localTime;
            Status = status;
            Data = data;
        }

        /// <summary>
        /// Constructor for dapper
        /// </summary>
        private Message() { }

        #endregion

        #region Public Methods

        public void AddAttachment(string name)
        {
            AttachmentsList.Add(name);
        }

        public void RemoveAttachment(string name)
        {
            AttachmentsList.Remove(name);
        }

        #endregion
    }
}
