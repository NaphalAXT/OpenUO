﻿#region License Header
// /***************************************************************************
//  *   Copyright (c) 2011 OpenUO Software Team.
//  *   All Right Reserved.
//  *
//  *   AnimationDataStorageAdapter.cs
//  *
//  *   This program is free software; you can redistribute it and/or modify
//  *   it under the terms of the GNU General Public License as published by
//  *   the Free Software Foundation; either version 3 of the License, or
//  *   (at your option) any later version.
//  ***************************************************************************/
#endregion

#region References
using System;
using System.Collections.Generic;
using System.IO;
#endregion

namespace OpenUO.Ultima.Adapters
{
	public class AnimationDataStorageAdapter : StorageAdapterBase, IAnimationDataStorageAdapter<AnimationData>
	{
		private AnimationData[] _AnimationData;

		public override int Length
		{
			get
			{
				if (!IsInitialized)
				{
					Initialize();
				}

				return _AnimationData.Length;
			}
		}

		public override void Initialize()
		{
			base.Initialize();

			InstallLocation install = Install;

			var animationData = new List<AnimationData>();

			using (FileStream stream = File.Open(install.GetPath("animdata.mul"), FileMode.Open))
			{
				using (BinaryReader reader = new BinaryReader(stream))
				{
					int totalBlocks = (int)(reader.BaseStream.Length / 548);

					for (int i = 0; i < totalBlocks; i++)
					{
						int header = reader.ReadInt32();
						var frameData = reader.ReadBytes(64);

						AnimationData animData = new AnimationData {
							FrameData = new sbyte[64],
							Unknown = reader.ReadByte(),
							FrameCount = reader.ReadByte(),
							FrameInterval = reader.ReadByte(),
							FrameStart = reader.ReadByte()
						};

						Buffer.BlockCopy(frameData, 0, animData.FrameData, 0, 64);
						animationData.Add(animData);
					}
				}
			}

			_AnimationData = animationData.ToArray();
		}

		public AnimationData GetAnimationData(int index)
		{
			if (index < _AnimationData.Length)
			{
				return _AnimationData[index];
			}

			return AnimationData.Empty;
		}
	}
}