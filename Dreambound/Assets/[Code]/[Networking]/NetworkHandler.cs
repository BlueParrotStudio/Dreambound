﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

using Dreambound.Networking.DataHandling;
using Dreambound.Networking.Utility;
using Dreambound.Networking.LoginSystem;

namespace Dreambound.Networking
{
    public class NetworkHandler
    {
        private Socket _socket;

        private bool _connected = false;
        private ByteBuffer _buffer;

        public void ConnectUsingSettings(string ip, int port)
        {
            //Disconnect the previous socket and make a new instance
            DisconnectPreviousSocket();
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                _socket.Connect(ip, port);
            }
            catch
            {
                Debug.LogWarning("Connection could not be made");
            }

            if (_socket.Connected)
            {
                Client client = new Client(_socket, new DataManager());
                _connected = true;
            }

            if (_buffer == null)
                _buffer = new ByteBuffer();
        }

        private void DisconnectPreviousSocket()
        {
            if (_socket != null)
            {
                _socket.Close();
                _socket = null;

                Debug.Log("Socket has disconnected");
            }
        }

        public void Login(string email, string password)
        {
            if (!_connected)
                return;

            _buffer.Clear();
            _buffer.WriteInt((int)PacketType.LoginRequest);
            _buffer.WriteString(email);
            _buffer.WriteString(password);

            NetworkSender.SendPacket(_buffer, _socket);
        }
        public void SendAccountInfo()
        {
            if (!_connected)
                return;


            _buffer.Clear();
            _buffer.WriteInt((int)PacketType.AccountInfo);
            //_buffer.WriteString(UserData.Username);
            //_buffer.WriteInt(UserData.ID);

            //REMOVE THS LATER!!!
            _buffer.WriteString("test1");
            _buffer.WriteInt(1);

            NetworkSender.SendPacket(_buffer, _socket);
        }
    }
}