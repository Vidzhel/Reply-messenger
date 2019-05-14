using System;

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

        #endregion

        #region public Members

        public int Id { get; private set; }

        public int SenderId { get; private set; }

        public int ReceiverId { get; private set; }

        public DataType DataType { get; private set; }

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

        public string Data { get; private set; }

        public MessageStatus Status { get; private set; }

        #endregion

        #region Constructor
        
        public Message(int senderId, int recieverId, DataType dataType, string UTCBin, string data, MessageStatus status = MessageStatus.SendingInProgress, int id = 0)
        {
            Id = id;
            SenderId = senderId;
            ReceiverId = recieverId;
            DataType = dataType;
            this.Date = UTCBin;
            Status = status;
            Data = data;
        }
        
        public Message(int senderId, int recieverId, DataType dataType, DateTime localTime, string data, MessageStatus status = MessageStatus.SendingInProgress, int id = 0)
        {
            Id = id;
            SenderId = senderId;
            ReceiverId = recieverId;
            DataType = dataType;
            LocalDate = localTime;
            Status = status;
            Data = data;
        }

        /// <summary>
        /// Constructor for dapper
        /// </summary>
        private Message() { }

        #endregion
    }
}
