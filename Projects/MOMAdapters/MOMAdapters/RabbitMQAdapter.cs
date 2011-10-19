/*
 * User: lustin (isthisdesign@gmail.com)
 * Date: 19.10.2011
 * Time: 10:17
 * 

 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using RabbitMQ.Client;

namespace MOMAdapters
{
	[AutoLocate("RabbitMQAdapter")]
	[ComVisible(true)]	
	[ComSourceInterfacesAttribute(typeof(IAdapter))]	
	[ClassInterface(ClassInterfaceType.AutoDual)]	
	[ProgId("MOMAdapters.RabbitMQAdapter")]
	[Guid("B329D40F-2AAB-4316-AA62-8E1750444F93")]	
	/// <summary>
	///adapter for RabbitMQ
	/// </summary>
	public class RabbitMQAdapter : IAdapter
	{
		private static readonly string AdapterType = "RabbitMQ";
		
		private int PortNumber = AmqpTcpEndpoint.UseDefaultPort;
		
		private IConnection          connection = null;
		
		private IModel    			sendDestination = null;
		private IModel 				receiveDestination = null;
		
		private string	sendFinalQuerry = "";
		private string	receiveFinalQuerry = "";
		
		public static readonly string         ReceiveDestinationName = "ReceiveDestinationName";
		public static readonly string            SendDestinationName = "SendDestinationName";
		
		private Dictionary<string, string>      parameters = new Dictionary<string, string>();
	
		[ComVisible(false)]
		public Dictionary<string, string> Parameters
		{
			get {return parameters;}			
		}
		
		public string GetAdapterType() 
		{
			return AdapterType;
		}
		
		
		public void SetParameter(string _name, string _value)
		{
			parameters.Add(_name, _value);
		}
		
		public void SetPort(int port)
		{
			PortNumber = port;	
		}
		
		public void ClearParameters()
		{
			parameters.Clear();
		}
		
			
		public void Start()
		{
			ValidateParameters();
			
			ConnectionFactory factory = new ConnectionFactory();
			if ((Parameters.ContainsKey("UserName")) && (Parameters.ContainsKey("Password")))
			{
				factory.UserName = Parameters["UserName"]; 
				factory.Password = Parameters["Password"];
			}

			factory.Protocol = Protocols.FromEnvironment();
			
			factory.VirtualHost = GetParameterWithDefault("vhost","/");
			factory.HostName = GetParameterWithDefault("hostName","localhost");
			factory.Port = PortNumber;
			
			connection = factory.CreateConnection();	

			if (Parameters.ContainsKey(SendDestinationName))
			{
				sendDestination = connection.CreateModel();
				sendFinalQuerry = sendDestination.QueueDeclare(Parameters[SendDestinationName], false, false, false, null);
			}
			
			if (Parameters.ContainsKey(ReceiveDestinationName))
			{
				receiveDestination = connection.CreateModel();
				receiveFinalQuerry = sendDestination.QueueDeclare(Parameters[ReceiveDestinationName], false, false, false, null);				
			}			
		}
		
		public void Stop()
		{
			if (connection != null)
			{
				if (sendDestination != null) {
					sendDestination.Close();
				}
				
				if (receiveDestination != null) {
					receiveDestination.Close();
				}
				connection.Close();
			}
			
		}
		
		public void SendFile(string _text)
		{
		
		}
		
		public void Send(string _text)
		{
		
			sendDestination.BasicPublish("",sendFinalQuerry,null,
			                             Encoding.UTF8.GetBytes(_text));
		}
		
		public bool HasMessage()
		{
			if (receiveDestination == null)
			{
				throw new Exception("This connector not configured for receiving");
			}
			
			BasicGetResult result = receiveDestination.BasicGet(receiveFinalQuerry,false);
			return (result != null);
		}
		
		public string Receive()
		{
			if (receiveDestination == null)
			{
				throw new Exception("This connector not configured for receiving");
			}
			
			BasicGetResult result = receiveDestination.BasicGet(receiveFinalQuerry,false);
			if (result == null) {
                 return null;
            } else {
                 receiveDestination.BasicAck(result.DeliveryTag, false);
                 return Encoding.UTF8.GetString(result.Body);
            }
		}
		
		
		public void Begin()
		{
			throw new NotImplementedException();
		}
		public void Commit()
		{
			throw new NotImplementedException();
		}
		public void Rollback()
		{
			throw new NotImplementedException();
		}
		
		private void ValidateParameters()
		{
			if (!((Parameters.ContainsKey(SendDestinationName)) || (Parameters.ContainsKey(ReceiveDestinationName))))
			{
				throw new Exception("SendDestinationName and/or ReceiveDestinationName must be set");
			}
			if ((Parameters.ContainsKey("UserName")) && (!Parameters.ContainsKey("Password")))
			{
				throw new Exception("Password must not be empty");
			}
		}
	
		private string GetParameterWithDefault(string paramName, string defaultValue)
		{
		
			if (Parameters.ContainsKey(paramName))
			{
			    return Parameters[paramName];
			} 
			else 
			{
				return defaultValue;
			}
			
		}
		
		public void Dispose()
		{
			Stop();			
		}
		
	}
}
