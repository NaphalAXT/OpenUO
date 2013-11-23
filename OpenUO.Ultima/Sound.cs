﻿#region License Header
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
		private readonly SoundPlayer _player;

		public Sound(string name, Stream stream)
		{
			Name = name;
			_player = new SoundPlayer(stream);
		}

		public string Name { get; private set; }

		public void Dispose()
		{
			if (_player == null)
			{
				return;
			}

			_player.Dispose();
		}

		public void Play()
		{
			_player.Play();
		}

		public void PlayLooping()
		{
			_player.PlayLooping();
		}

		public void PlaySync()
		{
			_player.PlaySync();
		}

		public void Stop()
		{
			_player.Stop();
		}
	}
}