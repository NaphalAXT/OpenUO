﻿#region License Header
// /***************************************************************************
//  *   Copyright (c) 2011 OpenUO Software Team.
//  *   All Right Reserved.
//  *
//  *   SoundStorageAdapter.cs
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
using System.Text;
#endregion

namespace OpenUO.Ultima.Adapters
{
	public class SoundStorageAdapter : StorageAdapterBase, ISoundStorageAdapter<Sound>
	{
		private FileIndexBase _fileIndex;

		public override int Length
		{
			get
			{
				if (!IsInitialized)
				{
					Initialize();
				}

				return _fileIndex.Length;
			}
		}

		public override void Initialize()
		{
			base.Initialize();

			InstallLocation install = Install;

			_fileIndex = install.IsUOPFormat
							 ? install.CreateFileIndex("soundLegacyMUL.uop", 0xFFF, false, ".dat")
							 : install.CreateFileIndex("soundidx.mul", "sound..mul");
		}

		public Sound GetSound(int index)
		{
			int length, extra;
			Stream stream = _fileIndex.Seek(index, out length, out extra);

			if (stream == null)
			{
				return null;
			}

			var waveHeader = CreateWaveHeader(length);

			length -= 40;

			int headerLength = (waveHeader.Length << 2);

			var stringBuffer = new byte[40];
			var buffer = new byte[length + headerLength];

			Buffer.BlockCopy(waveHeader, 0, buffer, 0, headerLength);

			stream.Read(stringBuffer, 0, 40);
			stream.Read(buffer, headerLength, length);

			string name = Encoding.ASCII.GetString(stringBuffer).Trim();
			int end = name.IndexOf("\0");
			name = name.Substring(0, end);

			return new Sound(name, new MemoryStream(buffer));
		}

		private static int[] CreateWaveHeader(int length)
		{
			/* ====================
             * = WAVE File layout =
             * ====================
             * char[4] = 'RIFF' \
             * int - chunk size |- Riff Header
             * char[4] = 'WAVE' /
             * char[4] = 'fmt ' \
             * int - chunk size |
             * short - format	|
             * short - channels	|
             * int - samples p/s|- Format header
             * int - avg bytes	|
             * short - align	|
             * short - bits p/s /
             * char[4] - data	\
             * int - chunk size | - Data header
             * short[..] - data /
             * ====================
             * */
			return new[]
			{
				0x46464952, (length + 12), 0x45564157, 0x20746D66, 0x10, 0x010001, 0x5622, 0xAC44, 0x100002, 0x61746164,
				(length - 24)
			};
		}
	}
}