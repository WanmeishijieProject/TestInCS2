using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace GPAP.TCP
{
    public class TcpCommand : CommandBase<TcpSession, StringRequestInfo>
    {
        public override void ExecuteCommand(TcpSession session, StringRequestInfo requestInfo)
        {
            session.CustomID = new Random().Next(10000, 99999);
            session.CustomName = "hello word";

            var key = requestInfo.Key;
            var param = requestInfo.Parameters;
            var body = requestInfo.Body;
            //返回随机数session.Send(session.CustomID.ToString());
            //返回
            session.Send(body);
        }
    }
}
