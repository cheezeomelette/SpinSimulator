using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

public class SerialConnection : MonoBehaviour
{
	[SerializeField] Dropdown comDropdown;
	[SerializeField] Dropdown baudRateDropdown;
	[SerializeField] Serial serial;
	[SerializeField] Text errorText;

	string portName;
	int baudRate = 115200;
	Parity parity = Parity.None;
	int dataBits = 8;
	StopBits stopBits = StopBits.One;
	Coroutine errorRoutine;

	private void Start()
	{
		SetConnectedPortList();
	}

	public void SetComPort(int optionIndex)
	{
		portName = comDropdown.options[optionIndex].text;
	}

	public void SetConnectedPortList()
	{
		comDropdown.options.Clear();
		string[] ports = SerialPort.GetPortNames();
		if (ports.Length == 0)
			ShowErrorRoutine(string.Join(',', ports));
		foreach (string portName in ports)
		{
			Dropdown.OptionData newOptionData = new Dropdown.OptionData(portName);
			comDropdown.options.Add(newOptionData);
		}
		comDropdown.captionText.text = comDropdown.options[comDropdown.value].text;
		SetComPort(comDropdown.value);
	}

	public void ConnectSerialPort()
	{
		if (serial.isConnected)
		{
			ShowErrorRoutine("Already connected");
			return;
		}

		serial.PortSettings(portName, baudRate, parity, dataBits, stopBits);
		serial.ConnectSerialPort();
	}

	public void DisconnectSerialPort()
	{
		if(!serial.isConnected)
		{
			ShowErrorRoutine("Nothing connected");
			return;
		}

		serial.DisconnectSerialPort();
	}

	private void ShowErrorRoutine(string error)
	{
		if(errorRoutine != null)
			StopCoroutine(errorRoutine);

		errorRoutine = StartCoroutine(ShowError(error));
	}

	IEnumerator ShowError(string error)
	{
		errorText.enabled = true;
		errorText.text = error;
		yield return new WaitForSeconds(1);
		errorText.enabled = false;
	}
}
