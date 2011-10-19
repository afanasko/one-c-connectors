/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <andrei.mejov@gmail.com>
 * Date: 03.01.2010
 * Time: 16:09
 *  
 */

using System;
using System.Threading;
using NUnit.Framework;

namespace MOMAdapters
{
	[TestFixture]
	public class Test
	{
		[Test]
		public void TestLocator()
		{			
			//Тестирование автоматического обноружения компонентов, помеченных AutoLocate'ом
			Locator locator = new Locator();
			
			Assert.IsTrue(locator.GetComponentsList().Length>0);
			
			locator.GetComponent("MSMQAdapter");
			locator.GetComponent("JabberAdapter");
			locator.GetComponent("ActiveMQAdapter");
			locator.GetComponent("RabbitMQAdapter");
		}
		
		[Test]
		//[Ignore]
		public void TestRabbitMQ()
		{
			
			Locator locator = new Locator();
			IAdapter sender = (IAdapter)locator.GetComponent("RabbitMQAdapter");
			sender.SetParameter("UserName", "guest");
			sender.SetParameter("Password", "guest");
			sender.SetParameter("ReceiveDestinationName", "queue/first");
			sender.SetParameter("SendDestinationName", "queue/second");			
			
			IAdapter receiver = (IAdapter)locator.GetComponent("RabbitMQAdapter");
			receiver.SetParameter("UserName", "guest");
			receiver.SetParameter("Password", "guest");
			receiver.SetParameter("ReceiveDestinationName", "queue/second");
			receiver.SetParameter("SendDestinationName", "queue/first");	
			
			sender.Start();
			receiver.Start();
			
			try
			{
				//sender.Begin();
				sender.Send("ЙЦУКЕН");
				//sender.Commit();
				
				Thread.Sleep(3000);
				
				//receiver.Begin();
				Assert.IsTrue(receiver.HasMessage());
				Assert.AreEqual(receiver.Receive(), "ЙЦУКЕН");
				//receiver.Commit();
			}
			finally
			{
				sender.Stop();
				receiver.Stop();
			}
			
		}
		
		[Test]
		[Ignore]
		public void TestAcitveMQ()
		{
			Locator locator = new Locator();
			IAdapter sender = (IAdapter)locator.GetComponent("ActiveMQAdapter");
			sender.SetParameter("ActiveMQConnectionString", "tcp://localhost:61616");
			sender.SetParameter("ReceiveDestinationName", "queue/first");
			sender.SetParameter("SendDestinationName", "queue/second");			
			
			IAdapter receiver = (IAdapter)locator.GetComponent("ActiveMQAdapter");
			receiver.SetParameter("ActiveMQConnectionString", "tcp://localhost:61616");
			receiver.SetParameter("ReceiveDestinationName", "queue/second");
			receiver.SetParameter("SendDestinationName", "queue/first");	
			
			sender.Start();
			receiver.Start();
			try
			{
				sender.Begin();
				sender.Send("ЙЦУКЕН");
				sender.Commit();
				
				Thread.Sleep(3000);
				
				receiver.Begin();
				Assert.IsTrue(receiver.HasMessage());
				Assert.AreEqual(receiver.Receive(), "ЙЦУКЕН");
				receiver.Commit();
			}
			finally
			{
				sender.Stop();
				receiver.Stop();
			}
		}
		
		[Test]
		[Ignore]
		public void TestHighLoadActiveMQ()
		{
			Locator locator = new Locator();
			IAdapter sender = (IAdapter)locator.GetComponent("ActiveMQAdapter");
			sender.SetParameter("ActiveMQConnectionString", "tcp://localhost:61616");
			sender.SetParameter("ReceiveDestinationName", "queue/first");
			sender.SetParameter("SendDestinationName", "queue/second");			
			
			IAdapter receiver = (IAdapter)locator.GetComponent("ActiveMQAdapter");
			receiver.SetParameter("ActiveMQConnectionString", "tcp://localhost:61616");
			receiver.SetParameter("ReceiveDestinationName", "queue/second");
			receiver.SetParameter("SendDestinationName", "queue/first");	
			
			sender.Start();
			receiver.Start();
			try
			{				
				
				Thread senderThread = new Thread(
								delegate () 
								{
									long counter = 0;
									while (true)
									{
										sender.Begin();
										sender.Send("ЙЦУКЕН " + counter);
										//Console.WriteLine(">"+counter);
										sender.Commit();	
										counter++;
										Thread.Sleep(10);
									}
								}
												);				
				
				Thread receiverThread = new Thread(
								delegate () 
								{											
									long counter = 0;
									while (true)
									{
										receiver.Begin();						
										if (counter < 100)
										{
											counter++;
											receiver.Receive();
											receiver.Rollback();											
										}	
										else
										{
											counter = 0;
											while (receiver.HasMessage())
											{
												Console.WriteLine(receiver.Receive());
											}
											receiver.Commit();												
										}
									}
								}
												);				
				
				senderThread.Start();
				Thread.Sleep(3000);												
				receiverThread.Start();
				receiverThread.Join();
			}
			finally
			{
				sender.Stop();
				receiver.Stop();
			}
		}
				
		[Test]
		[Ignore]
		public void TestSimpleLoad()
		{
			Locator locator = new Locator();
			IAdapter sender = (IAdapter)locator.GetComponent("ActiveMQAdapter");
			sender.SetParameter("ActiveMQConnectionString", "tcp://localhost:61616");
			sender.SetParameter("ReceiveDestinationName", "queue/first");
			sender.SetParameter("SendDestinationName", "queue/second");			
			
			IAdapter receiver = (IAdapter)locator.GetComponent("ActiveMQAdapter");
			receiver.SetParameter("ActiveMQConnectionString", "tcp://localhost:61616");
			receiver.SetParameter("ReceiveDestinationName", "queue/second");
			receiver.SetParameter("SendDestinationName", "queue/first");	
			
			sender.Start();
			receiver.Start();
			try
			{				
				
				Thread senderThread = new Thread(
								delegate () 
								{									
									long counter = 0;
									while (true)
									{
										sender.Begin();
										sender.Send("ЙЦУКЕН " + counter);										
										sender.Commit();	
										counter++;
										Thread.Sleep(10);
									}
								}
												);				
				
				Thread receiverThread = new Thread(
								delegate () 
								{
									long counter = 0;
									while (true)
									{
										receiver.Begin();																
										while (receiver.HasMessage())
										{
											string s = receiver.Receive();
											Assert.AreEqual(counter, Convert.ToInt64(s.Split(' ')[1]));											
											Console.WriteLine(s + "|" + Convert.ToInt64(s.Split(' ')[1]) + "|" + counter);
											counter++;
										}
										receiver.Commit();
										Thread.Sleep(1000);
									}
								}
												);
				
				senderThread.Start();
				Thread.Sleep(3000);												
				receiverThread.Start();
				receiverThread.Join();
			}
			finally
			{
				sender.Stop();
				receiver.Stop();
			}
		}
		
	}
}
