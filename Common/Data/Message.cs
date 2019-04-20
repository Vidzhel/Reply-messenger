using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    class Message : IData
    {

        #region public Members

        public readonly long SenderId;

        public readonly long RecieverId;

        public readonly DataType DataType;

        public readonly DateTime Date;

        public string Data;

        public MessageStatus Status;

        #endregion

        #region Constructor

        public Message(long senderId, long recieverId, DataType dataType, DateTime date, MessageStatus status, string data)
        {
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
