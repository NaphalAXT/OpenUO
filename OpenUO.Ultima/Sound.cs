#region License Header
// /***************************************************************************
//  *   Copyright (c) 2011 OpenUO Software Team.
//  *   All Right Reserved.
//  *
//  *   Sound.cs
//  *
//  *   This program is free software; you can redistribute it and/or modify
//  *   it under the terms of the GNU General Public License as published by
//  *   the Free Software Foundation; either version 3 of the License, or
//  *   (at your option) any later version.
//  ***************************************************************************/
#endregion

#region References
using System;
using System.IO;
using System.Media;
#endregion

namespace OpenUO.Ultima
{
	public class Sound : IDisposable
	{
		private readonly SoundPlayer _Player;

		public Sound(string name, Stream stream)
		{
			Name = name;
			_Player = new SoundPlayer(stream);
		}

		public string Name { get; private set; }

		public void Dispose()
		{
			if (_Player == null)
			{
				return;
			}

			_Player.Dispose();
		}

		public void Play()
		{
			_Player.Play();
		}

		public void PlayLooping()
		{
			_Player.PlayLooping();
		}

		public void PlaySync()
		{
			_Player.PlaySync();
		}

		public void Stop()
		{
			_Player.Stop();
		}
	}
}