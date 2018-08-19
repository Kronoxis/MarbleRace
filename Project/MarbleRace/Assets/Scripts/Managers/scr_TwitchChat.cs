using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchLib.Client.Models;
using TwitchLib.Client.Events;
using TwitchLib.Client.Services;
using TwitchLib.Api;
using TwitchLib.Unity;
using TwitchLib.Secrets;
using UnityEngine;

public class scr_TwitchChat : scr_Singleton<scr_TwitchChat>
{
	private Client m_Client;
	private TwitchAPI m_Api;

	public List<string> JoinedUsers = new List<string>();

	private void Start()
	{
		ConnectionCredentials credentials = new ConnectionCredentials("marblerace", Secrets.AccessToken);
		m_Client = new Client();
		m_Client.Initialize(credentials, scr_InputManager.ChannelName);

		m_Client.OnConnected += OnConnected;
		m_Client.OnConnectionError += OnConnectionError;
		m_Client.OnMessageReceived += OnMessageReceived;
		m_Client.OnChatCommandReceived += OnCommandReceived;
		m_Client.OnMessageSent += OnMessageSent;
		m_Client.ChatThrottler = new MessageThrottler(m_Client, 20, TimeSpan.FromSeconds(30));
		m_Client.ChatThrottler.StartQueue();

		m_Client.Connect();

		m_Api = new TwitchAPI();
		m_Api.Settings.AccessToken = Secrets.AccessToken;
		m_Api.Settings.ClientId = Secrets.ClientID;
	}

	public async Task<string> GetProfileImage(string username)
	{
		var response = await m_Api.Users.helix.GetUsersAsync(logins: new List<string> { username });
		if (response.Users.Length < 0)
			return null;
		return response.Users[0].ProfileImageUrl;
	}

	private void OnConnected(object sender, OnConnectedArgs e)
	{
		m_Client.SendMessage(scr_InputManager.ChannelName, "MarbleRace " + scr_InputManager.MajorVersion + "." + scr_InputManager.MinorVersion + " loaded.");
	}

	private void OnConnectionError(object sender, OnConnectionErrorArgs e)
	{
		m_Client.SendMessage(scr_InputManager.ChannelName,
							 "Oof, something went wrong. Send Kronoxis my regards with the following message attached "
							 + " (or try to solve it yourself if you feel adventurous): " + e.Error.Exception.StackTrace);
	}

	private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
	{
		//Debug.Log("Message received: " + (e.ChatMessage.IsModerator ? "[MOD] " : "") + e.ChatMessage.Username + ": " + e.ChatMessage.Message);
		Debug.Log(e.ChatMessage.Username + ": " + e.ChatMessage.UserId);
	}

	private void OnCommandReceived(object sender, OnChatCommandReceivedArgs e)
	{
		if (scr_InputManager.IsJoinable)
		{
			if (e.Command.CommandText.Equals("gjoin", StringComparison.InvariantCultureIgnoreCase))
			{
				if (JoinedUsers.Contains(e.Command.ChatMessage.DisplayName))
				{
					m_Client.SendMessage(scr_InputManager.ChannelName, e.Command.ChatMessage.DisplayName + ", hold your horses. You've already joined this giveaway! DansGame");
				}
				else
				{
					JoinedUsers.Add(e.Command.ChatMessage.DisplayName);
					m_Client.SendMessage(scr_InputManager.ChannelName, e.Command.ChatMessage.DisplayName + " has joined the giveaway! PogChamp");
				}
			}
		}
	}

	private void OnMessageSent(object sender, OnMessageSentArgs e)
	{
		//Debug.Log("Sent message: " + e.SentMessage.Message);
	}
}