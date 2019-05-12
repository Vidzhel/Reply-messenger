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

        public ulong Id { get; private set; }

        public long SenderId { get; private set; }

        public long ReceiverId { get; private set; }

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
                return date.ToUniversalTime().ToString();
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
        
        public Message(long senderId, long recieverId, DataType dataType, string UTCBin, string data, MessageStatus status = MessageStatus.SendingInProgress, ulong id = 0)
        {
            Id = id;
            SenderId = senderId;
            ReceiverId = recieverId;
            DataType = dataType;
            this.Date = UTCBin;
            Status = status;
            Data = data;
        }
        
        public Message(long senderId, long recieverId, DataType dataType, DateTime localTime, string data, MessageStatus status = MessageStatus.SendingInProgress, ulong id = 0)
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
