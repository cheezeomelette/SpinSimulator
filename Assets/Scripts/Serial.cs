using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Serial : MonoBehaviour
{
	[SerializeField] AxisManager axisManager;
	[SerializeField] Text recieveDataText;
	[SerializeField] Text transmissionTimeText;

	public bool isConnected => m_SerialPort.IsOpen;
	SerialPort m_SerialPort = new SerialPort();

	private Thread thread;
	byte[] recieveData = new byte[8];
	Queue<byte> recieveQ = new Queue<byte>();

	const string TRTIME_FORMAT = "{0} ms";
	float transmissionTime;

	const byte HEAD_ALL	= 0xFF;
	const byte HEAD_X	= 0xFB;

	float x;
	float y;
	float z;
	
	bool isStop = false;

	static string[] GetPortNames()
	{
		return System.IO.Ports.SerialPort.GetPortNames();
	}

	void Start()
	{
		string[] strs = GetPortNames();
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
		if(thread!= null)
		Debug.Log(thread.ThreadState);

		UnPacking();
		if (isStop)
			return;
		transmissionTime += Time.deltaTime;
		UpdateTransmissionTime();
		if (recieveData[1] == HEAD_X)
		{
			recieveDataText.text = RecieveDataToString();
		}
		SetAllAxis(x, y, z);
	}

	public void PortSettings(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
	{
		m_SerialPort.PortName = portName;
		m_SerialPort.BaudRate = baudRate;
		m_SerialPort.DataBits = dataBits;
		m_SerialPort.Parity = parity;
		m_SerialPort.StopBits = stopBits;
	}

	public void ConnectSerialPort()
	{
		m_SerialPort.Open();
		if (thread == null)
			thread = new Thread(DataRecieveThread);
		thread.Start();
	}

	public void DisconnectSerialPort()
	{
		m_SerialPort.Close();
		thread.Abort();
		thread.Join();
	}

	public void ClickStopButton()
	{
		isStop = !isStop;
	}

	private void DataRecieveThread()
	{
		while (true)
		{
			Thread.Sleep(10);
			try
			{
				if (m_SerialPort.IsOpen)
				{
					Thread.Sleep(100);
					while (m_SerialPort.BytesToRead > 0)
					{
						byte data = (byte)m_SerialPort.ReadByte();
						recieveQ.Enqueue(data);
					}

					m_SerialPort.ReadTimeout = 300;
				}
			}

			catch (Exception e)
			{
				Debug.Log(e);
			}
		}
	}

	private void UnPacking()
	{
		if (recieveQ.Count < 52)
			return;

		while (recieveQ.Peek() != HEAD_ALL && recieveQ.Count > 8)
		{
			recieveQ.Dequeue();
			continue;
		}

		for (int i = 0; i < 8; i++)
		{
			byte oneByte = recieveQ.Dequeue();
			recieveData[i] = oneByte;
		}

		if (recieveData[1] == HEAD_X)
		{
			//recieveDataText.text = RecieveDataToString();
			byte[] xByteArr = new byte[2] { recieveData[2], recieveData[3] };
			byte[] yByteArr = new byte[2] { recieveData[4], recieveData[5] };
			byte[] zByteArr = new byte[2] { recieveData[6], recieveData[7] };

			Array.Reverse(xByteArr);
			Array.Reverse(yByteArr);
			Array.Reverse(zByteArr);

			x = BitConverter.ToInt16(xByteArr) / 100f;
			y = BitConverter.ToInt16(yByteArr) / 100f;
			z = BitConverter.ToUInt16(zByteArr) / 100f;

			//x = BitConverter.ToInt16(recieveData, 2) / 100f;
			//y = BitConverter.ToInt16(recieveData, 4) / 100f;
			//z = BitConverter.ToUInt16(recieveData, 6) / 100f;
		}
	}

	private string RecieveDataToString()
	{
		StringBuilder builder = new StringBuilder();

		for (int i = 0; i < recieveData.Length; i++)
		{
			builder.AppendFormat("{0:X2}", recieveData[i]);
			builder.Append(i % 8 == 7 ? '\n' : ' ');
		}
		return builder.ToString();
	}

	private float[] ParseSerialData(string data, out bool isValid)
	{
		float[] axisDatas = { 0, 0, 0 };
		string[] splitData = data.Split(',');

		isValid = splitData.Length == 3 ? true : false;
			
		for(int i = 0; i < axisDatas.Length; i++)
		{
			axisDatas[i] = float.Parse(splitData[i]);
		}

		return axisDatas;
	}

	private void SetAllAxis(float x, float y, float z)
	{
		axisManager.SetAllAxis(x, y, z);
	}

	void OnApplicationQuit()
	{
		m_SerialPort.Close();
		thread.Abort();
	}
	
	void UpdateTransmissionTime()
	{
		transmissionTimeText.text = string.Format(TRTIME_FORMAT, (int)(transmissionTime * 1000));
		transmissionTime = 0f;
	}
}
