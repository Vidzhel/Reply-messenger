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

        public long RecieverId { get; private set; }

        public DataType DataType { get; private set; }

        //Local Time
        public DateTime Date {

            get {

                //Check if date isn't null, if it is, then convert from UTCBin to Local Time
                if (date == null)
                    return DateTime.FromBinary(Convert.ToInt64(UTCBin)).ToLocalTime();


                return date;
            }
            set
            {
                date = value;
            }
        }

        /// <summary>
        /// Binary representation of UTC time 
        /// </summary>
        public string UTCBin { get; private set; }

        public string Data { get; private set; }

        public MessageStatus Status { get; private set; }

        #endregion

        #region Constructor

        public Message(long senderId, long recieverId, DataType dataType, DateTime date, string data, MessageStatus status = MessageStatus.SendingInProgress, ulong id = 0)
        {
            Id = id;
            SenderId = senderId;
            RecieverId = recieverId;
            DataType = dataType;
            Date = date;
            Status = status;
            Data = data;
        }

        public Message(long senderId, long recieverId, DataType dataType, string UTCBin, string data, MessageStatus status = MessageStatus.SendingInProgress, ulong id = 0)
        {
            Id = id;
            SenderId = senderId;
            RecieverId = recieverId;
            DataType = dataType;
            this.UTCBin = UTCBin;
            Status = status;
            Data = data;
        }

        /// <summary>
        /// Constructor for dapper
        /// </summary>
        public Message() { }
        #endregion
    }
}
