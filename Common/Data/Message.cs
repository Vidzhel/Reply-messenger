using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    /// <summary>
    /// Store information about message
    /// </summary>
    public class Message
    {

        #region public Members

        public ulong Id { get; private set; }

        public long SenderId { get; private set; }

        public long RecieverId { get; private set; }

        public  DataType DataType { get; private set; }

        public DateTime Date { get; private set; }

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

        #endregion
    }
}
